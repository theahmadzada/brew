using ChaychiMenu.Application.Dto;
using ChaychiMenu.Application.Queries.Category;
using ChaychiMenu.Infrastructure.DbContext;

using ErrorOr;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace ChaychiMenu.Application.Handlers.Category;

public class GetCategoriesAccordingToSlugQueryHandler(AppDbContext dbContext)
    : IRequestHandler<GetCategoriesAccordingToSlugQuery, ErrorOr<List<GetCategoriesAccordintToSlugDto>>>
{
    public async Task<ErrorOr<List<GetCategoriesAccordintToSlugDto>>> Handle(GetCategoriesAccordingToSlugQuery request,
        CancellationToken cancellationToken)
    {
        var restaurantExists = await dbContext.Restaurants
            .AnyAsync(x => x.Slug == request.Slug, cancellationToken);
        if(!restaurantExists)
            return Error.NotFound("Restaurant.NotFound", "Restaurant not found");
        
        return await dbContext.Categories
            .AsNoTracking()
            .Where(x => x.Restaurant.Slug == request.Slug)
            .OrderBy(x => x.Order)
            .Select(x => new GetCategoriesAccordintToSlugDto()
            {
                Id = x.Id,
                RestaurantId = x.Restaurant.Id,
                Name = x.Name,
                MenuItems = x.MenuItems
                    .OrderBy(y => y.Order)
                    .Select(y => new MenuItemMinimalDto()
                    {
                        Id = y.Id, Order = y.Order, Price = y.Price, Title = y.Title
                    })
                    .ToList()
            })
            .ToListAsync(cancellationToken);
    }
}