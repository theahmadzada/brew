using System.Diagnostics;

using ChaychiMenu.Domain.Entities;
using ChaychiMenu.Infrastructure.DbContext;

using Microsoft.EntityFrameworkCore;

namespace ChaychiMenu.MigrationService;

public class Worker(
    IServiceProvider serviceProvider,
    IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
    public const string ActivitySourceName = "Migrations";
    private static readonly ActivitySource s_activitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(
        CancellationToken cancellationToken)
    {
        using var activity = s_activitySource.StartActivity(
            "Migrating database", ActivityKind.Client);

        try
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await RunMigrationAsync(dbContext, cancellationToken);
            await SeedDataAsync(dbContext, cancellationToken);
        }
        catch (Exception ex)
        {
            activity?.AddException(ex);
            throw;
        }

        hostApplicationLifetime.StopApplication();
    }

    private static async Task RunMigrationAsync(
        AppDbContext dbContext, CancellationToken cancellationToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await dbContext.Database.MigrateAsync(cancellationToken);
        });
    }

    private static async Task SeedDataAsync(
        AppDbContext dbContext, CancellationToken cancellationToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await dbContext.Database
                .BeginTransactionAsync(cancellationToken);

            if (dbContext.Roles.Any()) return;
            
            dbContext.Roles.Add(new AppRole()
            {
                Id = Guid.NewGuid(), Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = Guid.NewGuid().ToString(),
            });
    
            dbContext.Roles.Add(new AppRole()
            {
                Id = Guid.NewGuid(), Name = "Guest", NormalizedName = "GUEST", ConcurrencyStamp = Guid.NewGuid().ToString(),
            });
    
            dbContext.Roles.Add(new AppRole() 
            { 
                Id = Guid.NewGuid(), Name = "Employee", NormalizedName = "EMPLOYEE", ConcurrencyStamp = Guid.NewGuid().ToString(),
            });
    
            dbContext.Roles.Add(new AppRole()
            {
                Id = Guid.NewGuid(), Name = "Owner", NormalizedName = "OWNER", ConcurrencyStamp = Guid.NewGuid().ToString(),
            });
            
            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        });
    }
}