using ChaychiMenu.Application.Dto;

using ErrorOr;

using FluentValidation;

using MediatR;

namespace ChaychiMenu.Application.Commands.MenuItem;

public record CreateMenuItemCommand() : IRequest<ErrorOr<MenuItemDto>>
{
    public Guid AppUserId { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public Guid CategoryId { get; init; }
    public Stream? Image { get; init; }
    public string? ContentType { get; init; }
    public decimal Price { get; init; }
    public int? Order { get; init; }
    public required string UserRole { get; init; }
}

public class CreateMenuItemCommandValidator : AbstractValidator<CreateMenuItemCommand>
{
    public CreateMenuItemCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required")
            .MaximumLength(50)
            .WithMessage("Title cannot exceed 50 characters");
        RuleFor(x => x.Description)
            .MaximumLength(250)
            .WithMessage("Description cannot exceed 250 characters");
        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0")
            .NotEmpty()
            .WithMessage("Price is required");
    }
}