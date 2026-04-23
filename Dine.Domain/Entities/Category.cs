namespace Dine.Domain.Entities;

public class Category
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public List<MenuItem> MenuItems { get; set; } = [];
    public Guid RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; } = null!;
}