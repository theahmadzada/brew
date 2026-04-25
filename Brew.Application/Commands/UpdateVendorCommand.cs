using Brew.Application.Dto;

using ErrorOr;

using MediatR;

using Microsoft.AspNetCore.JsonPatch;

namespace Brew.Application.Commands;

public record UpdateVendorCommand : IRequest<ErrorOr<UpdatedVendorDto>>
{
    public Guid Id { get; init; }
    public required JsonPatchDocument<UpdateVendorDto> Document { get; init; }
}