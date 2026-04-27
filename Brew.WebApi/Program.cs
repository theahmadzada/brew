using Brew.Application;
using Brew.Infrastructure.DbContext;
using Brew.ServiceDefaults;
using Brew.WebApi;
using Brew.WebApi.Endpoints;
using Brew.WebApi.ExceptionHandler;

using FluentValidation;

using Microsoft.EntityFrameworkCore;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("brew-db")));
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
    .MapChainEndpoints();
app.Run();
