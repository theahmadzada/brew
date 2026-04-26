using Brew.Application;
using Brew.Infrastructure.DbContext;
using Brew.ServiceDefaults;
using Brew.WebApi;
using Brew.WebApi.Endpoints;
using Brew.WebApi.ExceptionHandler;

using FluentValidation;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<AppDbContext>("brew-db");
builder.Services.AddOpenApi();
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

app.MapOwnerEndpoints();
app.Run();
