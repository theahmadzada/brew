using ChaychiMenu.Domain.Enums;

namespace ChaychiMenu.Domain.Entities;

public class Order
{
    public Guid Id { get; set; }
    public Guid TableId { get; set; }
    public Table Table { get; set; } = null!;
    public long? TelegramUserId { get; set; }
    public OrderStatus Status { get; set; }
    public List<OrderItem> OrderItems { get; set; } = [];
    public DateTimeOffset CreatedAt { get; set; }
}