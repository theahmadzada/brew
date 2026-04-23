using Dine.Domain.Enums;

namespace Dine.Domain.Entities;

public class Call
{
    public Guid Id { get; set; }
    public Guid? WaiterId { get; set; }
    public Waiter? Waiter { get; set; } = null!;
    public Guid TableId { get; set; }
    public Table Table { get; set; } = null!;
    public Guid RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? AcceptedAt { get; set; }
    public DateTimeOffset? CompletedAt { get; set; }
    public CallStatus Status { get; set; }
}