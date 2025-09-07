using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Testcontainers.MySql;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Threading.Tasks;

namespace Backend.IntegrationTests.Common;

/// <summary>
/// custom web app factory for integration test
/// </summary>
/// <typeparam name="TProgram">a class that has a main method</typeparam>
/// created by tqcong 07/09/2025
public class IntegrationTestWebAppFactory<TProgram> : WebApplicationFactory<TProgram>, IAsyncLifetime where TProgram : class
{
    private readonly MySqlContainer _dbContainer = new MySqlBuilder()
        .WithImage("mysql:8.0")
        .WithDatabase("test_db")
        .WithUsername("test")
        .WithPassword("test")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseMySql(_dbContainer.GetConnectionString(), ServerVersion.AutoDetect(_dbContainer.GetConnectionString()));
            });
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }
}
