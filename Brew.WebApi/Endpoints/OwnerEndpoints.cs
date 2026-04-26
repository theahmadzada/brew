using System.Security.Claims;

using Brew.Application.Commands;
using Brew.Application.Dto;
using Brew.Domain;

using MediatR;

using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Brew.WebApi.Endpoints;

public static class OwnerEndpoints
{
    public static WebApplication MapOwnerEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/owners");
        
        group.MapPost("", async (CreateOwnerCommand command, ISender mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(command, cancellationToken);
            return result.Match(value => Results.Ok(value), errors => Results.BadRequest(errors));
        });

        app.MapPatch("/{id}", async (
            Guid id, 
            JsonPatchDocument<UpdateOwnerDto> document, 
            ISender mediator, 
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateOwnerCommand{Id = id, Document = document};
            var result = await mediator.Send(command, cancellationToken);
            return result.Match(value => Results.Ok(value), errors => Results.BadRequest(errors));
        });
        
        app.MapPost("/password", async (
            ClaimsPrincipal user,
            [FromBody] ChangePasswordDto dto,
            ISender mediator,
            CancellationToken cancellationToken) =>
        {
            var id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(!Guid.TryParse(id, out var parsedId))
                return Results.Problem(detail: "Invalid id", statusCode: 401);
            
            var command = new ChangeOwnerPasswordCommand()
            {
                Id = parsedId, OldPassword = dto.OldPassword, NewPassword = dto.NewPassword
            };
            var result = await mediator.Send(command, cancellationToken);
            return result.Match(value => Results.Ok(value), errors => Results.BadRequest(errors));
        }).RequireAuthorization(policy => policy.RequireRole(UserRole.Owner));

        app.MapDelete("/{id}", async (Guid id, ISender mediator, CancellationToken cancellationToken) =>
        {
            var command = new DeleteOwnerCommand { Id = id };
            var result = await mediator.Send(command, cancellationToken);
            return result.Match(value => Results.Ok(value), errors => Results.BadRequest(errors));
        }).RequireAuthorization(policy => policy.RequireRole(UserRole.Admin));
        
        return app;
    }
}