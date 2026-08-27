using ErrorOr;

using FluentValidation;

using MediatR;

using RMS.Application.Dto;

namespace RMS.Application.Commands.Chain;

public record CreateChainCommand : IRequest<ErrorOr<ChainDto>>
{
    public Guid AppUserId { get; init; }
    public required string Name { get; init; }
}

public class CreateChainCommandValidator : AbstractValidator<CreateChainCommand>
{
    public CreateChainCommandValidator()
    {
        RuleFor(x => x.AppUserId)
            .NotEmpty().WithMessage("Id is required");
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(50).WithMessage("Name cannot exceed 50 characters");;
    }
}