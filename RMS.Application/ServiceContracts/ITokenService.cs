using System.Security.Claims;

using RMS.Application.Dto;

namespace RMS.Application.ServiceContracts;

public interface ITokenService
{
    TokenDto GenerateAccessToken(IEnumerable<Claim> claims);
    TokenDto GenerateRefreshToken();
}