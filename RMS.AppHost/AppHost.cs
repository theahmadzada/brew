var builder = DistributedApplication.CreateBuilder(args);

var psql = builder.AddPostgres("rms")
    .WithDataVolume()
    .AddDatabase("rms-db");

var redis = builder.AddRedis("redis");

var migrations = builder.AddProject<Projects.RMS_MigrationService>("migrations")
    .WithReference(psql)
    .WaitFor(psql)
    .WithReference(redis)
    .WaitFor(redis);

builder.AddProject<Projects.RMS_WebApi>("webapi")
    .WithReference(psql)
    .WaitFor(psql)
    .WithReference(migrations)
    .WaitForCompletion(migrations);

builder.Build().Run();