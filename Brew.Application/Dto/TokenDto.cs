namespace Brew.Application.Dto;

public record TokenDto
{
    public required string Token { get; set; }
    public DateTimeOffset Expires { get; set; }
};