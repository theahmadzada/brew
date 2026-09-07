namespace ChaychiMenu.Application.Dto;

public class CreateCategoryDto
{
    public Guid AppUserId { get; init; }
    public Guid RestaurantId { get; init; }
    public required string Name { get; init; }
    public int? Order { get; init; }
}