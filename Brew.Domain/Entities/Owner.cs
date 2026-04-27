namespace Brew.Domain.Entities;

public class Owner
{
    public Guid Id { get; set; }
    public Guid AppUserId { get; set; }
    public AppUser AppUser { get; set; } = null!;
    public List<Chain> Chains { get; set; } = [];
}