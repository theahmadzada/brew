using ErrorOr;
using FluentValidation;
using MediatR;

namespace RMS.Application.Common;

public class ValidationBehaviour<TRequest, TResponse>(IValidator<TRequest>? validator)
    : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (validator is null) return await next(cancellationToken);
        var result = await validator.ValidateAsync(request, cancellationToken);

        if (result.IsValid) return await next(cancellationToken);
        var errors = result.Errors
            .ConvertAll(x => Error.Validation(x.ErrorCode, x.ErrorMessage));
        
        return (dynamic)errors;
    }
}