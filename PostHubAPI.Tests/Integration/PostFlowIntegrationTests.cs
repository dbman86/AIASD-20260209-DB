using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PostHubAPI.Data;
using PostHubAPI.Dtos.Post;
using PostHubAPI.Profiles;
using PostHubAPI.Services.Implementations;

namespace PostHubAPI.Tests.Integration;

/// <summary>
/// Integration tests for the complete post creation and retrieval flow
/// Tests the service layer with actual database context (in-memory)
/// </summary>
[Trait("Category", "Integration")]
[Trait("Priority", "High")]
public class PostFlowIntegrationTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly PostService _postService;

    public PostFlowIntegrationTests()
    {
        // Setup in-memory database for integration testing
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB per test
            .Options;

        _context = new ApplicationDbContext(options);

        // Setup AutoMapper with the actual mapping profiles
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<PostProfile>();
            cfg.AddProfile<CommentProfile>();
        });
        _mapper = config.CreateMapper();

        _postService = new PostService(_context, _mapper);
    }

    [Fact]
    public async Task CompletePostFlow_CreateAndRetrieve_Success()
    {
        // Arrange
        var createDto = new CreatePostDto
        {
            Title = "Integration Test Post",
            Body = "This is an integration test post body"
        };

        // Act - Create post
        var postId = await _postService.CreateNewPostAsync(createDto);

        // Assert - Post ID is returned
        postId.Should().BeGreaterThan(0);

        // Act - Retrieve post
        var retrievedPost = await _postService.GetPostByIdAsync(postId);

        // Assert - Retrieved post matches created post
        retrievedPost.Should().NotBeNull();
        retrievedPost.Id.Should().Be(postId);
        retrievedPost.Title.Should().Be(createDto.Title);
        retrievedPost.Body.Should().Be(createDto.Body);
        retrievedPost.CreationTime.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task PostFlow_CreateEditDelete_Success()
    {
        // Arrange - Create a post
        var createDto = new CreatePostDto
        {
            Title = "Original Title",
            Body = "Original Body"
        };
        var postId = await _postService.CreateNewPostAsync(createDto);

        // Act - Edit the post
        var editDto = new EditPostDto
        {
            Title = "Updated Title",
            Body = "Updated Body"
        };
        var editedPost = await _postService.EditPostAsync(postId, editDto);

        // Assert - Post is updated
        editedPost.Should().NotBeNull();
        editedPost.Title.Should().Be(editDto.Title);
        editedPost.Body.Should().Be(editDto.Body);

        // Act - Delete the post
        await _postService.DeletePostAsync(postId);

        // Assert - Post is deleted
        var action = async () => await _postService.GetPostByIdAsync(postId);
        await action.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task GetAllPosts_WithMultiplePosts_ReturnsAll()
    {
        // Arrange - Create multiple posts
        var post1 = new CreatePostDto { Title = "Post 1", Body = "Body 1" };
        var post2 = new CreatePostDto { Title = "Post 2", Body = "Body 2" };
        var post3 = new CreatePostDto { Title = "Post 3", Body = "Body 3" };

        await _postService.CreateNewPostAsync(post1);
        await _postService.CreateNewPostAsync(post2);
        await _postService.CreateNewPostAsync(post3);

        // Act
        var allPosts = await _postService.GetAllPostsAsync();

        // Assert
        allPosts.Should().HaveCount(3);
        allPosts.Should().Contain(p => p.Title == "Post 1");
        allPosts.Should().Contain(p => p.Title == "Post 2");
        allPosts.Should().Contain(p => p.Title == "Post 3");
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
