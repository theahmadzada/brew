using Brew.Application.Commands;
using MediatR;

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
        
        return app;
    }
}