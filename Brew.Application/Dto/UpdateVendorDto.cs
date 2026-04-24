using ErrorOr;

using MediatR;

namespace Brew.Application.Dto;

public record UpdateVendorDto
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? PhoneNumber { get; init; }
}