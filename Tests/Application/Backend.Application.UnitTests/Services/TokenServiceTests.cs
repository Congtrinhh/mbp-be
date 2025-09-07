using Application.Dtos.User;
using Application.Services;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Xunit;

namespace Backend.Application.UnitTests.Services
{
    /// <summary>
    /// Unit tests for the TokenService.
    /// </summary>
    /// created by Cline 06/09/2025
    public class TokenServiceTests
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly TokenService _tokenService;
        private readonly Faker<UserDto> _userDtoFaker;

        public TokenServiceTests()
        {
            _configurationMock = new Mock<IConfiguration>();
            
            // Mock configuration values
            _configurationMock.Setup(c => c["Jwt:Key"]).Returns("a-super-secret-key-for-testing-purpose-only");
            _configurationMock.Setup(c => c["Jwt:Issuer"]).Returns("TestIssuer");
            _configurationMock.Setup(c => c["Jwt:Audience"]).Returns("TestAudience");

            _tokenService = new TokenService(_configurationMock.Object);

            _userDtoFaker = new Faker<UserDto>()
                .RuleFor(u => u.Id, f => f.Random.Int(1))
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.FullName, f => f.Name.FullName())
                .RuleFor(u => u.AvatarUrl, f => f.Internet.Avatar())
                .RuleFor(u => u.IsMc, f => f.Random.Bool())
                .RuleFor(u => u.Gender, f => f.PickRandom<Domain.Enums.Gender>())
                .RuleFor(u => u.NickName, f => f.Name.FirstName());
        }

        [Fact]
        public void GenerateToken_WithValidUser_ReturnsTokenWithCorrectClaims()
        {
            // Arrange
            var user = _userDtoFaker.Generate();

            // Act
            var tokenString = _tokenService.GenerateToken(user);

            // Assert
            tokenString.Should().NotBeNullOrEmpty();

            var handler = new JwtSecurityTokenHandler();
            var decodedToken = handler.ReadJwtToken(tokenString);

            decodedToken.Issuer.Should().Be("TestIssuer");
            decodedToken.Audiences.First().Should().Be("TestAudience");

            decodedToken.Claims.First(c => c.Type == "id").Value.Should().Be(user.Id.ToString());
            decodedToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Email).Value.Should().Be(user.Email);
            decodedToken.Claims.First(c => c.Type == "fullName").Value.Should().Be(user.FullName);
            decodedToken.Claims.First(c => c.Type == "avatarUrl").Value.Should().Be(user.AvatarUrl);
            decodedToken.Claims.First(c => c.Type == "isMc").Value.Should().Be(user.IsMc.ToString().ToLowerInvariant());
            decodedToken.Claims.First(c => c.Type == "gender").Value.Should().Be(user.Gender.ToString());
            decodedToken.Claims.First(c => c.Type == "nickName").Value.Should().Be(user.NickName);
        }

        [Fact]
        public void GenerateToken_WithNullUserProperties_HandlesNullsGracefully()
        {
            // Arrange
            var user = new UserDto
            {
                Id = 1,
                Email = null,
                FullName = null,
                AvatarUrl = null,
                IsMc = null,
                Gender = null,
                NickName = null
            };

            // Act
            var tokenString = _tokenService.GenerateToken(user);

            // Assert
            tokenString.Should().NotBeNullOrEmpty();

            var handler = new JwtSecurityTokenHandler();
            var decodedToken = handler.ReadJwtToken(tokenString);

            decodedToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Email).Value.Should().Be(string.Empty);
            decodedToken.Claims.First(c => c.Type == "fullName").Value.Should().Be(string.Empty);
            decodedToken.Claims.First(c => c.Type == "avatarUrl").Value.Should().Be(string.Empty);
            decodedToken.Claims.First(c => c.Type == "isMc").Value.Should().Be("false");
            decodedToken.Claims.First(c => c.Type == "gender").Value.Should().Be(string.Empty);
            decodedToken.Claims.First(c => c.Type == "nickName").Value.Should().Be(string.Empty);
        }
    }
}
