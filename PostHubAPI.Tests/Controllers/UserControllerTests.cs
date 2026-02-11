using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PostHubAPI.Controllers;
using PostHubAPI.Dtos.User;
using PostHubAPI.Services.Interfaces;

namespace PostHubAPI.Tests.Controllers;

[Trait("Category", "Unit")]
[Trait("Priority", "Critical")]
public class UserControllerTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly UserController _controller;

    public UserControllerTests()
    {
        _mockUserService = new Mock<IUserService>();
        _controller = new UserController(_mockUserService.Object);
    }

    [Fact]
    public async Task Register_ValidDto_ReturnsOkResultWithToken()
    {
        // Arrange
        var registerDto = new RegisterUserDto
        {
            Email = "test@example.com",
            Username = "testuser",
            Password = "Test123!",
            ConfirmPassword = "Test123!"
        };
        var expectedToken = "mock-jwt-token";
        _mockUserService.Setup(s => s.Register(registerDto)).ReturnsAsync(expectedToken);

        // Act
        var result = await _controller.Register(registerDto);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(expectedToken);
    }

    [Fact]
    public async Task Register_ExistingEmail_ReturnsBadRequest()
    {
        // Arrange
        var registerDto = new RegisterUserDto
        {
            Email = "existing@example.com",
            Username = "testuser",
            Password = "Test123!",
            ConfirmPassword = "Test123!"
        };

        _mockUserService.Setup(s => s.Register(registerDto))
            .ThrowsAsync(new ArgumentException("Email already exists"));

        // Act
        var result = await _controller.Register(registerDto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.Value.Should().Be("Email already exists");
    }

    [Fact]
    public async Task Register_InvalidModelState_ReturnsBadRequest()
    {
        // Arrange
        var registerDto = new RegisterUserDto
        {
            Email = "",
            Username = "",
            Password = "",
            ConfirmPassword = ""
        };
        _controller.ModelState.AddModelError("Email", "Email is required");

        // Act
        var result = await _controller.Register(registerDto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsOkResultWithToken()
    {
        // Arrange
        var loginDto = new LoginUserDto
        {
            Username = "testuser",
            Password = "Test123!"
        };
        var expectedToken = "mock-jwt-token";
        _mockUserService.Setup(s => s.Login(loginDto)).ReturnsAsync(expectedToken);

        // Act
        var result = await _controller.Login(loginDto);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(expectedToken);
    }

    [Fact]
    public async Task Login_InvalidCredentials_ReturnsBadRequest()
    {
        // Arrange
        var loginDto = new LoginUserDto
        {
            Username = "testuser",
            Password = "WrongPassword"
        };

        _mockUserService.Setup(s => s.Login(loginDto))
            .ThrowsAsync(new ArgumentException("Invalid credentials"));

        // Act
        var result = await _controller.Login(loginDto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.Value.Should().Be("Invalid credentials");
    }

    [Fact]
    public async Task Login_InvalidModelState_ReturnsBadRequest()
    {
        // Arrange
        var loginDto = new LoginUserDto
        {
            Username = "",
            Password = ""
        };
        _controller.ModelState.AddModelError("Username", "Username is required");

        // Act
        var result = await _controller.Login(loginDto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }
}
