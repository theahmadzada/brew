namespace Brew.Domain.Entities;

public class Table
{
    public Guid Id { get; set; }
    public required string Number { get; set; }
    public required string SecretKey { get; set; }
    public Guid RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; } = null!;
}