using Brew.Infrastructure.DbContext;
using Brew.MigrationService;
using Brew.ServiceDefaults;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<AppDbContext>("brew-db");

var host = builder.Build();
host.Run();