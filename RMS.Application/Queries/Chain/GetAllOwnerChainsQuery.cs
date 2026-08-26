using ErrorOr;

using MediatR;

using RMS.Application.Dto;

namespace RMS.Application.Queries.Chain;

public record GetAllOwnerChainsQuery : IRequest<ErrorOr<List<ChainDto>>>
{
    public Guid OwnerId { get; init; }
}