using System.Security.Claims;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using RMS.Application.Commands.MenuItem;
using RMS.Application.Dto;
using RMS.WebApi.Extensions;

namespace RMS.WebApi.Endpoints;

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
            
            var request = new CreateMenuItemCommand
            {
                AppUserId = id.Value,
                Title = dto.Title,
                Description = dto.Description,
                CategoryId = dto.CategoryId,
                Price = dto.Price,
                Order = dto.Order,
                Image = dto.Image?.OpenReadStream(),
                ContentType = dto.Image?.ContentType
            };
            var result = await mediator.Send(request, cancellationToken);
            return result.Match(value => Results.Ok(value), errors => errors.ToProblem());
        }).DisableAntiforgery();

        group.MapPatch("/id", async (
            Guid id,
            [FromBody] ToggleMenuItemAvailabilityDto request,
            ISender mediator,
            CancellationToken cancellationToken) =>
        {
            var command = new ToggleMenuItemAvailabilityCommand()
            {
                MenuItemId = id, IsAvailable = request.IsAvailable
            };
            var result = await mediator.Send(command, cancellationToken);
            return result.Match(value => Results.Ok(value), errors => errors.ToProblem());
        });
        
        return app;
    }
}