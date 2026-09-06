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
            ISender mediator,
            CancellationToken cancellationToken) =>
        {
            var request = new CreateMenuItemCommand
            {
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
        
        return app;
    }
}