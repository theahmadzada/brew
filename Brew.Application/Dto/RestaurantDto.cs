namespace Brew.Application.Dto;

public record RestaurantDto
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
}