using MediatR;

using RMS.Application.Commands.Category;
using RMS.WebApi.Extensions;

namespace RMS.WebApi.Endpoints;

public static class CategoryEndpoints
{
    public static WebApplication MapCategoryEndpoints(this WebApplication app)
    {
        app.MapGroup("api/category");

        app.MapPost("/", async (
            ISender mediatr,
            CreateCategoryCommand command,
            CancellationToken cancellationToken
            ) =>
        {
            var result = await mediatr.Send(command, cancellationToken);
            return result.Match(value => Results.Ok(value), errors => errors.ToProblem());
        });
        
        return app;
    }
}