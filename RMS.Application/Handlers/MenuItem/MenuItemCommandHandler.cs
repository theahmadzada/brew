using ErrorOr;

using MediatR;

using RMS.Application.Commands.MenuItem;
using RMS.Application.Dto;
using RMS.Infrastructure.DbContext;
using RMS.Infrastructure.ServiceContracts;

namespace RMS.Application.Handlers.MenuItem;

public class MenuItemCommandHandler(
    AppDbContext dbContext, 
    IFileStorageService storage) : IRequestHandler<CreateMenuItemCommand, ErrorOr<MenuItemDto>>
{
    public async Task<ErrorOr<MenuItemDto>> Handle(CreateMenuItemCommand request, CancellationToken cancellationToken)
    {
        string? imageUrl = null;
        if (request.Image is not null && request.ContentType is not null)
            imageUrl = await storage.UploadAsync(request.Image, request.ContentType, cancellationToken);

        var menuItem = new RMS.Domain.Entities.MenuItem
        {
            Title = request.Title,
            Description = request.Description,
            CategoryId = request.CategoryId,
            Price = request.Price,
            Order = request.Order,
            ImageUrl = imageUrl
        };

        dbContext.MenuItems.Add(menuItem);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new MenuItemDto
        {
            Id = menuItem.Id,
            Title = menuItem.Title,
            Description = menuItem.Description,
            CategoryId = menuItem.CategoryId,
            Price = menuItem.Price,
            Order = menuItem.Order,
            ImageUrl = menuItem.ImageUrl
        };
    }
}