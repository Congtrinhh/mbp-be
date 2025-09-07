using Xunit;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Data;
using Respawn;
using Microsoft.EntityFrameworkCore;
using Application.Dtos.User;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using Domain.Entities;
using System;
using System.Net.Http;

namespace Backend.IntegrationTests.Common;

public class IntegrationTestBase : IClassFixture<IntegrationTestWebAppFactory<Program>>, IAsyncLifetime
{
    protected readonly IntegrationTestWebAppFactory<Program> Factory;
    protected readonly HttpClient Client;
    private Respawner _respawner;
    protected UserDto TestUser { get; private set; }

    public IntegrationTestBase(IntegrationTestWebAppFactory<Program> factory)
    {
        Factory = factory;
        Client = Factory.CreateClient();
        TestUser = new UserDto
        {
            Id = 1,
            Email = "test.user@example.com",
            FullName = "Test User",
            IsMc = false
        };
    }

    public async Task InitializeAsync()
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var connectionString = dbContext.Database.GetConnectionString();
        
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Database connection string is not configured.");
        }

        _respawner = await Respawner.CreateAsync(connectionString, new RespawnerOptions
        {
            DbAdapter = DbAdapter.MySql
        });

        await ResetDatabase();
        await SeedUserAsync();
        await AuthenticateClientAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    protected async Task ResetDatabase()
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var connectionString = dbContext.Database.GetConnectionString();
        if (!string.IsNullOrEmpty(connectionString))
        {
            await _respawner.ResetAsync(connectionString);
        }
    }

    protected async Task SeedUserAsync()
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var user = new User
        {
            Id = TestUser.Id,
            Email = TestUser.Email,
            FullName = TestUser.FullName,
            IsMc = TestUser.IsMc ?? false,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
    }

    protected async Task AuthenticateClientAsync()
    {
        using var scope = Factory.Services.CreateScope();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var token = AuthenticationHelper.GenerateToken(TestUser, configuration);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
