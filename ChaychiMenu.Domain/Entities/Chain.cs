namespace ChaychiMenu.Domain.Entities;

public class Chain
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public Owner Owner { get; set; } = null!;
    public required string Name { get; set; } 
    public List<Restaurant> Restaurants { get; set; } = [];
}