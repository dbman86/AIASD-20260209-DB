using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using PostHubAPI.Data;
using PostHubAPI.Dtos.Post;
using PostHubAPI.Exceptions;
using PostHubAPI.Models;
using PostHubAPI.Services.Implementations;

namespace PostHubAPI.Tests.Services;

[Trait("Category", "Unit")]
[Trait("Priority", "High")]
public class PostServiceTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly PostService _postService;

    public PostServiceTests()
    {
        // Setup in-memory database
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;
        _context = new ApplicationDbContext(options);

        // Setup AutoMapper with actual profiles
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Post, ReadPostDto>();
            cfg.CreateMap<CreatePostDto, Post>();
            cfg.CreateMap<EditPostDto, Post>();
        });
        _mapper = mapperConfig.CreateMapper();

        _postService = new PostService(_context, _mapper);
    }

    [Fact]
    public async Task GetAllPostsAsync_ReturnsAllPosts()
    {
        // Arrange
        var posts = new List<Post>
        {
            new Post { Id = 1, Title = "Post 1", Body = "Body 1", Comments = new List<Comment>() },
            new Post { Id = 2, Title = "Post 2", Body = "Body 2", Comments = new List<Comment>() }
        };
        _context.Posts.AddRange(posts);
        await _context.SaveChangesAsync();

        // Act
        var result = await _postService.GetAllPostsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().Contain(p => p.Title == "Post 1");
        result.Should().Contain(p => p.Title == "Post 2");
    }

    [Fact]
    public async Task GetAllPostsAsync_WithEmptyDatabase_ReturnsEmptyList()
    {
        // Act
        var result = await _postService.GetAllPostsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetPostByIdAsync_ValidId_ReturnsPost()
    {
        // Arrange
        var post = new Post { Id = 1, Title = "Test Post", Body = "Test Body", Comments = new List<Comment>() };
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        // Act
        var result = await _postService.GetPostByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be("Test Post");
        result.Body.Should().Be("Test Body");
    }

    [Fact]
    public async Task GetPostByIdAsync_InvalidId_ThrowsNotFoundException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _postService.GetPostByIdAsync(999));
    }

    [Fact]
    public async Task GetPostByIdAsync_InvalidId_ThrowsExceptionWithCorrectMessage()
    {
        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _postService.GetPostByIdAsync(999));

        // Assert
        exception.Message.Should().Be("Post not found!");
    }

    [Fact]
    public async Task CreateNewPostAsync_ValidDto_CreatesPost()
    {
        // Arrange
        var dto = new CreatePostDto
        {
            Title = "New Post",
            Body = "New Body"
        };

        // Act
        var postId = await _postService.CreateNewPostAsync(dto);

        // Assert
        postId.Should().BeGreaterThan(0);
        var savedPost = await _context.Posts.FindAsync(postId);
        savedPost.Should().NotBeNull();
        savedPost!.Title.Should().Be("New Post");
        savedPost.Body.Should().Be("New Body");
    }

    [Fact]
    public async Task CreateNewPostAsync_ValidDto_ReturnsNewPostId()
    {
        // Arrange
        var dto = new CreatePostDto
        {
            Title = "Another Post",
            Body = "Another Body"
        };

        // Act
        var postId = await _postService.CreateNewPostAsync(dto);

        // Assert
        postId.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task EditPostAsync_ValidId_UpdatesPost()
    {
        // Arrange
        var post = new Post { Id = 1, Title = "Original Title", Body = "Original Body" };
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        var editDto = new EditPostDto
        {
            Title = "Updated Title",
            Body = "Updated Body"
        };

        // Act
        var result = await _postService.EditPostAsync(1, editDto);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be("Updated Title");
        result.Body.Should().Be("Updated Body");

        var updatedPost = await _context.Posts.FindAsync(1);
        updatedPost!.Title.Should().Be("Updated Title");
        updatedPost.Body.Should().Be("Updated Body");
    }

    [Fact]
    public async Task EditPostAsync_InvalidId_ThrowsNotFoundException()
    {
        // Arrange
        var editDto = new EditPostDto
        {
            Title = "Updated Title",
            Body = "Updated Body"
        };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _postService.EditPostAsync(999, editDto));
    }

    [Fact]
    public async Task EditPostAsync_InvalidId_ThrowsExceptionWithCorrectMessage()
    {
        // Arrange
        var editDto = new EditPostDto
        {
            Title = "Updated Title",
            Body = "Updated Body"
        };

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _postService.EditPostAsync(999, editDto));

        // Assert
        exception.Message.Should().Be("Post not found!");
    }

    [Fact]
    public async Task DeletePostAsync_ValidId_DeletesPost()
    {
        // Arrange
        var post = new Post { Id = 1, Title = "Post to Delete", Body = "Body" };
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        // Act
        await _postService.DeletePostAsync(1);

        // Assert
        var deletedPost = await _context.Posts.FindAsync(1);
        deletedPost.Should().BeNull();
    }

    [Fact]
    public async Task DeletePostAsync_InvalidId_ThrowsNotFoundException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _postService.DeletePostAsync(999));
    }

    [Fact]
    public async Task DeletePostAsync_InvalidId_ThrowsExceptionWithCorrectMessage()
    {
        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _postService.DeletePostAsync(999));

        // Assert
        exception.Message.Should().Be("Post not found!");
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
