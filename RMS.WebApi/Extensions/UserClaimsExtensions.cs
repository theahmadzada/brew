using System.Security.Claims;

namespace RMS.WebApi.Extensions;

public static class UserClaimsExtensions
{
    public static Guid? GetUserId(this ClaimsPrincipal principal)
    {
        var id = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Guid.TryParse(id, out var parsedId);
        return parsedId;
    }
}