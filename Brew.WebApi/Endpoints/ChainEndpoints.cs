using System.Security.Claims;

using Brew.Application.Commands;
using Brew.Application.Dto;
using Brew.Domain;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace Brew.WebApi.Endpoints;

public static class ChainEndpoints
{
    public static WebApplication MapChainEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/chains");

        group.MapPost("/", async (ClaimsPrincipal user, [FromBody] CreateChainDto dto, ISender mediator, CancellationToken cancellationToken) =>
        {
            var id = user.GetUserId();
            if (id is null) 
                return Results.Unauthorized();

            var command = new CreateChainCommand() { Name = dto.Name, UserId = id.Value };
            var result = await mediator.Send(command, cancellationToken);
            return result.Match(value => Results.Ok(value), errors => Results.BadRequest(errors));
        }).RequireAuthorization(options => options.RequireRole(UserRole.Owner));
        
        return app;
    }
}