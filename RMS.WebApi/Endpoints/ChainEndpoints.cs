using System.Security.Claims;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using RMS.Application.Commands.Chain;
using RMS.Application.Dto;
using RMS.Application.Queries.Chain;
using RMS.Domain;
using RMS.WebApi.Extensions;

namespace RMS.WebApi.Endpoints;

public static class ChainEndpoints
{
    public static WebApplication MapChainEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/chains");

        group.MapPost("/", async (
            [FromBody] CreateChainDto dto,
            ClaimsPrincipal user,
            ISender mediator,
            CancellationToken cancellationToken) =>
        {
            var id = user.GetUserId();
            if (id is null)
                return Results.Unauthorized();

            var command = new CreateChainCommand() { Name = dto.Name, UserId = id.Value };
            var result = await mediator.Send(command, cancellationToken);
            return result.Match(value => Results.Ok(value), errors => errors.ToProblem());
        }).RequireAuthorization(options => options.RequireRole(UserRole.Owner));

        group.MapGet("/{id}", async (
            Guid id,
            ClaimsPrincipal user,
            ISender mediator,
            CancellationToken cancellationToken) =>
        {
            var ownerId = user.GetUserId();
            if (ownerId is null)
                return Results.Unauthorized();

            var query = new GetChainByIdQuery() { Id = id, OwnerId = ownerId.Value };
            var result = await mediator.Send(query, cancellationToken);
            return result.Match(value => Results.Ok(value), errors => errors.ToProblem());
        }).RequireAuthorization(options => options.RequireRole(UserRole.Owner));

        group.MapGet("/", async (
            ClaimsPrincipal user,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var ownerId = user.GetUserId();
            if (ownerId is null)
                return Results.Unauthorized();

            var query = new GetAllOwnerChainsQuery() { OwnerId = ownerId.Value };
            var result = await mediator.Send(query, cancellationToken);
            return result.Match(value => Results.Ok(value), errors => errors.ToProblem());
        }).RequireAuthorization(options => options.RequireRole(UserRole.Owner));

        return app;
    }
}