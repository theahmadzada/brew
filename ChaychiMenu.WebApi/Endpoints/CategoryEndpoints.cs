using System.Security.Claims;

using MediatR;

using ChaychiMenu.Application.Commands.Category;
using ChaychiMenu.Application.Dto;
using ChaychiMenu.Application.Queries.Category;
using ChaychiMenu.WebApi.Extensions;

namespace ChaychiMenu.WebApi.Endpoints;

public static class CategoryEndpoints
{
    public static WebApplication MapCategoryEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("api/category");

        group.MapPost("/", async (
            CreateCategoryDto request,
            ClaimsPrincipal user,
            ISender mediatr,
            CancellationToken cancellationToken) =>
        {
            var id = user.GetAppUserId();
            if (id is null)
                return Results.Unauthorized();

            var command = new CreateCategoryCommand()
            {
                AppUserId = id.Value,
                Name = request.Name,
                RestaurantId = request.RestaurantId,
                Order = request.Order
            };
            var result = await mediatr.Send(command, cancellationToken);
            return result.Match(value => Results.Ok(value), errors => errors.ToProblem());
        });

        group.MapGet("/{slug}", async (
            string slug,
            ISender mediatr,
            CancellationToken cancellationToken) =>
        {
            var query = new GetCategoriesAccordingToSlugQuery() { Slug = slug };
            var result = await mediatr.Send(query, cancellationToken);
            return result.Match(value => Results.Ok(value), errors => errors.ToProblem());
        });
        
        return app;
    }
}