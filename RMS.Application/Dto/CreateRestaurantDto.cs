namespace RMS.Application.Dto;

public record CreateRestaurantDto
{
    public required string Name { get; set; }
}