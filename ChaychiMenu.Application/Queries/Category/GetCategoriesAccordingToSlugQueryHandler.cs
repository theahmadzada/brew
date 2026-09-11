using ChaychiMenu.Application.Commands.Category;
using ChaychiMenu.Application.Dto;
using ChaychiMenu.Infrastructure.DbContext;

using ErrorOr;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace ChaychiMenu.Application.Queries.Category;

public class GetCategoriesAccordingToSlugQueryHandler(AppDbContext dbContext)
    : IRequestHandler<GetCategoriesAccordingToSlugQuery, ErrorOr<List<GetCategoriesAccordintToSlugDto>>>
{
    public async Task<ErrorOr<List<GetCategoriesAccordintToSlugDto>>> Handle(GetCategoriesAccordingToSlugQuery request,
        CancellationToken cancellationToken)
    {
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