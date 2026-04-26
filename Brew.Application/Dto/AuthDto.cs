namespace Brew.Application.Dto;

public record AuthDto
{
    public Guid Id { get; init; }
    public required string Email { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string AccessToken { get; init; }
    public DateTimeOffset AccessTokenExpiration { get; init; }
    public required string RefreshToken { get; init; }
    public DateTimeOffset RefreshTokenExpiration { get; init; }
}