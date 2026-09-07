using System.Security.Claims;

using ErrorOr;

using MediatR;

using Microsoft.AspNetCore.Identity;

using RMS.Application.Commands.AppUser;
using RMS.Application.Dto;
using RMS.Application.ServiceContracts;

namespace RMS.Application.Handlers.AppUser;

public class LogInCommandHandler(
    UserManager<Domain.Entities.AppUser> userManager,
    ITokenService tokenService) : IRequestHandler<LogInCommand, ErrorOr<AuthDto>>
{
    public async Task<ErrorOr<AuthDto>> Handle(LogInCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return Error.Unauthorized("User.InvalidCredentials", "Invalid email or password");

        var lockedOut = await userManager.IsLockedOutAsync(user);
        if(lockedOut)
            return Error.Unauthorized("User.LockedOut", "You are not allowed to log in");
        
        var result = await userManager.CheckPasswordAsync(user, request.Password);
        if(!result)
        {
            var accessFailed = await userManager.AccessFailedAsync(user);
            if(!accessFailed.Succeeded)
                return accessFailed.Errors
                    .Select(x => Error.Unexpected(x.Code, x.Description))
                    .ToList();
            
            return Error.Unauthorized("User.InvalidCredentials", "Invalid email or password");
        }

        var resetAccessFailedCount = await userManager.ResetAccessFailedCountAsync(user);
        if (!resetAccessFailedCount.Succeeded)
            return resetAccessFailedCount.Errors
                .Select(x => Error.Unexpected(x.Code, x.Description))
                .ToList();
        
        var roles = await userManager.GetRolesAsync(user);
        var claims = new List<Claim>([
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("jti", Guid.NewGuid().ToString()),
        ]);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var accessTokenData = tokenService.GenerateAccessToken(claims);
        var refreshTokenData = tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshTokenData.Token;
        user.RefreshTokenExpires = refreshTokenData.Expires;
        
        var updateResult = await userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
            return updateResult.Errors
                .Select(x => Error.Unexpected(x.Code, x.Description))
                .ToList();
        
        return new AuthDto()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email!,
            AccessToken = accessTokenData.Token,
            AccessTokenExpiration = accessTokenData.Expires,
            RefreshToken = refreshTokenData.Token,
            RefreshTokenExpiration = refreshTokenData.Expires,
        };
    }
}