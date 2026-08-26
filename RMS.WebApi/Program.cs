using RMS.Infrastructure.DbContext;
using RMS.ServiceDefaults;

using FluentValidation;

using Microsoft.EntityFrameworkCore;

using RMS.Application;
using RMS.WebApi;
using RMS.WebApi.Endpoints;
using RMS.WebApi.ExceptionHandler;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
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
    .MapChainEndpoints();
app.Run();