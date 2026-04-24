using Brew.Application.Commands;
using Brew.Application.Dto;

using MediatR;

using Microsoft.AspNetCore.JsonPatch;

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
            var command = new PatchVendorCommand{Id = id, Document = document};
            var result = await mediator.Send(command, cancellationToken);
            return result.Match(value => Results.Ok(value), errors => Results.BadRequest(errors));
        });
        
        return app;
    }
}