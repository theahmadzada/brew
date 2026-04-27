using Brew.Application.Dto;

using ErrorOr;

using FluentValidation;

using MediatR;

using Microsoft.AspNetCore.JsonPatch.SystemTextJson;

namespace Brew.Application.Commands;

public record UpdateOwnerCommand : IRequest<ErrorOr<UpdatedOwnerDto>>
{
    public Guid Id { get; init; }
    public required JsonPatchDocument<PatchOwnerDto> Document { get; init; }
}

public class UpdateOwnerCommandValidator : AbstractValidator<UpdateOwnerCommand>
{
    public UpdateOwnerCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage("Id cannot be null");
        RuleFor(x => x.Document)
            .NotNull().WithMessage("Document cannot be null");
        RuleFor(x => x.Document.Operations)
            .NotNull().WithMessage("Operations cannot be null");
    }
}
