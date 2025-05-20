using Application.Interfaces;
using Application.Services;
using Api.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Extensions
{
    /// <summary>
    /// Extension methods for authentication setup
    /// created by: roo 20/04/2025
    /// </summary>
    public static class AuthenticationExtensions
    {
        /// <summary>
        /// Add authentication related services to the service collection
        /// </summary>
        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
        {
            // Register HttpContextAccessor
            services.AddHttpContextAccessor();

            // Register CurrentUserService
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            return services;
        }

        /// <summary>
        /// Configure authentication middleware in the application pipeline
        /// </summary>
        public static IApplicationBuilder UseJwtAuthentication(this IApplicationBuilder app)
        {
            app.UseMiddleware<JwtAuthenticationMiddleware>();

            return app;
        }
    }
}