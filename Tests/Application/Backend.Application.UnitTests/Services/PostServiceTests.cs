using Application.Dtos.User;
using Application.Exceptions;
using Application.Interfaces;
using Application.Services;
using Bogus;
using Domain.Entities;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Application.UnitTests.Services
{
    /// <summary>
    /// Unit tests for the PostService.
    /// </summary>
    /// created by Cline 06/09/2025
    public class PostServiceTests
    {
        private readonly Mock<IPostRepository> _postRepositoryMock;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly PostService _postService;
        private readonly Faker<PostDto> _postDtoFaker;

        public PostServiceTests()
        {
            _postRepositoryMock = new Mock<IPostRepository>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _postService = new PostService(_postRepositoryMock.Object, _currentUserServiceMock.Object);

            _postDtoFaker = new Faker<PostDto>()
                .RuleFor(p => p.Id, f => f.Random.Int(1))
                .RuleFor(p => p.UserId, f => f.Random.Int(1));
        }

        [Fact]
        public async Task DeleteAsync_ShouldSucceed_WhenUserIsOwner()
        {
            // Arrange
            var userId = 1;
            var post = _postDtoFaker.Generate();
            post.UserId = userId;

            _currentUserServiceMock.Setup(s => s.GetUserId()).Returns(userId);
            _postRepositoryMock.Setup(r => r.GetByIdAsync(post.Id)).ReturnsAsync(post);
            _postRepositoryMock.Setup(r => r.DeleteAsync(post.Id)).ReturnsAsync(1);

            // Act
            var result = await _postService.DeleteAsync(post.Id);

            // Assert
            result.Should().Be(1);
            _postRepositoryMock.Verify(r => r.DeleteAsync(post.Id), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowNotSameUserException_WhenUserIsNotOwner()
        {
            // Arrange
            var ownerId = 1;
            var currentUserId = 2;
            var post = _postDtoFaker.Generate();
            post.UserId = ownerId;

            _currentUserServiceMock.Setup(s => s.GetUserId()).Returns(currentUserId);
            _postRepositoryMock.Setup(r => r.GetByIdAsync(post.Id)).ReturnsAsync(post);

            // Act
            Func<Task> act = async () => await _postService.DeleteAsync(post.Id);

            // Assert
            await act.Should().ThrowAsync<NotSameUserException>();
            _postRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
        }
    }
}
