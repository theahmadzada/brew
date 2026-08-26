using System.ComponentModel.DataAnnotations;

namespace RMS.Shared.Settings;

public record JwtSettings
{
    public const string SectionName = "JwtSettings";
    
    [Required(AllowEmptyStrings = false)]
    public string Issuer { get; init; } = string.Empty;
    
    [Required(AllowEmptyStrings = false)]
    public string Audience { get; init; } = string.Empty;
    
    [Required(AllowEmptyStrings = false)]
    public string SigningKey { get; init; } = string.Empty;
    
    public int ValidFor { get; init; }
}