using Application.Dtos.User;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using FluentAssertions;
using Moq;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Application.UnitTests.Services
{
    /// <summary>
    /// Unit tests for the UserService.
    /// </summary>
    /// created by Cline 24/08/2025
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IS3Service> _s3ServiceMock;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly IUserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _s3ServiceMock = new Mock<IS3Service>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _userService = new UserService(
                _userRepositoryMock.Object, 
                _s3ServiceMock.Object, 
                _currentUserServiceMock.Object
            );
        }

        [Fact]
        public async Task UploadAvatarAsync_ShouldReturnUserDtoWithAvatarUrl_WhenUploadIsSuccessful()
        {
            // Arrange
            var userId = 1;
            var fileName = "avatar.jpg";
            var contentType = "image/jpeg";
            var newAvatarUrl = "http://example.com/new-avatar.jpg";
            var oldAvatarUrl = "http://example.com/old-avatar.jpg";
            var userDto = new UserDto { Id = userId, AvatarUrl = oldAvatarUrl };

            using var fileStream = new MemoryStream();

            _currentUserServiceMock.Setup(s => s.GetUserId()).Returns(userId);
            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(userDto);
            _s3ServiceMock.Setup(s => s.UploadFileAsync(It.IsAny<Stream>(), It.IsAny<string>(), contentType)).ReturnsAsync(newAvatarUrl);
            _userRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<UserDto>())).ReturnsAsync(1);
            _s3ServiceMock.Setup(s => s.DeleteFileAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            _s3ServiceMock.Setup(s => s.GetS3ObjectPath(oldAvatarUrl)).Returns("images/old-avatar.jpg");

            // Act
            var result = await _userService.UploadAvatarAsync(userId, fileStream, fileName, contentType);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(userId);
            result.AvatarUrl.Should().Be(newAvatarUrl);

            _userRepositoryMock.Verify(r => r.UpdateAsync(It.Is<UserDto>(u => u.Id == userId && u.AvatarUrl == newAvatarUrl)), Times.Once);
            _s3ServiceMock.Verify(s => s.DeleteFileAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task UploadAvatarAsync_ShouldThrowUnauthorizedAccessException_WhenUserTriesToUpdateAnotherUser()
        {
            // Arrange
            var currentUserId = 1;
            var targetUserId = 2;
            var fileName = "avatar.jpg";
            var contentType = "image/jpeg";
            using var fileStream = new MemoryStream();

            _currentUserServiceMock.Setup(s => s.GetUserId()).Returns(currentUserId);

            // Act
            Func<Task> act = async () => await _userService.UploadAvatarAsync(targetUserId, fileStream, fileName, contentType);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedAccessException>().WithMessage("Cannot update avatar for another user");
        }

        [Fact]
        public async Task UploadAvatarAsync_ShouldThrowException_WhenUserNotFound()
        {
            // Arrange
            var userId = 1;
            var fileName = "avatar.jpg";
            var contentType = "image/jpeg";
            using var fileStream = new MemoryStream();

            _currentUserServiceMock.Setup(s => s.GetUserId()).Returns(userId);
            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync((UserDto)null);

            // Act
            Func<Task> act = async () => await _userService.UploadAvatarAsync(userId, fileStream, fileName, contentType);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("User not found");
        }
    }
}
