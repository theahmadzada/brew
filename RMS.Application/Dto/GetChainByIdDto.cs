namespace RMS.Application.Dto;

public record GetChainByIdDto()
{
    public required ChainDto Chain { get; init; }
    public ICollection<RestaurantDto> RestaurantDtos { get; init; } = [];
};