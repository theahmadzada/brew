using Brew.Application.Dto;

using ErrorOr;

using MediatR;

namespace Brew.Application.Queries.Chain;

public record GetChainByIdQuery : IRequest<ErrorOr<GetChainByIdDto>>
{
    public Guid Id { get; init; }
    public Guid OwnerId { get; init; }
}