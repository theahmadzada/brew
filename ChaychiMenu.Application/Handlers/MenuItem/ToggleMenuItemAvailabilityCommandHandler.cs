using ChaychiMenu.Application.Commands.MenuItem;
using ChaychiMenu.Domain;
using ChaychiMenu.Infrastructure.DbContext;

using ErrorOr;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace ChaychiMenu.Application.Handlers.MenuItem;

public class ToggleMenuItemAvailabilityCommandHandler(
    AppDbContext dbContext) : IRequestHandler<ToggleMenuItemAvailabilityCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(ToggleMenuItemAvailabilityCommand request, CancellationToken cancellationToken)
    {
        var query = dbContext.MenuItems.Where(x => x.Id == request.MenuItemId);
        if(request.UserRole != UserRole.Admin)
            query = query.Where(x => x.Category.Restaurant.Owner.AppUserId == request.AppUserId);
        
        var menuItem = await query.FirstOrDefaultAsync(cancellationToken);
        if (menuItem is null)
            return Error.NotFound("MenuItem.NotFound", "Menu Item not Found");

        menuItem.IsAvailable = request.IsAvailable;
        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}