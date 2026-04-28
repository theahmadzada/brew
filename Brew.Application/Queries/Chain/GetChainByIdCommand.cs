using Brew.Application.Dto;

using ErrorOr;

using MediatR;

namespace Brew.Application.Queries.Chain;

public class GetChainByIdCommand : IRequest<ErrorOr<GetChainByIdDto>>
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
}