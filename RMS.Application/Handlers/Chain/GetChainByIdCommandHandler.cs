using RMS.Infrastructure.DbContext;

using ErrorOr;

using MediatR;

using Microsoft.EntityFrameworkCore;

using RMS.Application.Dto;
using RMS.Application.Queries.Chain;

namespace RMS.Application.Handlers.Chain;

public class GetChainByIdCommandHandler(AppDbContext context) : IRequestHandler<GetChainByIdQuery, ErrorOr<GetChainByIdDto>>
{
    public async Task<ErrorOr<GetChainByIdDto>> Handle(GetChainByIdQuery request, CancellationToken cancellationToken)
    {
        var chain = await context.Chains
            .Where(x => x.Owner.Id == request.OwnerId && x.Id == request.Id)
            .Select(x => new GetChainByIdDto()
            {
                Chain = new ChainDto() { Id = x.Id, Name = x.Name },
                RestaurantDtos = x.Restaurants.Select(y => new RestaurantDto() { Id = y.Id, Name = y.Name })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);
        
        if (chain is null)
            return Error.NotFound("Chain.NotFound", "Chain not found");

        return chain;
    }
}