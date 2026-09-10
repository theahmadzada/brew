using ErrorOr;

using FluentValidation;

using MediatR;

namespace ChaychiMenu.Application.Commands.MenuItem;

public record ToggleMenuItemAvailabilityCommand() : IRequest<ErrorOr<bool>>
{
    public Guid AppUserId { get; init; }
    public Guid MenuItemId { get; init; }
    public bool IsAvailable { get; init; }
    public required string UserRole { get; init; }
};

public class ToggleMenuItemAvailabilityCommandValidator : AbstractValidator<ToggleMenuItemAvailabilityCommand>
{
    public ToggleMenuItemAvailabilityCommandValidator()
    {
        RuleFor(m => m.MenuItemId)
            .NotEmpty()
            .WithMessage("MenuItemId cannot be empty");
        RuleFor(m => m.IsAvailable)
            .NotEmpty()
            .WithMessage("IsAvailable cannot be empty");
        RuleFor(m => m.AppUserId)
            .NotEmpty()
            .WithMessage("AppUserId cannot be empty");
        RuleFor(m => m.UserRole)
            .NotEmpty()
            .WithMessage("UserRole cannot be empty");
    }
}