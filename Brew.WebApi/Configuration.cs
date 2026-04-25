using Brew.Application.Common;
using Brew.Application.ServiceContracts;
using Brew.Application.Services;
using Brew.Domain.Entities;
using Brew.Infrastructure.DbContext;
using Brew.Shared;

using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using AssemblyReference = Brew.Application.AssemblyReference;

namespace Brew.WebApi;

public static class Configuration
{
    public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentity<AppUser, AppRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
            options.Password.RequiredLength = 8;
            options.User.RequireUniqueEmail = true;
        }).AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
        
        return services;
    }

    public static IServiceCollection ConfigureAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudiences = configuration.GetSection("JwtSettings:Audience").Get<string[]>(),
                    ValidIssuers = configuration.GetSection("JwtSettings:Issuer").Get<string[]>(),
                    IssuerSigningKey = configuration.GetSection("JwtSettings:Key").Get<SymmetricSecurityKey>(),
                };
            });

        services.AddAuthorization();
        return services;
    }

    public static IServiceCollection ConfigureMediatr(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(AssemblyReference.Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        });
        
        return services;
    }

    public static IServiceCollection ConfigureOptions(this IServiceCollection services)
    {
        services.AddOptions<JwtSettings>()
            .BindConfiguration(JwtSettings.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddOptions<RefreshTokenSettings>()
            .BindConfiguration(RefreshTokenSettings.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        return services;
    }

    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddSingleton<IJwtService, JwtService>();
        return services;
    }
}