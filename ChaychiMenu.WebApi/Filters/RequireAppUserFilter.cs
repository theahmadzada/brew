using System.Security.Claims;

namespace ChaychiMenu.WebApi.Filters;

public class RequireAppUserFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var user = context.HttpContext.User;

        var appUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (appUserId is null)
            return Results.Unauthorized();
        
        context.HttpContext.Items["AppUserId"] = appUserId;

        return await next(context);
    }
}