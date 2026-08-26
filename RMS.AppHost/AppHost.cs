var builder = DistributedApplication.CreateBuilder(args);

var psql = builder.AddPostgres("rms")
    .WithDataVolume()
    .AddDatabase("rms-db");

var migrations = builder.AddProject<Projects.RMS_MigrationService>("migrations")
    .WithReference(psql)
    .WaitFor(psql);

builder.AddProject<Projects.Brew_WebApi>("webapi")
    .WithReference(psql)
    .WaitFor(psql)
    .WithReference(migrations)
    .WaitForCompletion(migrations);

builder.Build().Run();