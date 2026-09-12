using ChaychiMenu.Application.Dto;
using ChaychiMenu.Application.Queries.MenuItem;
using ChaychiMenu.Infrastructure.DbContext;

using ErrorOr;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace ChaychiMenu.Application.Handlers.MenuItem;

public class GetMenuItemByIdQueryHandler(AppDbContext dbContext) : IRequestHandler<GetMenuItemByIdQuery, ErrorOr<MenuItemDto>>
{
    public async Task<ErrorOr<MenuItemDto>> Handle(GetMenuItemByIdQuery request, CancellationToken cancellationToken)
    {
        var menuItem = await dbContext.MenuItems
            .Select(x => new MenuItemDto()
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Price = x.Price,
                CategoryId = x.CategoryId,
                Order = x.Order,
                ImageUrl = x.ImageUrl
            })
            .Where(x => x.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);
        
        if (menuItem is null) return Error.NotFound("MenuItem.NotFound", "Menu item not found");
        return menuItem;
    }
}