using ErrorOr;

using FluentValidation;

using MediatR;

using Microsoft.AspNetCore.JsonPatch.SystemTextJson;

using RMS.Application.Dto;

namespace RMS.Application.Commands.Owner;

public record UpdateOwnerCommand : IRequest<ErrorOr<UpdatedOwnerDto>>
{
    public Guid AppUserId { get; init; }
    public required JsonPatchDocument<PatchOwnerDto> Document { get; init; }
}

public class UpdateOwnerCommandValidator : AbstractValidator<UpdateOwnerCommand>
{
    public UpdateOwnerCommandValidator()
    {
        RuleFor(x => x.AppUserId)
            .NotNull().WithMessage("Id cannot be null");
        RuleFor(x => x.Document)
            .NotNull().WithMessage("Document cannot be null");
        RuleFor(x => x.Document.Operations)
            .NotNull().WithMessage("Operations cannot be null");
    }
}
