using ChaychiMenu.Application.Commands.MenuItem;
using ChaychiMenu.Infrastructure.DbContext;

using ErrorOr;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace ChaychiMenu.Application.Handlers.Category;

public class ToggleMenuItemAvailabilityCommandHandler(
    AppDbContext dbContext) : IRequestHandler<ToggleMenuItemAvailabilityCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(ToggleMenuItemAvailabilityCommand request, CancellationToken cancellationToken)
    {
        var menuItem =
            await dbContext.MenuItems.FirstOrDefaultAsync(x => x.Id == request.MenuItemId, cancellationToken);
        if (menuItem is null)
            return Error.NotFound("MenuItem.NotFound", "Menu Item not Found");

        menuItem.IsAvailable = true;
        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}