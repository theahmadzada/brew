using Brew.Application.Dto;
using Brew.Application.Queries.Chain;
using Brew.Infrastructure.DbContext;

using ErrorOr;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Brew.Application.Handlers.Chain;

public class GetAllOwnerChainsQueryHandler(AppDbContext dbContext) : IRequestHandler<GetAllOwnerChainsQuery, ErrorOr<List<ChainDto>>>
{
    public async Task<ErrorOr<List<ChainDto>>> Handle(GetAllOwnerChainsQuery request, CancellationToken cancellationToken)
    {
        var chains = await dbContext.Chains.Where(x => x.Owner.Id == request.OwnerId)
            .Select(x => new ChainDto() { Id = x.Id, Name = x.Name })
            .ToListAsync(cancellationToken);

        return chains;
    }
}