using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PostHubAPI.Controllers;
using PostHubAPI.Dtos.Comment;
using PostHubAPI.Exceptions;
using PostHubAPI.Services.Interfaces;

namespace PostHubAPI.Tests.Controllers;

[Trait("Category", "Unit")]
[Trait("Priority", "High")]
public class CommentControllerTests
{
    private readonly Mock<ICommentService> _mockCommentService;
    private readonly CommentController _controller;

    public CommentControllerTests()
    {
        _mockCommentService = new Mock<ICommentService>();
        _controller = new CommentController(_mockCommentService.Object);

        // Setup HttpContext for Request.Scheme and Request.Host
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
        _controller.HttpContext.Request.Scheme = "https";
        _controller.HttpContext.Request.Host = new HostString("localhost:5001");
    }

    [Fact]
    public async Task GetComment_ValidId_ReturnsOkResultWithComment()
    {
        // Arrange
        var commentId = 1;
        var comment = new ReadCommentDto { Id = commentId, Body = "Test Comment", CreationTime = DateTime.UtcNow };
        _mockCommentService.Setup(s => s.GetCommentAsync(commentId)).ReturnsAsync(comment);

        // Act
        var result = await _controller.GetComment(commentId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(comment);
    }

    [Fact]
    public async Task GetComment_InvalidId_ReturnsNotFound()
    {
        // Arrange
        var invalidId = 999;
        _mockCommentService.Setup(s => s.GetCommentAsync(invalidId))
            .ThrowsAsync(new NotFoundException($"Comment with id {invalidId} was not found"));

        // Act
        var result = await _controller.GetComment(invalidId);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult!.Value.Should().Be($"Comment with id {invalidId} was not found");
    }

    [Fact]
    public async Task CreateNewComment_ValidPostIdAndDto_ReturnsCreatedResult()
    {
        // Arrange
        var postId = 1;
        var createDto = new CreateCommentDto { Body = "New Comment" };
        var newCommentId = 1;
        _mockCommentService.Setup(s => s.CreateNewCommnentAsync(postId, createDto)).ReturnsAsync(newCommentId);

        // Act
        var result = await _controller.CreateNewComment(postId, createDto);

        // Assert
        result.Should().BeOfType<CreatedResult>();
        var createdResult = result as CreatedResult;
        createdResult!.Value.Should().Be(newCommentId);
        createdResult.Location.Should().Contain($"/api/Comment/{newCommentId}");
    }

    [Fact]
    public async Task CreateNewComment_InvalidPostId_ReturnsNotFound()
    {
        // Arrange
        var invalidPostId = 999;
        var createDto = new CreateCommentDto { Body = "New Comment" };
        _mockCommentService.Setup(s => s.CreateNewCommnentAsync(invalidPostId, createDto))
            .ThrowsAsync(new NotFoundException($"Post with id {invalidPostId} was not found"));

        // Act
        var result = await _controller.CreateNewComment(invalidPostId, createDto);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task CreateNewComment_InvalidModelState_ReturnsBadRequest()
    {
        // Arrange
        var postId = 1;
        var createDto = new CreateCommentDto { Body = "" };
        _controller.ModelState.AddModelError("Body", "Body is required");

        // Act
        var result = await _controller.CreateNewComment(postId, createDto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task EditComment_ValidIdAndDto_ReturnsOkResult()
    {
        // Arrange
        var commentId = 1;
        var editDto = new EditCommentDto { Body = "Updated Comment" };
        var editedComment = new ReadCommentDto { Id = commentId, Body = editDto.Body, CreationTime = DateTime.UtcNow };
        _mockCommentService.Setup(s => s.EditCommentAsync(commentId, editDto)).ReturnsAsync(editedComment);

        // Act
        var result = await _controller.EditComment(commentId, editDto);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(editedComment);
    }

    [Fact]
    public async Task EditComment_InvalidId_ReturnsNotFound()
    {
        // Arrange
        var invalidId = 999;
        var editDto = new EditCommentDto { Body = "Updated Comment" };
        _mockCommentService.Setup(s => s.EditCommentAsync(invalidId, editDto))
            .ThrowsAsync(new NotFoundException($"Comment with id {invalidId} was not found"));

        // Act
        var result = await _controller.EditComment(invalidId, editDto);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task EditComment_InvalidModelState_ReturnsBadRequest()
    {
        // Arrange
        var commentId = 1;
        var editDto = new EditCommentDto { Body = "" };
        _controller.ModelState.AddModelError("Body", "Body is required");

        // Act
        var result = await _controller.EditComment(commentId, editDto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task DeleteComment_ValidId_ReturnsNoContent()
    {
        // Arrange
        var commentId = 1;
        _mockCommentService.Setup(s => s.DeleteCommentAsync(commentId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteComment(commentId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteComment_InvalidId_ReturnsNotFound()
    {
        // Arrange
        var invalidId = 999;
        _mockCommentService.Setup(s => s.DeleteCommentAsync(invalidId))
            .ThrowsAsync(new NotFoundException($"Comment with id {invalidId} was not found"));

        // Act
        var result = await _controller.DeleteComment(invalidId);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }
}
