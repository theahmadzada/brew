using ErrorOr;

using MediatR;

using RMS.Application.Dto;

namespace RMS.Application.Queries.Chain;

public record GetChainByIdQuery : IRequest<ErrorOr<GetChainByIdDto>>
{
    public Guid Id { get; init; }
    public Guid OwnerId { get; init; }
}