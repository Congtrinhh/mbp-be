using Application.Dtos;
using Application.Dtos.User;
using Application.Interfaces;
using Application.Services;
using Bogus;
using Domain.Entities;
using FluentAssertions;
using Google.Apis.Auth;
using Moq;

namespace Backend.Application.UnitTests.Services;

/// <summary>
/// Contains unit tests for the AuthService.
/// </summary>
/// created by Cline 21/08/2025
public class AuthServiceTests
{
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly Mock<IUserService> _userServiceMock;
    private readonly AuthService _sut;
    private readonly Faker<UserDto> _userDtoFaker;
    private readonly Faker<GoogleJsonWebSignature.Payload> _payloadFaker;

    public AuthServiceTests()
    {
        _tokenServiceMock = new Mock<ITokenService>();
        _userServiceMock = new Mock<IUserService>();
        _sut = new AuthService(_tokenServiceMock.Object, _userServiceMock.Object);

        _userDtoFaker = new Faker<UserDto>()
            .RuleFor(u => u.Id, f => f.Random.Int(1))
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.FullName, f => f.Name.FullName());

        _payloadFaker = new Faker<GoogleJsonWebSignature.Payload>()
            .RuleFor(p => p.Email, (f, u) => f.Internet.Email());
    }

    [Fact]
    public async Task LoginAsync_ExistingUser_ReturnsTokenAndNotNewUser()
    {
        // Arrange
        var payload = _payloadFaker.Generate();
        var loginRequest = new GoogleLoginRequestDto { IsCreateUser = false };
        var existingUser = _userDtoFaker.Generate();
        var expectedToken = "jwt_token_string";

        _userServiceMock.Setup(x => x.FindByFieldAsync("Email", payload.Email))
            .ReturnsAsync(existingUser);

        _tokenServiceMock.Setup(x => x.GenerateToken(existingUser))
            .Returns(expectedToken);

        // Act
        var result = await _sut.LoginAsync(payload, loginRequest);

        // Assert
        result.Should().BeOfType<GoogleLoginResponseDto>();
        var response = result as GoogleLoginResponseDto;
        response.AccessToken.Should().Be(expectedToken);
        response.IsNewUser.Should().BeFalse();
    }

    [Fact]
    public async Task LoginAsync_NewUser_ReturnsIsNewUserTrue()
    {
        // Arrange
        var payload = _payloadFaker.Generate();
        var loginRequest = new GoogleLoginRequestDto { IsCreateUser = false };

        _userServiceMock.Setup(x => x.FindByFieldAsync("Email", payload.Email))
            .ReturnsAsync((UserDto)null);

        // Act
        var result = await _sut.LoginAsync(payload, loginRequest);

        // Assert
        result.Should().BeOfType<GoogleLoginResponseDto>();
        var response = result as GoogleLoginResponseDto;
        response.AccessToken.Should().BeEmpty();
        response.IsNewUser.Should().BeTrue();
    }

    [Fact]
    public async Task LoginAsync_CreateUser_ReturnsTokenAndNotNewUser()
    {
        // Arrange
        var payload = _payloadFaker.Generate();
        var loginRequest = new GoogleLoginRequestDto { IsCreateUser = true, IsMc = true, IsNewbie = true };
        var expectedToken = "jwt_token_string";

        _userServiceMock.Setup(x => x.AddAsync(It.IsAny<UserDto>()))
            .ReturnsAsync(1); // Simulate successful user creation

        _tokenServiceMock.Setup(x => x.GenerateToken(It.IsAny<UserDto>()))
            .Returns(expectedToken);

        // Act
        var result = await _sut.LoginAsync(payload, loginRequest);

        // Assert
        result.Should().BeOfType<GoogleLoginResponseDto>();
        var response = result as GoogleLoginResponseDto;
        response.AccessToken.Should().Be(expectedToken);
        response.IsNewUser.Should().BeFalse();
    }

    [Fact]
    public async Task LoginAsync_CreateUserFails_ThrowsException()
    {
        // Arrange
        var payload = _payloadFaker.Generate();
        var loginRequest = new GoogleLoginRequestDto { IsCreateUser = true };

        _userServiceMock.Setup(x => x.AddAsync(It.IsAny<UserDto>()))
            .ReturnsAsync(0); // Simulate failed user creation

        // Act
        Func<Task> act = async () => await _sut.LoginAsync(payload, loginRequest);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Couldn't create new user");
    }
}
