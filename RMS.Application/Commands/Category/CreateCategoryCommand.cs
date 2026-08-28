using ErrorOr;

using FluentValidation;

using MediatR;

using RMS.Application.Dto;

namespace RMS.Application.Commands.Category;

public record CreateCategoryCommand() : IRequest<ErrorOr<CategoryDto>>
{
    public Guid RestaurantId { get; set; }
    public required string Name { get; set; }
    public int Order { get; set; }
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