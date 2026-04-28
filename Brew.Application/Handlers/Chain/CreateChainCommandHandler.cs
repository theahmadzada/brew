using Brew.Application.Commands.Chain;
using Brew.Application.Dto;
using Brew.Infrastructure.DbContext;

using ErrorOr;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Brew.Application.Handlers.Chain;

public class CreateChainCommandHandler(AppDbContext dbContext) : IRequestHandler<CreateChainCommand, ErrorOr<ChainDto>>
{
    public async Task<ErrorOr<ChainDto>> Handle(CreateChainCommand request, CancellationToken cancellationToken)
    {
        var ownerId = await dbContext.Owners
            .Where(x => x.AppUserId == request.UserId)
            .Select(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if(ownerId == Guid.Empty)
            return Error.NotFound("Owner.NotFound", "Owner not found");

        var chain = new Domain.Entities.Chain() { OwnerId = ownerId, Name = request.Name };
        dbContext.Chains.Add(chain);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new ChainDto() { Id = chain.Id, Name = chain.Name, };
    }
}