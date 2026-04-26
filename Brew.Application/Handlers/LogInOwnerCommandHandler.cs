using System.Security.Claims;

using Brew.Application.Commands;
using Brew.Application.Dto;
using Brew.Application.ServiceContracts;
using Brew.Domain.Entities;

using ErrorOr;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Brew.Application.Handlers;

public class LogInOwnerCommandHandler(
    UserManager<AppUser> userManager,
    ITokenService tokenService) : IRequestHandler<LogInOwnerCommand, ErrorOr<AuthDto>>
{
    public async Task<ErrorOr<AuthDto>> Handle(LogInOwnerCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return Error.NotFound("User.NotFound", "User not found");

        var result = await userManager.CheckPasswordAsync(user, request.Password);
        if(!result)
            return Error.Unauthorized("User.Unauthorized", "User unauthorized");

        var roles = await userManager.GetRolesAsync(user);
        var claims = new List<Claim>([
            new Claim(JwtRegisteredClaimNames.Email, request.Email),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
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