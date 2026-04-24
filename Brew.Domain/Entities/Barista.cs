namespace Brew.Domain.Entities;

public class Barista
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public long? TelegramId { get; set; }
    public bool IsActive { get; set; }
    public Guid RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; } = null!;
    public List<Order> Orders = [];
}