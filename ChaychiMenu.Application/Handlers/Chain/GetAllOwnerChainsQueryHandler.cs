using ChaychiMenu.Application.Dto;
using ChaychiMenu.Application.Queries.Chain;
using ChaychiMenu.Infrastructure.DbContext;

using ErrorOr;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace ChaychiMenu.Application.Handlers.Chain;

public class GetAllOwnerChainsQueryHandler(AppDbContext dbContext) : IRequestHandler<GetAllOwnerChainsQuery, ErrorOr<List<ChainDto>>>
{
    public async Task<ErrorOr<List<ChainDto>>> Handle(GetAllOwnerChainsQuery request, CancellationToken cancellationToken)
    {
        var ownerExists = await dbContext.Owners.AnyAsync(x => x.AppUserId == request.AppUserId, cancellationToken);
        if (!ownerExists) return Error.NotFound("Owner.NotFound", "Owner not found");
        
        var chains = await dbContext.Chains
            .Where(x => x.Owner.AppUserId == request.AppUserId)
            .Select(x => new ChainDto() { Id = x.Id, Name = x.Name })
            .ToListAsync(cancellationToken);

        return chains;
    }
}