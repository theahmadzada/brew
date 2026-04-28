using Brew.Application.Dto;

using ErrorOr;

using MediatR;

namespace Brew.Application.Queries.Chain;

public record GetAllOwnerChainsQuery : IRequest<ErrorOr<List<ChainDto>>>
{
    public Guid OwnerId { get; init; }
}