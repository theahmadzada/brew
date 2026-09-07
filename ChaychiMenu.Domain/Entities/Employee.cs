namespace ChaychiMenu.Domain.Entities;

public class Employee
{
    public Guid Id { get; set; }
    public Guid AppUserId { get; set; }
    public AppUser AppUser { get; set; } = null!;
    public Guid RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; } = null!;
    public List<Order> Orders { get; set; } = [];
}