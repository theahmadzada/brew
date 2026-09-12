using System.Security.Claims;

namespace ChaychiMenu.WebApi.Filters;

public class RequireAppRoleFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var user = context.HttpContext.User;

        var userRole = user.FindFirst(ClaimTypes.Role)?.Value;
        if (userRole is null)
            return Results.Unauthorized();

        context.HttpContext.Items["UserRole"] = userRole;

        return await next(context);
    }
}