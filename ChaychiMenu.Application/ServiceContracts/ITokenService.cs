using System.Security.Claims;

using ChaychiMenu.Application.Dto;

namespace ChaychiMenu.Application.ServiceContracts;

public interface ITokenService
{
    TokenDto GenerateAccessToken(IEnumerable<Claim> claims);
    TokenDto GenerateRefreshToken();
}