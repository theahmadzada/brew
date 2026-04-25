using Microsoft.AspNetCore.Identity;

namespace Brew.Domain.Entities;

public class AppUser : IdentityUser<Guid>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public long? TelegramId { get; set; }
    public bool IsDeleted { get; set; }
}