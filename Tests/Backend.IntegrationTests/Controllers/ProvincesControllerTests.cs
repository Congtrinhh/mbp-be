using System.Net;
using System.Threading.Tasks;
using Backend.IntegrationTests.Common;
using Xunit;

namespace Backend.IntegrationTests.Controllers;

/// <summary>
/// integration test for provinces controller
/// </summary>
/// created by tqcong 07/09/2025
public class ProvincesControllerTests : IntegrationTestBase
{
    public ProvincesControllerTests(IntegrationTestWebAppFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async Task HealthCheck_ShouldReturnOk()
    {
        // Act
        var response = await Client.GetAsync("/api/health");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
