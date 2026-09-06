using ErrorOr;

using FluentValidation;

using MediatR;

using RMS.Application.Dto;

namespace RMS.Application.Commands.Category;

public record CreateCategoryCommand() : IRequest<ErrorOr<CategoryDto>>
{
    public Guid AppUserId { get; init; }
    public Guid RestaurantId { get; init; }
    public required string Name { get; init; }
    public int? Order { get; init; }
}

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Category name is required")
            .MaximumLength(50)
            .WithMessage("Category name cannot exceed 50 characters");
        RuleFor(x => x.RestaurantId)
            .NotEmpty()
            .WithMessage("Category identifier is required");
    }
}