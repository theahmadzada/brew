using ChaychiMenu.ServiceDefaults;

using FluentValidation;

using Microsoft.EntityFrameworkCore;

using ChaychiMenu.Application;
using ChaychiMenu.Infrastructure.DbContext;
using ChaychiMenu.WebApi;
using ChaychiMenu.WebApi.Endpoints;
using ChaychiMenu.WebApi.ExceptionHandler;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddMinioClient("minio");
builder.AddRedisDistributedCache("redis");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("rms-db")));
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureAuth(builder.Configuration);
builder.Services.ConfigureMediatr();
builder.Services.ConfigureOptions();
builder.Services.ConfigureServices();
builder.Services.AddValidatorsFromAssembly(AssemblyReference.Assembly);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapOwnerEndpoints()
    .MapRestaurantEndpoints()
    .MapChainEndpoints()
    .MapCategoryEndpoints()
    .MapMenuItemEndpoints();
app.Run();