using ChaychiMenu.Application.Dto;

using ErrorOr;

using MediatR;

namespace ChaychiMenu.Application.Queries.Chain;

public record GetAllOwnerChainsQuery : IRequest<ErrorOr<List<ChainDto>>>
{
    public Guid AppUserId { get; init; }
}