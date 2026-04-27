namespace Brew.Application.Dto;

public record ChainDto
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
};