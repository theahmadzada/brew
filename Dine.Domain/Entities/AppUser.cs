using Microsoft.AspNetCore.Identity;

namespace Dine.Domain.Entities;

public class AppUser : IdentityUser<Guid>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public List<Restaurant> Restaurants { get; set; } = [];
}