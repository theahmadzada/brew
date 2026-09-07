var builder = DistributedApplication.CreateBuilder(args);

builder.AddDockerComposeEnvironment("docker-compose");

var psql = builder.AddPostgres("rms")
    .WithDataVolume()
    .AddDatabase("rms-db");

var redis = builder.AddRedis("redis");

var migrations = builder.AddProject<Projects.ChaychiMenu_MigrationService>("migrations")
    .WithReference(psql)
    .WaitFor(psql);

var minio = builder.AddMinioContainer("minio")
    .WithDataVolume();

builder.AddProject<Projects.ChaychiMenu_WebApi>("webapi")
    .WithExternalHttpEndpoints()
    .WithReference(psql)
    .WaitFor(psql)
    .WithReference(migrations)
    .WaitForCompletion(migrations)
    .WithReference(redis)
    .WaitFor(redis)
    .WithReference(minio)
    .WaitFor(minio);

builder.Build().Run();