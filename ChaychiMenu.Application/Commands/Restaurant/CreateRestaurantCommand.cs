using ChaychiMenu.Application.Dto;

using ErrorOr;

using FluentValidation;

using MediatR;

namespace ChaychiMenu.Application.Commands.Restaurant;

public record CreateRestaurantCommand() : IRequest<ErrorOr<RestaurantDto>>
{
    public required string Name { get; set; }
    public Guid AppUserId { get; set; }
    public Guid? ChainId { get; set; }
}

public class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
{
    public CreateRestaurantCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(50)
            .WithMessage("Name cannot exceed 50 characters");
    }
}