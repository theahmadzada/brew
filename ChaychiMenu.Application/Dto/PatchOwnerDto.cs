namespace ChaychiMenu.Application.Dto;

public record PatchOwnerDto
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? PhoneNumber { get; init; }
}