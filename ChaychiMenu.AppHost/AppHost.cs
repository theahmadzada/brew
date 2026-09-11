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

var jwtIssuer = builder.AddParameter("jwt-issuer");
var jwtAudience = builder.AddParameter("jwt-audience");
var jwtSigningKey = builder.AddParameter("jwt-signing-key", secret: true);
var refreshTokenKey = builder.AddParameter("refresh-token-key", secret: true);

builder.AddProject<Projects.ChaychiMenu_WebApi>("webapi")
    .WithExternalHttpEndpoints()
    .WithReference(psql)
    .WaitFor(psql)
    .WithReference(migrations)
    .WaitForCompletion(migrations)
    .WithReference(redis)
    .WaitFor(redis)
    .WithReference(minio)
    .WaitFor(minio)
    .WithEnvironment("JwtSettings_Issuer", jwtIssuer)
    .WithEnvironment("JwtSettings_Audience", jwtAudience)
    .WithEnvironment("JwtSettings_SigningKey", jwtSigningKey)
    .WithEnvironment("RefreshTokenSettings_Key", refreshTokenKey);

builder.Build().Run();