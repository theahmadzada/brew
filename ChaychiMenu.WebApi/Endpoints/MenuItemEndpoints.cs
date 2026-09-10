using System.Security.Claims;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using ChaychiMenu.Application.Commands.MenuItem;
using ChaychiMenu.Application.Dto;
using ChaychiMenu.WebApi.Extensions;

namespace ChaychiMenu.WebApi.Endpoints;

public static class MenuItemEndpoints
{
    public static WebApplication MapMenuItemEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("api/menu-items");

        group.MapPost("/", async (
            [FromForm] CreateMenuItemDto dto,
            ClaimsPrincipal user,
            ISender mediator,
            CancellationToken cancellationToken) =>
        {
            var id = user.GetAppUserId();
            if (id is null)
                return Results.Unauthorized();
            
            var userRole = user.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole is null)
                return Results.Unauthorized();
            
            var request = new CreateMenuItemCommand
            {
                AppUserId = id.Value,
                Title = dto.Title,
                Description = dto.Description,
                CategoryId = dto.CategoryId,
                Price = dto.Price,
                Order = dto.Order,
                Image = dto.Image?.OpenReadStream(),
                ContentType = dto.Image?.ContentType,
                UserRole = userRole,
            };
            var result = await mediator.Send(request, cancellationToken);
            return result.Match(value => Results.Ok(value), errors => errors.ToProblem());
        }).DisableAntiforgery();

        group.MapPatch("/id", async (
            Guid id,
            [FromBody] ToggleMenuItemAvailabilityDto request,
            ClaimsPrincipal user,
            ISender mediator,
            CancellationToken cancellationToken) =>
        {
            var userId = user.GetAppUserId();
            if (userId is null)
                return Results.Unauthorized();
            
            var userRole = user.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole is null)
                return Results.Unauthorized();
            
            var command = new ToggleMenuItemAvailabilityCommand()
            {
                MenuItemId = id, 
                IsAvailable = request.IsAvailable,
                AppUserId = userId.Value,
                UserRole = userRole,
            };
            var result = await mediator.Send(command, cancellationToken);
            return result.Match(value => Results.Ok(value), errors => errors.ToProblem());
        });
        
        return app;
    }
}