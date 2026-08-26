namespace RMS.Application.Dto;

public record ChangePasswordDto
{
    public required string OldPassword { get; init; }
    public required string NewPassword { get; init; }
}