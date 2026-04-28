using ErrorOr;

using FluentValidation;

using MediatR;

namespace Brew.Application.Commands.Owner;

public record ChangeOwnerPasswordCommand : IRequest<ErrorOr<Guid>>
{
    public Guid Id { get; set; }
    public required string OldPassword { get; init; }
    public required string NewPassword { get; init; }
}

public class ChangeOwnerPasswordCommandValidator : AbstractValidator<ChangeOwnerPasswordCommand>
{
    public ChangeOwnerPasswordCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage("Id cannot be null");
        RuleFor(x => x.OldPassword)
            .NotNull().WithMessage("Old password cannot be null");
        RuleFor(x => x.NewPassword)
            .NotNull().WithMessage("New password cannot be null")
            .MinimumLength(8).WithMessage("New password must be at least 8 characters long");
    }
}