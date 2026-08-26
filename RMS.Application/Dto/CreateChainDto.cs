namespace RMS.Application.Dto;

public record CreateChainDto
{
    public required string Name { get; init; }
};