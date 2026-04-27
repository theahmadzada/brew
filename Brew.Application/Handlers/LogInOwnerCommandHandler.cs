using System.Security.Claims;

using Brew.Application.Commands;
using Brew.Application.Dto;
using Brew.Application.ServiceContracts;
using Brew.Domain.Entities;

using ErrorOr;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace Brew.Application.Handlers;

public class LogInOwnerCommandHandler(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    ITokenService tokenService) : IRequestHandler<LogInOwnerCommand, ErrorOr<AuthDto>>
{
    public async Task<ErrorOr<AuthDto>> Handle(LogInOwnerCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return Error.Unauthorized("User.InvalidCredentials", "Invalid email or password");

        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, true);
        if(!result.Succeeded)
            return Error.Unauthorized("User.InvalidCredentials", "Invalid email or password");

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