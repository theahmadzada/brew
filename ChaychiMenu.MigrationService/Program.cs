using ChaychiMenu.Infrastructure.DbContext;
using ChaychiMenu.MigrationService;
using ChaychiMenu.ServiceDefaults;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<AppDbContext>("rms-db");

var host = builder.Build();
host.Run();