using ErrorOr;

using MediatR;

using Microsoft.EntityFrameworkCore;

using RMS.Application.Commands.Restaurant;
using RMS.Application.Dto;
using RMS.Infrastructure.DbContext;

using Slugify;

namespace RMS.Application.Handlers.Restaurant;

public class CreateRestaurantCommandHandler(
    AppDbContext dbContext, 
    ISlugHelper slugHelper) : IRequestHandler<CreateRestaurantCommand, ErrorOr<RestaurantDto>>
{
    public async Task<ErrorOr<RestaurantDto>> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var ownerId = await dbContext.Owners
            .Where(x => x.AppUserId == request.AppUserId)
            .Select(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (ownerId == Guid.Empty) return Error.NotFound("Owner.NotFound", "Owner not found");
        
        var slug = slugHelper.GenerateSlug(request.Name);
        var isSlugPresent = await dbContext.Restaurants.AnyAsync(x => x.Slug == slug, cancellationToken);
        if (isSlugPresent)
        {
            var hash = Guid.NewGuid().ToString("N").Substring(0, 6);
            slug = $"{slug}-{hash}";
        }

        if (request.ChainId.HasValue)
        {
            var chainExists = await dbContext.Chains.AnyAsync(x => x.Id == request.ChainId &&
                x.OwnerId == ownerId, cancellationToken);
            
            if (!chainExists) return Error.NotFound("Chain.NotFound", "Chain not found");
        }

        var restaurant = new Domain.Entities.Restaurant()
        {
            Name = request.Name, Slug = slug, OwnerId = ownerId, ChainId = request.ChainId
        };
        dbContext.Restaurants.Add(restaurant);
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return new RestaurantDto() { Id = restaurant.Id, Name = request.Name, };
    }
}