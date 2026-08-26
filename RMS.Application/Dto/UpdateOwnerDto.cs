using Microsoft.AspNetCore.JsonPatch.SystemTextJson;

namespace RMS.Application.Dto;

public record UpdateOwnerDto
{
    public Guid Id { get; init; }
    public required JsonPatchDocument<PatchOwnerDto> Document { get; init; }
};