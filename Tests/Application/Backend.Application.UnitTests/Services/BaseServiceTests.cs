using Application.Interfaces;
using Application.Services;
using Bogus;
using Domain.Entities;
using Domain.Entities.Paging;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Application.UnitTests.Services
{
    /// <summary>
    /// Unit tests for the BaseService class.
    /// </summary>
    /// created by Cline 06/09/2025
    public class BaseServiceTests
    {
        // Dummy entity and dto for generic parameters
        public class TestEntity : BaseEntity { public string Name { get; set; } }
        public class TestDto : BaseEntity { public string Name { get; set; } }

        private readonly Mock<IBaseRepository<TestEntity, TestDto>> _repositoryMock;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly BaseService<TestEntity, TestDto> _service;
        private readonly Faker<TestDto> _dtoFaker;
        private readonly Faker<TestEntity> _entityFaker;

        public BaseServiceTests()
        {
            _repositoryMock = new Mock<IBaseRepository<TestEntity, TestDto>>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _service = new BaseService<TestEntity, TestDto>(_repositoryMock.Object, _currentUserServiceMock.Object);

            _dtoFaker = new Faker<TestDto>()
                .RuleFor(d => d.Id, f => f.Random.Int(1))
                .RuleFor(d => d.Name, f => f.Lorem.Word());

            _entityFaker = new Faker<TestEntity>()
                .RuleFor(e => e.Id, f => f.Random.Int(1))
                .RuleFor(e => e.Name, f => f.Lorem.Word());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectDto()
        {
            // Arrange
            var expectedDto = _dtoFaker.Generate();
            // Return a new object to prevent the service from mutating the expected object.
            _repositoryMock.Setup(r => r.GetByIdAsync(expectedDto.Id))
                           .ReturnsAsync(new TestDto { Id = expectedDto.Id, Name = expectedDto.Name });

            // Act
            var result = await _service.GetByIdAsync(expectedDto.Id);

            // Assert
            // Use BeEquivalentTo to compare property values, not object references.
            // This will correctly fail if the service logic is wrong.
            result.Should().BeEquivalentTo(expectedDto);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEntities()
        {
            // Arrange
            var entities = _entityFaker.Generate(3);
            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(entities);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().BeEquivalentTo(entities);
        }

        [Fact]
        public async Task AddAsync_ShouldSetCreatedByAndCallRepository()
        {
            // Arrange
            var dto = _dtoFaker.Generate();
            var userId = 42;
            _currentUserServiceMock.Setup(s => s.GetUserId()).Returns(userId);
            _repositoryMock.Setup(r => r.AddAsync(dto)).ReturnsAsync(1);

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            dto.CreatedBy.Should().Be(userId);
            result.Should().Be(1);
            _repositoryMock.Verify(r => r.AddAsync(dto), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldSetModifiedByAndCallRepository()
        {
            // Arrange
            var dto = _dtoFaker.Generate();
            var userId = 99;
            _currentUserServiceMock.Setup(s => s.GetUserId()).Returns(userId);
            _repositoryMock.Setup(r => r.UpdateAsync(dto)).ReturnsAsync(1);

            // Act
            var result = await _service.UpdateAsync(dto);

            // Assert
            dto.ModifiedBy.Should().Be(userId);
            result.Should().Be(1);
            _repositoryMock.Verify(r => r.UpdateAsync(dto), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepository()
        {
            // Arrange
            var idToDelete = 1;
            _repositoryMock.Setup(r => r.DeleteAsync(idToDelete)).ReturnsAsync(1);

            // Act
            var result = await _service.DeleteAsync(idToDelete);

            // Assert
            result.Should().Be(1);
            _repositoryMock.Verify(r => r.DeleteAsync(idToDelete), Times.Once);
        }

        [Fact]
        public async Task GetPagedAsync_ShouldReturnPagedResponse()
        {
            // Arrange
            var pagedRequest = new PagedRequest { PageIndex = 1, PageSize = 10 };
            var pagedResponse = new PagedResponse<TestDto>
            {
                Items = _dtoFaker.Generate(5),
                TotalCount = 5
            };
            _repositoryMock.Setup(r => r.GetPagedAsync(pagedRequest)).ReturnsAsync(pagedResponse);

            // Act
            var result = await _service.GetPagedAsync(pagedRequest);

            // Assert
            result.Should().BeEquivalentTo(pagedResponse);
        }

        [Fact]
        public async Task FindByFieldAsync_ShouldReturnDto()
        {
            // Arrange
            var dto = _dtoFaker.Generate();
            var fieldName = "Name";
            var value = dto.Name;
            _repositoryMock.Setup(r => r.FindByFieldAsync(fieldName, value)).ReturnsAsync(dto);

            // Act
            var result = await _service.FindByFieldAsync(fieldName, value);

            // Assert
            result.Should().Be(dto);
        }
    }
}
