using System.ComponentModel.DataAnnotations;

namespace Brew.Shared.Settings;

public record RefreshTokenSettings
{
    public const string SectionName = "RefreshTokenSettings";
    
    [Required(AllowEmptyStrings = false)]
    public string Key { get; init; }  = string.Empty;
    
    public int ValidFor { get; init; }
}