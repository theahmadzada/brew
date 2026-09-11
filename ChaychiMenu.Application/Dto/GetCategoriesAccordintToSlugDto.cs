namespace ChaychiMenu.Application.Dto;

public record GetCategoriesAccordintToSlugDto()
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public int Order { get; set; }
    public List<MenuItemMinimalDto> MenuItems { get; init; } = [];
    public Guid RestaurantId { get; set; }
};