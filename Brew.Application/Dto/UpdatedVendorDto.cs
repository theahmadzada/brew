namespace Brew.Application.Dto;

public class UpdatedVendorDto
{
    public Guid Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string PhoneNumber { get; init; }
}