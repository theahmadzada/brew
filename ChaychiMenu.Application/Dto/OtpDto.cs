namespace ChaychiMenu.Application.Dto;

public record OtpDto()
{
    public required string Otp { get; init; }
    public Guid AppUserId { get; init; }
    public int ExpiresInMinutes { get; init; }
};
