using Brew.Application.Dto;

using ErrorOr;

using MediatR;

using Microsoft.AspNetCore.JsonPatch;

namespace Brew.Application.Commands;

public record PatchVendorCommand : IRequest<ErrorOr<PatchedVendorDto>>
{
    public Guid Id { get; init; }
    public required JsonPatchDocument<UpdateVendorDto> Document { get; init; }
}