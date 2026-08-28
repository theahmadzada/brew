using System.Security.Claims;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using RMS.Application.Commands.Restaurant;
using RMS.Application.Dto;
using RMS.WebApi.Extensions;

namespace RMS.WebApi.Endpoints;

public static class RestaurantEndpoints
{
    public static WebApplication MapRestaurantEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("api/restaurants");

        group.MapPost("/", async (
            [FromBody] CreateRestaurantDto request,
            ClaimsPrincipal user,
            ISender mediator,
            CancellationToken cancellationToken) =>
        {
            var id = user.GetAppUserId();
            if (id is null)
                return Results.Unauthorized();
            
            var command = new CreateRestaurantCommand() { Name = request.Name, AppUserId = id.Value };
            var result = await mediator.Send(command, cancellationToken);
            return result.Match(value => Results.Ok(value), errors => errors.ToProblem());
        });
        
        return app;
    }
}