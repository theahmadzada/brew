namespace Brew.Domain.Entities;

public class Employee
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public AppUser User { get; set; } = null!;
    public Guid RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; } = null!;
    public List<Order> Orders = [];
}