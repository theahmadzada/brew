namespace Brew.Domain.Entities;

public class MenuItem
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }
    public bool IsDeleted { get; set; }
    public int Order { get; set; }
}