using Microsoft.AspNetCore.Http;

namespace RMS.Application.Dto;

public record CreateMenuItemDto()
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public Guid CategoryId { get; init; }
    public IFormFile? Image { get; init; }
    public decimal Price { get; init; }
    public int Order { get; init; }
};