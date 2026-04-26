namespace Brew.Application.Dto;

public record UpdateOwnerDto
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? PhoneNumber { get; init; }
}