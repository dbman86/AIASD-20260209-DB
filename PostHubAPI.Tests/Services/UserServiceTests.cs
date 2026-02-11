using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using PostHubAPI.Dtos.User;
using PostHubAPI.Models;
using PostHubAPI.Services.Implementations;
using System.IdentityModel.Tokens.Jwt;

namespace PostHubAPI.Tests.Services;

public class UserServiceTests : IDisposable
{
    private readonly Mock<UserManager<User>> _mockUserManager;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        // Setup UserManager mock
        var store = new Mock<IUserStore<User>>();
        _mockUserManager = new Mock<UserManager<User>>(
            store.Object, null, null, null, null, null, null, null, null);

        // Setup Configuration mock with JWT settings
        _mockConfiguration = new Mock<IConfiguration>();
        _mockConfiguration.Setup(c => c["JWT:Secret"]).Returns("ThisIsAVerySecretKeyForJWTTokenGenerationWithAtLeast32Characters");
        _mockConfiguration.Setup(c => c["JWT:Issuer"]).Returns("TestIssuer");
        _mockConfiguration.Setup(c => c["JWT:Audience"]).Returns("TestAudience");

        _userService = new UserService(_mockConfiguration.Object, _mockUserManager.Object);
    }

    [Fact]
    public async Task Register_NewUser_ReturnsJwtToken()
    {
        // Arrange
        var registerDto = new RegisterUserDto
        {
            Email = "test@example.com",
            Username = "testuser",
            Password = "Password123!",
            ConfirmPassword = "Password123!"
        };

        _mockUserManager.Setup(x => x.FindByEmailAsync(registerDto.Email))
            .ReturnsAsync((User?)null);

        _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), registerDto.Password))
            .ReturnsAsync(IdentityResult.Success);

        _mockUserManager.Setup(x => x.FindByNameAsync(registerDto.Username))
            .ReturnsAsync(new User
            {
                Email = registerDto.Email,
                UserName = registerDto.Username
            });

        _mockUserManager.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), registerDto.Password))
            .ReturnsAsync(true);

        // Act
        var token = await _userService.Register(registerDto);

        // Assert
        token.Should().NotBeNullOrEmpty();
        var tokenHandler = new JwtSecurityTokenHandler();
        tokenHandler.CanReadToken(token).Should().BeTrue();
    }

    [Fact]
    public async Task Register_ExistingEmail_ThrowsArgumentException()
    {
        // Arrange
        var registerDto = new RegisterUserDto
        {
            Email = "existing@example.com",
            Username = "newuser",
            Password = "Password123!",
            ConfirmPassword = "Password123!"
        };

        _mockUserManager.Setup(x => x.FindByEmailAsync(registerDto.Email))
            .ReturnsAsync(new User { Email = registerDto.Email });

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _userService.Register(registerDto));
    }

    [Fact]
    public async Task Register_ExistingEmail_ThrowsExceptionWithCorrectMessage()
    {
        // Arrange
        var registerDto = new RegisterUserDto
        {
            Email = "existing@example.com",
            Username = "newuser",
            Password = "Password123!",
            ConfirmPassword = "Password123!"
        };

        _mockUserManager.Setup(x => x.FindByEmailAsync(registerDto.Email))
            .ReturnsAsync(new User { Email = registerDto.Email });

        // Act
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _userService.Register(registerDto));

        // Assert
        exception.Message.Should().Contain("already exists");
    }

    [Fact]
    public async Task Register_FailedUserCreation_ThrowsArgumentException()
    {
        // Arrange
        var registerDto = new RegisterUserDto
        {
            Email = "test@example.com",
            Username = "testuser",
            Password = "weak",
            ConfirmPassword = "weak"
        };

        _mockUserManager.Setup(x => x.FindByEmailAsync(registerDto.Email))
            .ReturnsAsync((User?)null);

        var errors = new[]
        {
            new IdentityError { Code = "PasswordTooShort", Description = "Password is too short" }
        };

        _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), registerDto.Password))
            .ReturnsAsync(IdentityResult.Failed(errors));

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _userService.Register(registerDto));
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsJwtToken()
    {
        // Arrange
        var loginDto = new LoginUserDto
        {
            Username = "testuser",
            Password = "Password123!"
        };

        var user = new User
        {
            Email = "test@example.com",
            UserName = loginDto.Username
        };

        _mockUserManager.Setup(x => x.FindByNameAsync(loginDto.Username))
            .ReturnsAsync(user);

        _mockUserManager.Setup(x => x.CheckPasswordAsync(user, loginDto.Password))
            .ReturnsAsync(true);

        // Act
        var token = await _userService.Login(loginDto);

        // Assert
        token.Should().NotBeNullOrEmpty();
        var tokenHandler = new JwtSecurityTokenHandler();
        tokenHandler.CanReadToken(token).Should().BeTrue();
    }

    [Fact]
    public async Task Login_InvalidUsername_ThrowsArgumentException()
    {
        // Arrange
        var loginDto = new LoginUserDto
        {
            Username = "nonexistent",
            Password = "Password123!"
        };

        _mockUserManager.Setup(x => x.FindByNameAsync(loginDto.Username))
            .ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _userService.Login(loginDto));
    }

    [Fact]
    public async Task Login_InvalidUsername_ThrowsExceptionWithCorrectMessage()
    {
        // Arrange
        var loginDto = new LoginUserDto
        {
            Username = "nonexistent",
            Password = "Password123!"
        };

        _mockUserManager.Setup(x => x.FindByNameAsync(loginDto.Username))
            .ReturnsAsync((User?)null);

        // Act
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _userService.Login(loginDto));

        // Assert
        exception.Message.Should().Contain("is not registered");
    }

    [Fact]
    public async Task Login_InvalidPassword_ThrowsArgumentException()
    {
        // Arrange
        var loginDto = new LoginUserDto
        {
            Username = "testuser",
            Password = "WrongPassword!"
        };

        var user = new User
        {
            Email = "test@example.com",
            UserName = loginDto.Username
        };

        _mockUserManager.Setup(x => x.FindByNameAsync(loginDto.Username))
            .ReturnsAsync(user);

        _mockUserManager.Setup(x => x.CheckPasswordAsync(user, loginDto.Password))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _userService.Login(loginDto));
    }

    [Fact]
    public async Task Login_InvalidPassword_ThrowsExceptionWithCorrectMessage()
    {
        // Arrange
        var loginDto = new LoginUserDto
        {
            Username = "testuser",
            Password = "WrongPassword!"
        };

        var user = new User
        {
            Email = "test@example.com",
            UserName = loginDto.Username
        };

        _mockUserManager.Setup(x => x.FindByNameAsync(loginDto.Username))
            .ReturnsAsync(user);

        _mockUserManager.Setup(x => x.CheckPasswordAsync(user, loginDto.Password))
            .ReturnsAsync(false);

        // Act
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _userService.Login(loginDto));

        // Assert
        exception.Message.Should().Contain("Unable to authenticate");
    }

    [Fact]
    public async Task Login_ValidCredentials_TokenContainsCorrectClaims()
    {
        // Arrange
        var loginDto = new LoginUserDto
        {
            Username = "testuser",
            Password = "Password123!"
        };

        var user = new User
        {
            Email = "test@example.com",
            UserName = loginDto.Username
        };

        _mockUserManager.Setup(x => x.FindByNameAsync(loginDto.Username))
            .ReturnsAsync(user);

        _mockUserManager.Setup(x => x.CheckPasswordAsync(user, loginDto.Password))
            .ReturnsAsync(true);

        // Act
        var token = await _userService.Login(loginDto);

        // Assert
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);

        jwtToken.Claims.Should().Contain(c => c.Type == System.Security.Claims.ClaimTypes.Name && c.Value == loginDto.Username);
        jwtToken.Claims.Should().Contain(c => c.Type == System.Security.Claims.ClaimTypes.Email && c.Value == user.Email);
    }

    public void Dispose()
    {
        // Cleanup if needed
    }
}
