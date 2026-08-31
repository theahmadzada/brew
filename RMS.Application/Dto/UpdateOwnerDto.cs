using Microsoft.AspNetCore.JsonPatch.SystemTextJson;

namespace RMS.Application.Dto;

public record UpdateOwnerDto
{ 
    public required JsonPatchDocument<PatchOwnerDto> Document { get; init; }
};