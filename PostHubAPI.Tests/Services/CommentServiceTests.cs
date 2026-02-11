using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PostHubAPI.Data;
using PostHubAPI.Dtos.Comment;
using PostHubAPI.Exceptions;
using PostHubAPI.Models;
using PostHubAPI.Services.Implementations;

namespace PostHubAPI.Tests.Services;

public class CommentServiceTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly CommentService _commentService;

    public CommentServiceTests()
    {
        // Setup in-memory database
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;
        _context = new ApplicationDbContext(options);

        // Setup AutoMapper
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Comment, ReadCommentDto>();
            cfg.CreateMap<CreateCommentDto, Comment>();
            cfg.CreateMap<EditCommentDto, Comment>();
        });
        _mapper = mapperConfig.CreateMapper();

        _commentService = new CommentService(_context, _mapper);
    }

    [Fact]
    public async Task GetCommentAsync_ValidId_ReturnsComment()
    {
        // Arrange
        var post = new Post { Id = 1, Title = "Test Post", Body = "Body" };
        var comment = new Comment
        {
            Id = 1,
            Body = "Test Comment",
            PostId = 1,
            Post = post
        };
        _context.Posts.Add(post);
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        // Act
        var result = await _commentService.GetCommentAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Body.Should().Be("Test Comment");
    }

    [Fact]
    public async Task GetCommentAsync_InvalidId_ThrowsNotFoundException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _commentService.GetCommentAsync(999));
    }

    [Fact]
    public async Task GetCommentAsync_InvalidId_ThrowsExceptionWithCorrectMessage()
    {
        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _commentService.GetCommentAsync(999));

        // Assert
        exception.Message.Should().Be("Comment not found!!");
    }

    [Fact]
    public async Task CreateNewCommentAsync_ValidPostId_CreatesComment()
    {
        // Arrange
        var post = new Post { Id = 1, Title = "Test Post", Body = "Body" };
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        var dto = new CreateCommentDto
        {
            Body = "New Comment"
        };

        // Act
        var commentId = await _commentService.CreateNewCommnentAsync(1, dto);

        // Assert
        commentId.Should().BeGreaterThan(0);
        var savedComment = await _context.Comments.FindAsync(commentId);
        savedComment.Should().NotBeNull();
        savedComment!.Body.Should().Be("New Comment");
        savedComment.PostId.Should().Be(1);
    }

    [Fact]
    public async Task CreateNewCommentAsync_InvalidPostId_ThrowsNotFoundException()
    {
        // Arrange
        var dto = new CreateCommentDto
        {
            Body = "New Comment"
        };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _commentService.CreateNewCommnentAsync(999, dto));
    }

    [Fact]
    public async Task CreateNewCommentAsync_InvalidPostId_ThrowsExceptionWithCorrectMessage()
    {
        // Arrange
        var dto = new CreateCommentDto
        {
            Body = "New Comment"
        };

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _commentService.CreateNewCommnentAsync(999, dto));

        // Assert
        exception.Message.Should().Be("Post not found!");
    }

    [Fact]
    public async Task EditCommentAsync_ValidId_UpdatesComment()
    {
        // Arrange
        var post = new Post { Id = 1, Title = "Test Post", Body = "Body" };
        var comment = new Comment
        {
            Id = 1,
            Body = "Original Comment",
            PostId = 1,
            Post = post
        };
        _context.Posts.Add(post);
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        var editDto = new EditCommentDto
        {
            Body = "Updated Comment"
        };

        // Act
        var result = await _commentService.EditCommentAsync(1, editDto);

        // Assert
        result.Should().NotBeNull();
        result.Body.Should().Be("Updated Comment");

        var updatedComment = await _context.Comments.FindAsync(1);
        updatedComment!.Body.Should().Be("Updated Comment");
    }

    [Fact]
    public async Task EditCommentAsync_InvalidId_ThrowsNotFoundException()
    {
        // Arrange
        var editDto = new EditCommentDto
        {
            Body = "Updated Comment"
        };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _commentService.EditCommentAsync(999, editDto));
    }

    [Fact]
    public async Task EditCommentAsync_InvalidId_ThrowsExceptionWithCorrectMessage()
    {
        // Arrange
        var editDto = new EditCommentDto
        {
            Body = "Updated Comment"
        };

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _commentService.EditCommentAsync(999, editDto));

        // Assert
        exception.Message.Should().Be("Comment not found!");
    }

    [Fact]
    public async Task DeleteCommentAsync_ValidId_DeletesComment()
    {
        // Arrange
        var post = new Post { Id = 1, Title = "Test Post", Body = "Body" };
        var comment = new Comment
        {
            Id = 1,
            Body = "Comment to Delete",
            PostId = 1,
            Post = post
        };
        _context.Posts.Add(post);
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        // Act
        await _commentService.DeleteCommentAsync(1);

        // Assert
        var deletedComment = await _context.Comments.FindAsync(1);
        deletedComment.Should().BeNull();
    }

    [Fact]
    public async Task DeleteCommentAsync_InvalidId_ThrowsNotFoundException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _commentService.DeleteCommentAsync(999));
    }

    [Fact]
    public async Task DeleteCommentAsync_InvalidId_ThrowsExceptionWithCorrectMessage()
    {
        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _commentService.DeleteCommentAsync(999));

        // Assert
        exception.Message.Should().Be("Comment not found!");
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
