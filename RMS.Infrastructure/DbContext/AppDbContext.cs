using RMS.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RMS.Infrastructure.DbContext;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser, AppRole, Guid>(options)
{
    public DbSet<Owner> Owners { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<Table> Tables { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Chain> Chains { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<AppUser>().HasQueryFilter(x => !x.IsDeleted);
        builder.Entity<Owner>().HasQueryFilter(x => !x.AppUser.IsDeleted);
        builder.Entity<Chain>().HasQueryFilter(x => !x.Owner.AppUser.IsDeleted);
        builder.Entity<Employee>().HasQueryFilter(x => !x.AppUser.IsDeleted);
    }
}