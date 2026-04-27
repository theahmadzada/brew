namespace Brew.Application.Dto;

public record TokenDto
{
    public required string Token { get; init; }
    public DateTimeOffset Expires { get; init; }
};