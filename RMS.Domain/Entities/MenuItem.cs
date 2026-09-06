namespace RMS.Domain.Entities;

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
    public bool IsAvailable { get; set; } = true;
    public int Order { get; set; }
}