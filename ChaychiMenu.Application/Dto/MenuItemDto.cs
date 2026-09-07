namespace ChaychiMenu.Application.Dto;

public record MenuItemDto()
{
    public Guid Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public Guid CategoryId { get; init; }
    public string? ImageUrl { get; init; }
    public decimal Price { get; init; }
    public int Order { get; init; }
};