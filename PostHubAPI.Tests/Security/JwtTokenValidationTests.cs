using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace PostHubAPI.Tests.Security;

/// <summary>
/// Tests for JWT token validation logic
/// Ensures tokens are generated and validated correctly
/// </summary>
[Trait("Category", "Security")]
[Trait("Priority", "Critical")]
public class JwtTokenValidationTests
{
    private readonly IConfiguration _configuration;
    private readonly string _validSecret = "ThisIsAVerySecretKeyForJWTTokenGenerationWithAtLeast32Characters";
    private readonly string _validIssuer = "TestIssuer";
    private readonly string _validAudience = "TestAudience";

    public JwtTokenValidationTests()
    {
        var configData = new Dictionary<string, string>
        {
            {"JWT:Secret", _validSecret},
            {"JWT:Issuer", _validIssuer},
            {"JWT:Audience", _validAudience}
        };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configData!)
            .Build();
    }

    [Fact]
    public void ValidToken_CanBeGenerated()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "testuser"),
            new Claim(ClaimTypes.Email, "test@example.com")
        };

        // Act
        var token = GenerateToken(claims);

        // Assert
        token.Should().NotBeNullOrEmpty();
        var handler = new JwtSecurityTokenHandler();
        handler.CanReadToken(token).Should().BeTrue();
    }

    [Fact]
    public void ValidToken_ContainsExpectedClaims()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "testuser"),
            new Claim(ClaimTypes.Email, "test@example.com")
        };

        // Act
        var token = GenerateToken(claims);
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        // Assert
        jwtToken.Claims.Should().Contain(c => c.Type == ClaimTypes.Name && c.Value == "testuser");
        jwtToken.Claims.Should().Contain(c => c.Type == ClaimTypes.Email && c.Value == "test@example.com");
    }

    [Fact]
    public void ValidToken_HasCorrectIssuerAndAudience()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "testuser")
        };

        // Act
        var token = GenerateToken(claims);
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        // Assert
        jwtToken.Issuer.Should().Be(_validIssuer);
        jwtToken.Audiences.Should().Contain(_validAudience);
    }

    [Fact]
    public void ValidToken_HasExpirationTime()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "testuser")
        };

        // Act
        var token = GenerateToken(claims);
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        // Assert
        jwtToken.ValidTo.Should().BeAfter(DateTime.UtcNow);
        jwtToken.ValidTo.Should().BeBefore(DateTime.UtcNow.AddHours(4));
    }

    [Fact]
    public void ExpiredToken_CanBeDetected()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "testuser")
        };

        // Act
        var token = GenerateToken(claims, expiresInMinutes: -10); // Already expired
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        // Assert
        jwtToken.ValidTo.Should().BeBefore(DateTime.UtcNow,
            "Expired tokens should have ValidTo in the past");
    }

    [Fact]
    public void TokenWithInvalidSignature_FailsValidation()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "testuser")
        };
        var token = GenerateToken(claims);

        // Tamper with the token (change last character)
        var tamperedToken = token.Substring(0, token.Length - 1) + "X";

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _validIssuer,
            ValidAudience = _validAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_validSecret))
        };

        var handler = new JwtSecurityTokenHandler();

        // Act & Assert
        Action act = () => handler.ValidateToken(tamperedToken, validationParameters, out _);
        act.Should().Throw<SecurityTokenException>();
    }

    [Fact]
    public void MalformedToken_CannotBeRead()
    {
        // Arrange
        var malformedToken = "not.a.valid.jwt.token.format";
        var handler = new JwtSecurityTokenHandler();

        // Act
        var canRead = handler.CanReadToken(malformedToken);

        // Assert
        canRead.Should().BeFalse();
    }

    [Fact]
    public void TokenWithWrongSecret_FailsValidation()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "testuser")
        };
        var token = GenerateToken(claims);

        var wrongValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _validIssuer,
            ValidAudience = _validAudience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("WrongSecretKeyThatDoesNotMatchTheTokenSignature123456"))
        };

        var handler = new JwtSecurityTokenHandler();

        // Act & Assert
        Action act = () => handler.ValidateToken(token, wrongValidationParameters, out _);
        act.Should().Throw<SecurityTokenSignatureKeyNotFoundException>();
    }

    [Fact]
    public void TokenWithMissingClaims_StillValid()
    {
        // Arrange
        var claims = new List<Claim>(); // Empty claims

        // Act
        var token = GenerateToken(claims);
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        // Assert
        jwtToken.Should().NotBeNull();
        jwtToken.Claims.Should().NotBeEmpty(); // JWT adds standard claims
    }

    [Fact]
    public void ValidToken_CanBeValidatedSuccessfully()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "testuser"),
            new Claim(ClaimTypes.Email, "test@example.com")
        };
        var token = GenerateToken(claims);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _validIssuer,
            ValidAudience = _validAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_validSecret))
        };

        var handler = new JwtSecurityTokenHandler();

        // Act
        var principal = handler.ValidateToken(token, validationParameters, out var validatedToken);

        // Assert
        principal.Should().NotBeNull();
        validatedToken.Should().NotBeNull();
        principal.Claims.Should().Contain(c => c.Type == ClaimTypes.Name && c.Value == "testuser");
    }

    // Helper method to generate tokens for testing
    private string GenerateToken(IEnumerable<Claim> claims, int expiresInMinutes = 180)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_validSecret));

        var token = new JwtSecurityToken(
            issuer: _validIssuer,
            audience: _validAudience,
            expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
            claims: claims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
