var builder = DistributedApplication.CreateBuilder(args);

var psql = builder.AddPostgres("brew")
    .WithDataVolume()
    .AddDatabase("brew-db");

var migrations = builder.AddProject<Projects.Brew_MigrationService>("migrations")
    .WithReference(psql)
    .WaitFor(psql);

builder.AddProject<Projects.Brew_WebApi>("webapi")
    .WithReference(psql)
    .WaitFor(psql)
    .WithReference(migrations)
    .WaitForCompletion(migrations);

builder.Build().Run();