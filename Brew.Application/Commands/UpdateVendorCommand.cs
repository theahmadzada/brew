using Brew.Application.Dto;

using ErrorOr;

using FluentValidation;

using MediatR;

using Microsoft.AspNetCore.JsonPatch;

namespace Brew.Application.Commands;

public record UpdateVendorCommand : IRequest<ErrorOr<UpdatedVendorDto>>
{
    public Guid Id { get; init; }
    public required JsonPatchDocument<UpdateVendorDto> Document { get; init; }
}

public class UpdateVendorCommandValidator : AbstractValidator<UpdateVendorCommand>
{
    public UpdateVendorCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage("Id cannot be null");
        RuleFor(x => x.Document)
            .NotNull().WithMessage("Document cannot be null");
        RuleFor(x => x.Document.Operations)
            .NotNull().WithMessage("Operations cannot be null");
    }
}
