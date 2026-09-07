using Microsoft.AspNetCore.JsonPatch.SystemTextJson;

namespace ChaychiMenu.Application.Dto;

public record UpdateOwnerDto
{ 
    public required JsonPatchDocument<PatchOwnerDto> Document { get; init; }
};