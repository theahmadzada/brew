using System.Security.Claims;

using Brew.Application.Commands;
using Brew.Application.Dto;
using Brew.Domain;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace Brew.WebApi.Endpoints;

public static class OwnerEndpoints
{
    public static WebApplication MapOwnerEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("api/owners");
        
        group.MapPost("/", async (CreateOwnerCommand command, ISender mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(command, cancellationToken);
            return result.Match(value => Results.Ok(value), errors => Results.BadRequest(errors));
        });

        group.MapPost("/login", async (LogInOwnerCommand command, ISender mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(command, cancellationToken);
            return result.Match(value => Results.Ok(value), errors => Results.BadRequest(errors));
        });
        
        group.MapPatch("/{id}", async (
            UpdateOwnerDto dto, 
            ISender mediator, 
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateOwnerCommand { Id = dto.Id, Document = dto.Document };
            var result = await mediator.Send(command, cancellationToken);
            return result.Match(value => Results.Ok(value), errors => Results.BadRequest(errors));
        });
        
        group.MapPost("/password", async (
            ClaimsPrincipal user,
            [FromBody] ChangePasswordDto dto,
            ISender mediator,
            CancellationToken cancellationToken) =>
        {
            var id = user.GetUserId();
            if(id is null)
                return Results.Unauthorized();
            
            var command = new ChangeOwnerPasswordCommand()
            {
                Id = id.Value, OldPassword = dto.OldPassword, NewPassword = dto.NewPassword
            };
            var result = await mediator.Send(command, cancellationToken);
            return result.Match(value => Results.Ok(value), errors => Results.BadRequest(errors));
        }).RequireAuthorization(policy => policy.RequireRole(UserRole.Owner));

        group.MapDelete("/{id}", async (Guid id, ISender mediator, CancellationToken cancellationToken) =>
        {
            var command = new DeleteOwnerCommand { Id = id };
            var result = await mediator.Send(command, cancellationToken);
            return result.Match(value => Results.Ok(value), errors => Results.BadRequest(errors));
        }).RequireAuthorization(policy => policy.RequireRole(UserRole.Admin));
        
        return app;
    }
}

public static class UserClaimsExtension
{
    public static Guid? GetUserId(this ClaimsPrincipal principal)
    {
        var id = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Guid.TryParse(id, out var parsedId);
        return parsedId;
    }
}