using ErrorOr;

using FluentValidation;

using MediatR;

namespace Brew.Application.Commands;

public record ChangeVendorPasswordCommand : IRequest<ErrorOr<Guid>>
{
    public Guid Id { get; set; }
    public required string OldPassword { get; init; }
    public required string NewPassword { get; init; }
}

public class ChangeVendorPasswordCommandValidator : AbstractValidator<ChangeVendorPasswordCommand>
{
    public ChangeVendorPasswordCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage("Id cannot be null");
        RuleFor(x => x.OldPassword)
            .NotNull().WithMessage("Old password cannot be null");
        RuleFor(x => x.NewPassword)
            .NotNull().WithMessage("New password cannot be null");
    }
}