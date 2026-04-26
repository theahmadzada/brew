namespace Brew.Domain.Entities;

public class Owner
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public AppUser User { get; set; } = null!;
    public List<Chain> Chains { get; set; } = [];
}