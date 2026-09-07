using MediatR;

using RMS.Application.Commands.AppUser;
using RMS.WebApi.Extensions;

namespace RMS.WebApi.Endpoints;

public static class AppUserEndpoint
{
    public static WebApplication MapAppUserEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("api/auth");
        
        group.MapPost("/login", async (
            LogInCommand command,
            ISender mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(command, cancellationToken);
            return result.Match(value => Results.Ok(value), errors => errors.ToProblem());
        });
        
        return app;
    }
}