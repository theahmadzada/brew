var builder = DistributedApplication.CreateBuilder(args);

var psql = builder.AddPostgres("brew")
    .WithDataVolume()
    .AddDatabase("brew-db");

builder.AddProject<Projects.Brew_WebApi>("webapi")
    .WithReference(psql)
    .WaitFor(psql);

builder.Build().Run();