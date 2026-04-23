using Brew.Domain.Enums;

namespace Brew.Domain.Entities;

public class Call
{
    public Guid Id { get; set; }
    public Guid? WaiterId { get; set; }
    public Waiter? Waiter { get; set; }
    public Guid TableId { get; set; }
    public Table Table { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? AcceptedAt { get; set; }
    public DateTimeOffset? CompletedAt { get; set; }
    public CallStatus Status { get; set; }
}