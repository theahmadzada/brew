using ErrorOr;

using FluentValidation;

using MediatR;

using RMS.Application.Dto;

namespace RMS.Application.Commands.Owner;

public record LogInOwnerCommand : IRequest<ErrorOr<AuthDto>>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}

public class LogInOwnerCommandValidator : AbstractValidator<LogInOwnerCommand>
{
    public LogInOwnerCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email address");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long");
    }
}