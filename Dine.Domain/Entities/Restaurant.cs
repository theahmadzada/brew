namespace Dine.Domain.Entities;

public class Restaurant
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public List<Category> Categories { get; set; } = [];
    public List<Table> Tables { get; set; } = [];
    public List<Waiter> Waiters { get; set; } = [];
    public Guid UserId { get; set; }
    public AppUser User { get; set; } = null!;
    public long? TelegramChatId { get; set; }
}