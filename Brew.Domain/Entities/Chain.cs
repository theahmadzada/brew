namespace Brew.Domain.Entities;

public class Chain
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public AppUser User { get; set; } = null!;
    public required string Name { get; set; } 
    public List<Restaurant> Restaurants { get; set; } = [];
}