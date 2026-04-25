using System.Security.Claims;

using Brew.Application.Dto;

namespace Brew.Application.ServiceContracts;

public interface IJwtService
{
    TokenDto GenerateAccessToken(IEnumerable<Claim> claims);
    TokenDto GenerateRefreshToken();
}