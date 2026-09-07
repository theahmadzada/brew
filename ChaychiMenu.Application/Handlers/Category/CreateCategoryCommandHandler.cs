using ChaychiMenu.Application.Commands.Category;
using ChaychiMenu.Application.Dto;
using ChaychiMenu.Infrastructure.DbContext;

using ErrorOr;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace ChaychiMenu.Application.Handlers.Category;

public class CreateCategoryCommandHandler(AppDbContext dbContext) : IRequestHandler<CreateCategoryCommand, ErrorOr<CategoryDto>>
{
    public async Task<ErrorOr<CategoryDto>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var restaurantId = await dbContext.Restaurants
            .Where(x => x.Id == request.RestaurantId && x.Owner.AppUserId == request.AppUserId)
            .Select(restaurant => restaurant.Id)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (restaurantId == Guid.Empty)
            return Error.NotFound("Restaurant.NotFound", "Restaurant not found");
        
        var category = new Domain.Entities.Category()
        {
            Name = request.Name, 
            RestaurantId = restaurantId, 
        };
        
        if (request.Order is null)
        {
            var categories = await dbContext.Categories
                .Where(x => x.RestaurantId == request.RestaurantId && x.Restaurant.Owner.AppUserId == request.AppUserId)
                .CountAsync(cancellationToken);
            category.Order = categories;
        }
        else
        {
            category.Order = request.Order.Value;
        }
        
        dbContext.Categories.Add(category);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CategoryDto()
        {
            Id = category.Id, 
            Name = category.Name, 
            Order = category.Order
        };
    }
}