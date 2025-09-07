using Application.Dtos.User;
using Application.Interfaces;
using Application.Services;
using Bogus;
using Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Application.UnitTests.Services
{
    /// <summary>
    /// Unit tests for the MediaService.
    /// </summary>
    /// created by Cline 06/09/2025
    public class MediaServiceTests
    {
        private readonly Mock<IMediaRepository> _mediaRepositoryMock;
        private readonly Mock<IS3Service> _s3ServiceMock;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly MediaService _mediaService;
        private readonly Faker<MediaDto> _mediaDtoFaker;

        public MediaServiceTests()
        {
            _mediaRepositoryMock = new Mock<IMediaRepository>();
            _s3ServiceMock = new Mock<IS3Service>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _mediaService = new MediaService(_mediaRepositoryMock.Object, _s3ServiceMock.Object, _currentUserServiceMock.Object);

            _mediaDtoFaker = new Faker<MediaDto>()
                .RuleFor(m => m.Id, f => f.Random.Int(1))
                .RuleFor(m => m.Url, f => f.Internet.Url());
        }

        [Fact]
        public async Task AddAsync_ShouldThrowException_WhenFileIsNull()
        {
            // Arrange
            var mediaDto = new MediaDto { File = null };

            // Act
            Func<Task> act = async () => await _mediaService.AddAsync(mediaDto);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("file to upload cannot be null");
        }

        [Theory]
        [InlineData(MediaType.Image, "images")]
        [InlineData(MediaType.Video, "videos")]
        [InlineData(MediaType.Audio, "audios")]
        public async Task AddAsync_ShouldUploadFileAndCallBaseAddAsync(MediaType mediaType, string expectedPrefix)
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            var memoryStream = new MemoryStream();
            fileMock.Setup(f => f.OpenReadStream()).Returns(memoryStream);
            fileMock.Setup(f => f.ContentType).Returns("image/jpeg");

            var mediaDto = new MediaDto { File = fileMock.Object, Type = mediaType };
            var uploadedUrl = "http://s3.com/test.jpg";
            var userId = 1;

            _s3ServiceMock.Setup(s => s.UploadFileAsync(It.IsAny<Stream>(), It.Is<string>(s => s.StartsWith(expectedPrefix)), It.IsAny<string>()))
                .ReturnsAsync(uploadedUrl);
            _currentUserServiceMock.Setup(s => s.GetUserId()).Returns(userId);
            _mediaRepositoryMock.Setup(r => r.AddAsync(It.IsAny<MediaDto>())).ReturnsAsync(1);

            // Act
            var result = await _mediaService.AddAsync(mediaDto);

            // Assert
            result.Should().Be(1);
            mediaDto.Url.Should().Be(uploadedUrl);
            mediaDto.CreatedBy.Should().Be(userId);
            _s3ServiceMock.Verify(s => s.UploadFileAsync(memoryStream, It.Is<string>(s => s.StartsWith(expectedPrefix)), "image/jpeg"), Times.Once);
            _mediaRepositoryMock.Verify(r => r.AddAsync(mediaDto), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowException_WhenMediaNotFound()
        {
            // Arrange
            _mediaRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((MediaDto)null);

            // Act
            Func<Task> act = async () => await _mediaService.DeleteAsync(1);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Cannot find media to delete");
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteFromS3AndCallBaseDeleteAsync()
        {
            // Arrange
            var media = _mediaDtoFaker.Generate();
            var s3Path = "images/test.jpg";

            _mediaRepositoryMock.Setup(r => r.GetByIdAsync(media.Id)).ReturnsAsync(media);
            _s3ServiceMock.Setup(s => s.GetS3ObjectPath(media.Url)).Returns(s3Path);
            _s3ServiceMock.Setup(s => s.DeleteFileAsync(s3Path)).Returns(Task.CompletedTask);
            _mediaRepositoryMock.Setup(r => r.DeleteAsync(media.Id)).ReturnsAsync(1);

            // Act
            var result = await _mediaService.DeleteAsync(media.Id);

            // Assert
            result.Should().Be(1);
            _s3ServiceMock.Verify(s => s.DeleteFileAsync(s3Path), Times.Once);
            _mediaRepositoryMock.Verify(r => r.DeleteAsync(media.Id), Times.Once);
        }
    }
}
