using System.Security.Claims;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using ChaychiMenu.Application.Commands.Owner;
using ChaychiMenu.Application.Dto;
using ChaychiMenu.Domain;
using ChaychiMenu.WebApi.Extensions;

namespace ChaychiMenu.WebApi.Endpoints;

public static class OwnerEndpoints
{
    public static WebApplication MapOwnerEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("api/owners");

        group.MapPost("/", async (
            CreateOwnerCommand command,
            ISender mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(command, cancellationToken);
            return result.Match(value => Results.Ok(value), errors => errors.ToProblem());
        });
        
        group.MapPost("/otp", async (
            GenerateTelegramOtpCommand request,
            ISender mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request, cancellationToken);
            return result.Match(value => Results.Ok(value), errors => errors.ToProblem());
        });
        
        group.MapPost("/otp/validate", async (
            ValidateOtpCommand request,
            ISender mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request, cancellationToken);
            return result.Match(value => Results.Ok(value), errors => errors.ToProblem());
        });
        
        group.MapPatch("/{id}", async (
            Guid id,
            UpdateOwnerDto dto,
            ISender mediator,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateOwnerCommand { AppUserId = id, Document = dto.Document };
            var result = await mediator.Send(command, cancellationToken);
            return result.Match(value => Results.Ok(value), errors => errors.ToProblem());
        }).RequireAuthorization(policy => policy.RequireRole(UserRole.Owner));

        group.MapPost("/password", async (
            [FromBody] ChangePasswordDto dto,
            ClaimsPrincipal user,
            ISender mediator,
            CancellationToken cancellationToken) =>
        {
            var id = user.GetAppUserId();
            if (id is null)
                return Results.Unauthorized();

            var command = new ChangeOwnerPasswordCommand()
            {
                AppUserId = id.Value, OldPassword = dto.OldPassword, NewPassword = dto.NewPassword
            };
            var result = await mediator.Send(command, cancellationToken);
            return result.Match(value => Results.Ok(value), errors => errors.ToProblem());
        }).RequireAuthorization(policy => policy.RequireRole(UserRole.Owner));

        group.MapDelete("/{id}", async (
            Guid id,
            ISender mediator,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteOwnerCommand { AppUserId = id };
            var result = await mediator.Send(command, cancellationToken);
            return result.Match(value => Results.Ok(value), errors => errors.ToProblem());
        }).RequireAuthorization(policy =>
        {
            policy.RequireRole(UserRole.Owner);
            policy.RequireRole(UserRole.Admin);
        });

        return app;
    }
}
