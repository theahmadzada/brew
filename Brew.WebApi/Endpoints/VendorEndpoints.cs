using System.Security.Claims;

using Brew.Application.Commands;
using Brew.Application.Dto;

using MediatR;

using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Brew.WebApi.Endpoints;

public static class VendorEndpoints
{
    public static WebApplication MapVendorEndpoints(this WebApplication app)
    {
        app.MapPost("/vendors", async (CreateVendorCommand command, ISender mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(command, cancellationToken);
            return result.Match(value => Results.Ok(value), errors => Results.BadRequest(errors));
        });

        app.MapPatch("/vendors/{id}", async (
            Guid id, 
            JsonPatchDocument<UpdateVendorDto> document, 
            ISender mediator, 
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateVendorCommand{Id = id, Document = document};
            var result = await mediator.Send(command, cancellationToken);
            return result.Match(value => Results.Ok(value), errors => Results.BadRequest(errors));
        });
        
        app.MapPost("/vendor/password", async (
            ClaimsPrincipal user,
            [FromBody] ChangePasswordDto dto,
            ISender mediator,
            CancellationToken cancellationToken) =>
        {
            var id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(!Guid.TryParse(id, out var parsedId))
                return Results.Problem(detail: "Invalid id", statusCode: 401);
            
            var command = new ChangeVendorPasswordCommand()
            {
                Id = parsedId, OldPassword = dto.OldPassword, NewPassword = dto.NewPassword
            };
            var result = await mediator.Send(command, cancellationToken);
            return result.Match(value => Results.Ok(value), errors => Results.BadRequest(errors));
        }).RequireAuthorization();
        
        return app;
    }
}