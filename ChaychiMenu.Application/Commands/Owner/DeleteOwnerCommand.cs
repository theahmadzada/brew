using ErrorOr;

using FluentValidation;

using MediatR;

namespace ChaychiMenu.Application.Commands.Owner;

public record DeleteOwnerCommand : IRequest<ErrorOr<Guid>>
{
    public Guid AppUserId { get; init; }
}

public class DeleteOwnerCommandValidator : AbstractValidator<DeleteOwnerCommand>
{
    public DeleteOwnerCommandValidator()
    {
        RuleFor(x => x.AppUserId)
            .NotEmpty().WithMessage("The id cannot be empty");
    }
}