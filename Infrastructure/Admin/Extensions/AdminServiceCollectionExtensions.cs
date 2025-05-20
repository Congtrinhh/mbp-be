using System;
using System.Text;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pomelo.EntityFrameworkCore.MySql;
using Infrastructure.Admin.Data;
using Application.Admin.Mapping;
using Domain.Entities;
using Application.Interfaces.Admin;
using Infrastructure.Repositories.Admin;

namespace Infrastructure.Admin.Extensions
{
    public static class AdminServiceCollectionExtensions
    {
        public static IServiceCollection AddAdminServices(this IServiceCollection services, string connectionString)
        {
            // Register DbContext
            var serverVersion = ServerVersion.AutoDetect(connectionString);
            services.AddDbContext<AdminDbContext>(options =>
                options.UseMySql(
                    connectionString,
                    serverVersion,
                    b => b.MigrationsAssembly("Infrastructure")
                )
            );

            // Register repositories
            services.AddScoped(typeof(IAdminBaseRepository<>), typeof(AdminBaseRepository<>));

            // TODO: Register admin services after implementing base service interfaces
            
            // TODO: Add AutoMapper registration after resolving package conflicts

            return services;
        }

        public static IServiceCollection AddAdminCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AdminCorsPolicy", builder =>
                {
                    builder
                        .WithOrigins(
                            "http://localhost:5173",  // Development
                            "https://admin.mcbookingplatform.com") // Production
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
                        .WithHeaders("Authorization", "Content-Type")
                        .AllowCredentials()
                        .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
                });
            });

            return services;
        }

        public static IServiceCollection AddAdminAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication()
                .AddJwtBearer("AdminScheme", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "mc-booking-platform",
                        ValidAudience = "admin-api",
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes("YourSuperSecretKeyHere-PleaseChangeThis-InProduction"))
                    };
                });

            return services;
        }

        public static IServiceCollection AddAdminAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy =>
                {
                    policy.RequireRole("Admin");
                    // Add additional admin policy requirements here
                });
            });

            return services;
        }
    }
}