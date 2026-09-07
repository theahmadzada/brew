namespace ChaychiMenu.Application.Dto;

public record ToggleMenuItemAvailabilityDto()
{
    public bool IsAvailable { get; init; }
}