using System.Net;
using System.Net.Http.Headers;
using FluentAssertions;
using PostHubAPI.Tests.Infrastructure;

namespace PostHubAPI.Tests.Security;

/// <summary>
/// Tests for authorization enforcement on protected endpoints
/// Verifies [Authorize] attributes work correctly
/// </summary>
[Trait("Category", "Security")]
[Trait("Priority", "Critical")]
public class AuthorizationTests : IClassFixture<PostHubTestFactory>
{
    private readonly PostHubTestFactory _factory;
    private readonly HttpClient _client;

    public AuthorizationTests(PostHubTestFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CommentEndpoint_WithoutAuthentication_Returns401()
    {
        // Arrange - No authentication header

        // Act
        var response = await _client.GetAsync("/api/Comment/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task CommentEndpoint_GetComment_WithoutToken_Returns401()
    {
        // Act
        var response = await _client.GetAsync("/api/Comment/999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized,
            "CommentController has [Authorize] attribute and should reject unauthenticated requests");
    }

    [Fact]
    public async Task CommentEndpoint_CreateComment_WithoutToken_Returns401()
    {
        // Arrange
        var content = new StringContent(
            "{\"body\":\"Test comment\"}",
            System.Text.Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PostAsync("/api/Comment/1", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task CommentEndpoint_EditComment_WithoutToken_Returns401()
    {
        // Arrange
        var content = new StringContent(
            "{\"body\":\"Updated comment\"}",
            System.Text.Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PutAsync("/api/Comment/1", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task CommentEndpoint_DeleteComment_WithoutToken_Returns401()
    {
        // Act
        var response = await _client.DeleteAsync("/api/Comment/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task CommentEndpoint_WithInvalidToken_Returns401()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", "invalid-token-12345");

        // Act
        var response = await _client.GetAsync("/api/Comment/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized,
            "Invalid JWT tokens should be rejected");
    }

    [Fact]
    public async Task CommentEndpoint_WithMalformedToken_Returns401()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", "not.a.valid.jwt");

        // Act
        var response = await _client.GetAsync("/api/Comment/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task PostEndpoint_WithoutAuthentication_AllowsAccess()
    {
        // Arrange - Post endpoints are NOT protected

        // Act
        var response = await _client.GetAsync("/api/Post");

        // Assert
        response.StatusCode.Should().NotBe(HttpStatusCode.Unauthorized,
            "Post endpoints should be accessible without authentication");
    }

    [Fact]
    public async Task UserEndpoint_Register_AllowsAccess()
    {
        // Arrange
        var content = new StringContent(
            "{\"email\":\"test@example.com\",\"username\":\"testuser\",\"password\":\"Pass123!\",\"confirmPassword\":\"Pass123!\"}",
            System.Text.Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PostAsync("/api/User/Register", content);

        // Assert
        // Should not return 401 (may return 400 or other validation errors, but not 401)
        response.StatusCode.Should().NotBe(HttpStatusCode.Unauthorized,
            "Registration endpoint should be accessible without authentication");
    }

    [Fact]
    public async Task UserEndpoint_Login_AllowsAccess()
    {
        // Arrange
        var content = new StringContent(
            "{\"username\":\"testuser\",\"password\":\"Pass123!\"}",
            System.Text.Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PostAsync("/api/User/Login", content);

        // Assert
        response.StatusCode.Should().NotBe(HttpStatusCode.Unauthorized,
            "Login endpoint should be accessible without authentication");
    }
}
