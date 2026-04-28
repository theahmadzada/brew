using ErrorOr;

using FluentValidation;

using MediatR;

namespace Brew.Application.Commands.Owner;

public record DeleteOwnerCommand : IRequest<ErrorOr<Guid>>
{
    public Guid Id { get; set; }
}

public class DeleteOwnerCommandValidator : AbstractValidator<DeleteOwnerCommand>
{
    public DeleteOwnerCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("The id cannot be empty");
    }
}