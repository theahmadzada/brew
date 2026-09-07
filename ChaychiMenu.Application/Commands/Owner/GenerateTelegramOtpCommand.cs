using ChaychiMenu.Application.Dto;

using ErrorOr;

using FluentValidation;

using MediatR;

namespace ChaychiMenu.Application.Commands.Owner;

public record GenerateTelegramOtpCommand() : IRequest<ErrorOr<OtpDto>>
{
    public Guid AppUserId { get; init; } 
}

public class GenerateTelegramOtpCommandValidator : AbstractValidator<GenerateTelegramOtpCommand>
{
    public GenerateTelegramOtpCommandValidator()
    {
        RuleFor(x => x.AppUserId)
            .NotEmpty()
            .WithMessage("AppUserId is required");
    }
}