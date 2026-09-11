namespace ChaychiMenu.Application.Dto;

public record MenuItemMinimalDto()
{ 
    public Guid Id { get; init; }
    public required string Title { get; init; }
    public decimal Price { get; init; }
    public int Order { get; init; }
};