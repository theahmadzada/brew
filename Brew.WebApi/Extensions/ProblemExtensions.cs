using ErrorOr;

namespace Brew.WebApi.Extensions;

public static class ProblemExtensions
{
    public static IResult ToProblem(this List<Error> errors)
    {
        if (errors.All(x => x.Type == ErrorType.Validation))
            return Results.UnprocessableEntity(errors);

        return errors.First().Type switch
        {
            ErrorType.Unauthorized => Results.Unauthorized(),
            ErrorType.NotFound => Results.NotFound(errors),
            ErrorType.Conflict => Results.Conflict(errors),
            ErrorType.Forbidden => Results.Forbid(),
            ErrorType.Failure => Results.Problem(),
            _ => Results.BadRequest(errors)
        };
    }
}