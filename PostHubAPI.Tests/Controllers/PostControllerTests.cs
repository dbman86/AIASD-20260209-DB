using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PostHubAPI.Controllers;
using PostHubAPI.Dtos.Post;
using PostHubAPI.Exceptions;
using PostHubAPI.Services.Interfaces;

namespace PostHubAPI.Tests.Controllers;

[Trait("Category", "Unit")]
[Trait("Priority", "High")]
public class PostControllerTests
{
    private readonly Mock<IPostService> _mockPostService;
    private readonly PostController _controller;

    public PostControllerTests()
    {
        _mockPostService = new Mock<IPostService>();
        _controller = new PostController(_mockPostService.Object);
        
        // Setup HttpContext for Request.Scheme and Request.Host
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
        _controller.HttpContext.Request.Scheme = "https";
        _controller.HttpContext.Request.Host = new HostString("localhost:5001");
    }

    [Fact]
    public async Task GetAllPosts_ReturnsOkResultWithPosts()
    {
        // Arrange
        var posts = new List<ReadPostDto>
        {
            new ReadPostDto { Id = 1, Title = "Post 1", Body = "Body 1", CreationTime = DateTime.UtcNow },
            new ReadPostDto { Id = 2, Title = "Post 2", Body = "Body 2", CreationTime = DateTime.UtcNow }
        };
        _mockPostService.Setup(s => s.GetAllPostsAsync()).ReturnsAsync(posts);

        // Act
        var result = await _controller.GetAllPosts();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(posts);
    }

    [Fact]
    public async Task GetPostById_ValidId_ReturnsOkResultWithPost()
    {
        // Arrange
        var postId = 1;
        var post = new ReadPostDto { Id = postId, Title = "Test Post", Body = "Test Body", CreationTime = DateTime.UtcNow };
        _mockPostService.Setup(s => s.GetPostByIdAsync(postId)).ReturnsAsync(post);

        // Act
        var result = await _controller.GetPostById(postId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(post);
    }

    [Fact]
    public async Task GetPostById_InvalidId_ReturnsNotFound()
    {
        // Arrange
        var invalidId = 999;
        _mockPostService.Setup(s => s.GetPostByIdAsync(invalidId))
            .ThrowsAsync(new NotFoundException($"Post with id {invalidId} was not found"));

        // Act
        var result = await _controller.GetPostById(invalidId);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult!.Value.Should().Be($"Post with id {invalidId} was not found");
    }

    [Fact]
    public async Task CreatePost_ValidDto_ReturnsCreatedResult()
    {
        // Arrange
        var createDto = new CreatePostDto { Title = "New Post", Body = "New Body" };
        var newPostId = 1;
        _mockPostService.Setup(s => s.CreateNewPostAsync(createDto)).ReturnsAsync(newPostId);

        // Act
        var result = await _controller.CreatePost(createDto);

        // Assert
        result.Should().BeOfType<CreatedResult>();
        var createdResult = result as CreatedResult;
        createdResult!.Value.Should().Be(newPostId);
        createdResult.Location.Should().Contain($"/api/Post/{newPostId}");
    }

    [Fact]
    public async Task CreatePost_InvalidModelState_ReturnsBadRequest()
    {
        // Arrange
        var createDto = new CreatePostDto { Title = "", Body = "" };
        _controller.ModelState.AddModelError("Title", "Title is required");

        // Act
        var result = await _controller.CreatePost(createDto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task EditPost_ValidIdAndDto_ReturnsOkResult()
    {
        // Arrange
        var postId = 1;
        var editDto = new EditPostDto { Title = "Updated Title", Body = "Updated Body" };
        var editedPost = new ReadPostDto { Id = postId, Title = editDto.Title, Body = editDto.Body, CreationTime = DateTime.UtcNow };
        _mockPostService.Setup(s => s.EditPostAsync(postId, editDto)).ReturnsAsync(editedPost);

        // Act
        var result = await _controller.EditPost(postId, editDto);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(editedPost);
    }

    [Fact]
    public async Task EditPost_InvalidId_ReturnsNotFound()
    {
        // Arrange
        var invalidId = 999;
        var editDto = new EditPostDto { Title = "Updated Title", Body = "Updated Body" };
        _mockPostService.Setup(s => s.EditPostAsync(invalidId, editDto))
            .ThrowsAsync(new NotFoundException($"Post with id {invalidId} was not found"));

        // Act
        var result = await _controller.EditPost(invalidId, editDto);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task EditPost_InvalidModelState_ReturnsBadRequest()
    {
        // Arrange
        var postId = 1;
        var editDto = new EditPostDto { Title = "", Body = "" };
        _controller.ModelState.AddModelError("Title", "Title is required");

        // Act
        var result = await _controller.EditPost(postId, editDto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task DeletePost_ValidId_ReturnsNoContent()
    {
        // Arrange
        var postId = 1;
        _mockPostService.Setup(s => s.DeletePostAsync(postId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeletePost(postId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeletePost_InvalidId_ReturnsNotFound()
    {
        // Arrange
        var invalidId = 999;
        _mockPostService.Setup(s => s.DeletePostAsync(invalidId))
            .ThrowsAsync(new NotFoundException($"Post with id {invalidId} was not found"));

        // Act
        var result = await _controller.DeletePost(invalidId);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }
}
