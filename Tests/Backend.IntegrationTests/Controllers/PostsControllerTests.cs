using Backend.IntegrationTests.Common;
using Domain.Entities;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Application.Dtos.User;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.IntegrationTests.Controllers
{
    public class PostsControllerTests : IntegrationTestBase
    {
        public PostsControllerTests(IntegrationTestWebAppFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk()
        {
            // Act
            var response = await Client.GetAsync("/api/Posts");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenPostExists()
        {
            // Arrange
            var post = new Post { Caption = "Test Post", DetailDesc = "Test Content", CreatedAt = DateTime.UtcNow, UserId = TestUser.Id, IsActive = true };
            using (var scope = Factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<Infrastructure.Data.ApplicationDbContext>();
                dbContext.Posts.Add(post);
                await dbContext.SaveChangesAsync();
            }

            // Act
            var response = await Client.GetAsync($"/api/Posts/{post.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var returnedPost = await response.Content.ReadFromJsonAsync<PostDto>();
            Assert.NotNull(returnedPost);
            Assert.Equal(post.Id, returnedPost.Id);
        }

        [Fact]
        public async Task Create_ShouldReturnCreated()
        {
            // Arrange
            var request = new PostDto { Caption = "New Post", DetailDesc = "New Content" };

            // Act
            var response = await Client.PostAsJsonAsync("/api/Posts", request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Update_ShouldReturnNoContent()
        {
            // Arrange
            var post = new Post { Caption = "Original Post", DetailDesc = "Original Content", CreatedAt = DateTime.UtcNow, UserId = TestUser.Id, IsActive = true };
            using (var scope = Factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<Infrastructure.Data.ApplicationDbContext>();
                dbContext.Posts.Add(post);
                await dbContext.SaveChangesAsync();
            }
            var request = new PostDto { Id = post.Id, Caption = "Updated Post", DetailDesc = "Updated Content" };

            // Act
            var response = await Client.PutAsJsonAsync($"/api/Posts/{post.Id}", request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent()
        {
            // Arrange
            var post = new Post { Caption = "To Be Deleted", DetailDesc = "Delete Me", CreatedAt = DateTime.UtcNow, UserId = TestUser.Id, IsActive = true };
            using (var scope = Factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<Infrastructure.Data.ApplicationDbContext>();
                dbContext.Posts.Add(post);
                await dbContext.SaveChangesAsync();
            }

            // Act
            var response = await Client.DeleteAsync($"/api/Posts/{post.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
