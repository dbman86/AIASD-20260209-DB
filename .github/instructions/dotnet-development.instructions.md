---
ai_generated: true
model: "anthropic/claude-3.5-sonnet@2024-10-22"
operator: "johnmillerATcodemag-com"
chat_id: "dotnet-standards-20260210"
prompt: |
  create an instruction containing the standards and practices for these technologies
  Core Framework
  .NET 9.0 - Target framework for all projects
  C# 11+ - Programming language (with nullable reference types and implicit usings enabled)
  ASP.NET Core - Web application framework
started: "2026-02-10T19:12:10Z"
ended: "2026-02-10T19:12:10Z"
task_durations:
  - task: "requirements analysis"
    duration: "00:05:00"
  - task: "content creation"
    duration: "00:25:00"
  - task: "examples and validation"
    duration: "00:10:00"
total_duration: "00:40:00"
ai_log: "ai-logs/2026/02/10/dotnet-standards-20260210/conversation.md"
source: "johnmillerATcodemag-com"
applyTo: "**/*.{cs,csproj,sln}"
---

# .NET 9.0 Development Standards and Practices

## Overview

This instruction file defines comprehensive standards and practices for developing applications using .NET 9.0, C# 11+, and ASP.NET Core. These guidelines ensure code quality, maintainability, security, and consistency across all .NET projects in the repository.

**Target Audience**: Developers, architects, and AI assistants working with .NET technologies

**Scope**: All C# code, project files, and .NET solutions in this repository

**Related Documentation**:
- [Evergreen Development Instructions](.github/instructions/evergreen-development.instructions.md)
- [AI-Assisted Output Instructions](.github/instructions/ai-assisted-output.instructions.md)

## Table of Contents

- [Core Technology Stack](#core-technology-stack)
- [Project Configuration Standards](#project-configuration-standards)
- [C# Language Features and Practices](#c-language-features-and-practices)
- [ASP.NET Core Development](#aspnet-core-development)
- [Code Organization and Architecture](#code-organization-and-architecture)
- [Dependency Management](#dependency-management)
- [Testing Standards](#testing-standards)
- [Security Practices](#security-practices)
- [Performance Guidelines](#performance-guidelines)
- [Logging and Monitoring](#logging-and-monitoring)
- [Build and Deployment](#build-and-deployment)
- [Quality Checklist](#quality-checklist)

## Core Technology Stack

### Framework Versions

- **.NET 9.0**: Target framework for all projects (TFM: `net9.0`)
- **C# 11+**: Minimum language version with latest features enabled
- **ASP.NET Core**: Web framework for all web applications and APIs

### Required Project Settings

All `.csproj` files MUST include:

```xml
<PropertyGroup>
  <TargetFramework>net9.0</TargetFramework>
  <Nullable>enable</Nullable>
  <ImplicitUsings>enable</ImplicitUsings>
  <LangVersion>latest</LangVersion>
</PropertyGroup>
```

### SDK Requirements

- **.NET 9.0 SDK**: Minimum version 9.0.100
- **Visual Studio 2022 17.8+** or **VS Code** with C# Dev Kit
- **Git**: For version control

## Project Configuration Standards

### Project File (.csproj) Structure

#### Console Applications

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net9.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <LangVersion>latest</LangVersion>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
    <EnforceCodeStyleInBuild>true</EnforceCodeStyleInBuild>
  </PropertyGroup>
</Project>
```

#### ASP.NET Core Web Applications

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <LangVersion>latest</LangVersion>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
    <EnforceCodeStyleInBuild>true</EnforceCodeStyleInBuild>
    <UserSecretsId>unique-guid-here</UserSecretsId>
  </PropertyGroup>
</Project>
```

#### Class Libraries

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <LangVersion>latest</LangVersion>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
    <EnforceCodeStyleInBuild>true</EnforceCodeStyleInBuild>
    <GenerateDocumentationFile>true</GenerateDocumentationFile>
  </PropertyGroup>
</Project>
```

### Required PropertyGroup Settings

| Property | Value | Purpose |
|----------|-------|---------|
| `Nullable` | `enable` | Enable nullable reference types |
| `ImplicitUsings` | `enable` | Reduce using statement boilerplate |
| `LangVersion` | `latest` | Use latest C# features |
| `TreatWarningsAsErrors` | `true` | Enforce zero warnings |
| `EnforceCodeStyleInBuild` | `true` | Enforce code style rules at build time |

### Implicit Usings Configuration

When `ImplicitUsings` is enabled, these namespaces are automatically available:

**All Projects:**
- `System`
- `System.Collections.Generic`
- `System.IO`
- `System.Linq`
- `System.Net.Http`
- `System.Threading`
- `System.Threading.Tasks`

**Web Projects (additional):**
- `Microsoft.AspNetCore.Builder`
- `Microsoft.AspNetCore.Hosting`
- `Microsoft.AspNetCore.Http`
- `Microsoft.AspNetCore.Routing`
- `Microsoft.Extensions.Configuration`
- `Microsoft.Extensions.DependencyInjection`
- `Microsoft.Extensions.Hosting`
- `Microsoft.Extensions.Logging`

**Custom Implicit Usings:**

Add to `.csproj` to customize:

```xml
<ItemGroup>
  <Using Include="YourNamespace.Common" />
  <Using Remove="System.Net.Http" />
</ItemGroup>
```

## C# Language Features and Practices

### Nullable Reference Types (Required)

**Always enable nullable reference types** for null safety:

```csharp
// ✅ CORRECT: Explicit nullability
public class UserService
{
    private readonly ILogger<UserService> _logger;
    private string? _cachedValue; // Nullable
    private string _userId; // Non-nullable

    public UserService(ILogger<UserService> logger, string userId)
    {
        _logger = logger;
        _userId = userId;
    }

    public User? FindUser(int id)
    {
        // May return null
        return _repository.Find(id);
    }

    public User GetUser(int id)
    {
        // Never returns null, throws if not found
        return _repository.Find(id)
            ?? throw new NotFoundException($"User {id} not found");
    }
}

// ❌ INCORRECT: Nullable warnings ignored
#nullable disable
public class UserService
{
    private string userId; // Null safety disabled
}
```

### Modern C# Features (C# 11+)

#### File-Scoped Namespaces (Required)

```csharp
// ✅ CORRECT: File-scoped namespace
namespace MyApp.Services;

public class UserService
{
    // Implementation
}

// ❌ INCORRECT: Traditional namespace
namespace MyApp.Services
{
    public class UserService
    {
    }
}
```

#### Primary Constructors (C# 12)

```csharp
// ✅ CORRECT: Primary constructor for dependency injection
public class UserService(ILogger<UserService> logger, IUserRepository repository)
{
    public async Task<User?> GetUserAsync(int id)
    {
        logger.LogInformation("Fetching user {UserId}", id);
        return await repository.FindAsync(id);
    }
}

// Alternative: Traditional constructor (also acceptable)
public class UserService
{
    private readonly ILogger<UserService> _logger;
    private readonly IUserRepository _repository;

    public UserService(ILogger<UserService> logger, IUserRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }
}
```

#### Record Types

```csharp
// ✅ CORRECT: Records for DTOs and value objects
public record UserDto(int Id, string Email, string Name);

public record CreateUserRequest
{
    public required string Email { get; init; }
    public required string Name { get; init; }
    public string? PhoneNumber { get; init; }
}

// ✅ CORRECT: Record with validation
public record EmailAddress
{
    public string Value { get; }

    public EmailAddress(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be empty", nameof(value));

        if (!value.Contains('@'))
            throw new ArgumentException("Invalid email format", nameof(value));

        Value = value;
    }
}
```

#### Pattern Matching

```csharp
// ✅ CORRECT: Modern pattern matching
public decimal CalculateDiscount(Customer customer) => customer switch
{
    { IsPremium: true, YearsActive: > 5 } => 0.20m,
    { IsPremium: true } => 0.15m,
    { YearsActive: > 3 } => 0.10m,
    _ => 0.05m
};

// ✅ CORRECT: Type pattern matching
public string ProcessResult(Result result) => result switch
{
    SuccessResult success => $"Success: {success.Message}",
    ErrorResult error => $"Error: {error.Message}",
    WarningResult warning => $"Warning: {warning.Message}",
    _ => throw new ArgumentException($"Unknown result type: {result.GetType()}")
};
```

#### Init-Only Properties and Required Members

```csharp
// ✅ CORRECT: Required properties with init
public class CreateUserCommand
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    public string? PhoneNumber { get; init; }
}

// Usage
var command = new CreateUserCommand
{
    Email = "user@example.com",
    Password = "secure123"
    // PhoneNumber is optional
};
```

#### Target-Typed New

```csharp
// ✅ CORRECT: Target-typed new expressions
private readonly List<string> _tags = new();
private readonly Dictionary<string, int> _cache = new();

public User CreateUser()
{
    return new() // Type inferred from return type
    {
        Id = 1,
        Email = "user@example.com"
    };
}
```

### Naming Conventions

| Element | Convention | Example |
|---------|-----------|---------|
| Namespace | PascalCase | `MyApp.Services.Users` |
| Class | PascalCase | `UserService` |
| Interface | IPascalCase | `IUserRepository` |
| Method | PascalCase | `GetUserAsync` |
| Property | PascalCase | `UserName` |
| Private Field | _camelCase | `_logger` |
| Parameter | camelCase | `userId` |
| Local Variable | camelCase | `userName` |
| Constant | PascalCase | `MaxRetryCount` |
| Async Method | PascalCaseAsync | `SaveUserAsync` |

### Async/Await Patterns

```csharp
// ✅ CORRECT: Async method with proper suffix and cancellation
public async Task<User?> GetUserAsync(int id, CancellationToken cancellationToken = default)
{
    return await _repository.FindAsync(id, cancellationToken);
}

// ✅ CORRECT: Async LINQ
public async Task<List<User>> GetActiveUsersAsync(CancellationToken cancellationToken = default)
{
    var users = await _repository.GetAllAsync(cancellationToken);
    return users.Where(u => u.IsActive).ToList();
}

// ✅ CORRECT: ConfigureAwait for libraries
public async Task<string> ReadFileAsync(string path)
{
    return await File.ReadAllTextAsync(path).ConfigureAwait(false);
}

// ❌ INCORRECT: Async void (except event handlers)
public async void SaveUser(User user) // Should return Task
{
    await _repository.SaveAsync(user);
}

// ❌ INCORRECT: Blocking async code
public User GetUser(int id)
{
    return GetUserAsync(id).Result; // Deadlock risk!
}
```

## ASP.NET Core Development

### Minimal API vs Controllers

**Minimal APIs** (Recommended for simple APIs):

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/users/{id}", async (int id, IUserRepository repo) =>
{
    var user = await repo.FindAsync(id);
    return user is not null ? Results.Ok(user) : Results.NotFound();
})
.WithName("GetUser")
.WithOpenApi();

app.MapPost("/users", async (CreateUserRequest request, IUserService service) =>
{
    var user = await service.CreateUserAsync(request);
    return Results.Created($"/users/{user.Id}", user);
})
.WithName("CreateUser")
.WithOpenApi();

app.Run();
```

**Controllers** (For complex APIs with many endpoints):

```csharp
[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService, ILogger<UsersController> logger) : ControllerBase
{
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> GetUser(int id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching user {UserId}", id);

        var user = await userService.GetUserAsync(id, cancellationToken);

        return user is not null
            ? Ok(user)
            : NotFound();
    }

    [HttpPost]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserDto>> CreateUser(
        CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating user with email {Email}", request.Email);

        var user = await userService.CreateUserAsync(request, cancellationToken);

        return CreatedAtAction(
            nameof(GetUser),
            new { id = user.Id },
            user);
    }
}
```

### Dependency Injection

```csharp
// ✅ CORRECT: Service registration in Program.cs
var builder = WebApplication.CreateBuilder(args);

// Register services by lifetime
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddTransient<IEmailSender, EmailSender>();

// Register with configuration
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

// Register HttpClient with typed client
builder.Services.AddHttpClient<IExternalApiClient, ExternalApiClient>(client =>
{
    client.BaseAddress = new Uri("https://api.example.com");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// ✅ CORRECT: Constructor injection
public class UserService(
    IUserRepository repository,
    ILogger<UserService> logger,
    IOptions<AppSettings> options)
{
    private readonly AppSettings _settings = options.Value;

    public async Task<User> CreateUserAsync(CreateUserRequest request)
    {
        logger.LogInformation("Creating user {Email}", request.Email);
        // Implementation
    }
}
```

### Service Lifetimes

| Lifetime | When to Use | Example |
|----------|-------------|---------|
| **Singleton** | Stateless services, cached data, configuration | `IConfiguration`, `IMemoryCache` |
| **Scoped** | Per-request services, DB contexts | `DbContext`, `IUserRepository` |
| **Transient** | Lightweight stateless services | `IEmailSender`, `IValidator<T>` |

### Middleware Configuration

```csharp
var app = builder.Build();

// ✅ CORRECT: Middleware order matters
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.Run();
```

### Configuration Management

```csharp
// appsettings.json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MyApp;Trusted_Connection=true;"
  },
  "EmailSettings": {
    "SmtpServer": "smtp.example.com",
    "Port": 587,
    "SenderEmail": "noreply@example.com"
  }
}

// ✅ CORRECT: Strongly-typed configuration
public class EmailSettings
{
    public required string SmtpServer { get; init; }
    public required int Port { get; init; }
    public required string SenderEmail { get; init; }
}

// Registration
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

// Usage
public class EmailService(IOptions<EmailSettings> options)
{
    private readonly EmailSettings _settings = options.Value;

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        // Use _settings.SmtpServer, etc.
    }
}
```

### Model Validation

```csharp
// ✅ CORRECT: Data annotations
using System.ComponentModel.DataAnnotations;

public class CreateUserRequest
{
    [Required]
    [EmailAddress]
    [StringLength(100)]
    public required string Email { get; init; }

    [Required]
    [StringLength(100, MinimumLength = 8)]
    public required string Password { get; init; }

    [Phone]
    public string? PhoneNumber { get; init; }

    [Range(18, 120)]
    public int? Age { get; init; }
}

// ✅ CORRECT: Validate in controller
[HttpPost]
public async Task<ActionResult<UserDto>> CreateUser(CreateUserRequest request)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    var user = await _userService.CreateUserAsync(request);
    return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
}

// ✅ CORRECT: FluentValidation (for complex validation)
public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(100);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Matches(@"[A-Z]").WithMessage("Password must contain uppercase letter")
            .Matches(@"[a-z]").WithMessage("Password must contain lowercase letter")
            .Matches(@"\d").WithMessage("Password must contain digit");
    }
}
```

## Code Organization and Architecture

### Solution Structure

```
MySolution/
├── src/
│   ├── MyApp.Api/              # ASP.NET Core API
│   ├── MyApp.Core/             # Domain models, interfaces
│   ├── MyApp.Infrastructure/   # Data access, external services
│   └── MyApp.Shared/           # Shared utilities, constants
├── tests/
│   ├── MyApp.Api.Tests/
│   ├── MyApp.Core.Tests/
│   └── MyApp.Integration.Tests/
├── docs/
└── MySolution.sln
```

### Layer Responsibilities

**Core/Domain Layer:**
- Domain entities and value objects
- Business logic and domain services
- Repository interfaces
- Domain events

**Infrastructure Layer:**
- Repository implementations
- External service integrations
- Data access (Entity Framework Core)
- File system, caching

**API/Presentation Layer:**
- Controllers or minimal API endpoints
- DTOs and view models
- Authentication/Authorization
- API documentation (Swagger)

### Dependency Flow

```
API Layer → Infrastructure Layer → Core Layer
                ↓
            External Systems (DB, APIs, etc.)
```

**Rule**: Core layer has NO dependencies on infrastructure or API layers.

### File Organization

```csharp
// ✅ CORRECT: One type per file
// UserService.cs
namespace MyApp.Services;

public class UserService { }

// IUserService.cs
namespace MyApp.Services;

public interface IUserService { }

// ❌ INCORRECT: Multiple types in one file (except nested types)
// Services.cs
public class UserService { }
public class ProductService { }
public interface IUserService { }
```

## Dependency Management

### Package Version Management

**Use Central Package Management (Directory.Packages.props):**

```xml
<!-- Directory.Packages.props (in solution root) -->
<Project>
  <PropertyGroup>
    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
  </PropertyGroup>

  <ItemGroup>
    <PackageVersion Include="Microsoft.EntityFrameworkCore" Version="9.0.0" />
    <PackageVersion Include="Microsoft.EntityFrameworkCore.SqlServer" Version="9.0.0" />
    <PackageVersion Include="Swashbuckle.AspNetCore" Version="6.5.0" />
    <PackageVersion Include="FluentValidation.AspNetCore" Version="11.3.0" />
    <PackageVersion Include="Serilog.AspNetCore" Version="8.0.0" />
  </ItemGroup>
</Project>
```

**Project file references version from central location:**

```xml
<!-- MyApp.Api.csproj -->
<ItemGroup>
  <PackageReference Include="Microsoft.EntityFrameworkCore" />
  <PackageReference Include="Swashbuckle.AspNetCore" />
</ItemGroup>
```

### Essential NuGet Packages

**Web APIs:**
- `Swashbuckle.AspNetCore` - API documentation
- `FluentValidation.AspNetCore` - Model validation
- `Serilog.AspNetCore` - Structured logging
- `Microsoft.AspNetCore.Authentication.JwtBearer` - JWT auth

**Data Access:**
- `Microsoft.EntityFrameworkCore` - ORM
- `Microsoft.EntityFrameworkCore.SqlServer` - SQL Server provider
- `Dapper` - Micro-ORM for performance-critical queries

**Testing:**
- `xUnit` - Test framework
- `FluentAssertions` - Assertion library
- `Moq` - Mocking framework
- `Microsoft.AspNetCore.Mvc.Testing` - Integration testing

### Update Strategy

Follow [Evergreen Development Instructions](evergreen-development.instructions.md):

- **Security patches**: Within 24 hours (critical), 1 week (high)
- **Minor updates**: Weekly batch
- **Major updates**: Quarterly planning

## Testing Standards

### Unit Testing with xUnit

```csharp
// ✅ CORRECT: Unit test structure
public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockRepository;
    private readonly Mock<ILogger<UserService>> _mockLogger;
    private readonly UserService _sut; // System Under Test

    public UserServiceTests()
    {
        _mockRepository = new Mock<IUserRepository>();
        _mockLogger = new Mock<ILogger<UserService>>();
        _sut = new UserService(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetUserAsync_WithValidId_ReturnsUser()
    {
        // Arrange
        var userId = 1;
        var expectedUser = new User { Id = userId, Email = "test@example.com" };
        _mockRepository
            .Setup(r => r.FindAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedUser);

        // Act
        var result = await _sut.GetUserAsync(userId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(userId);
        result.Email.Should().Be("test@example.com");
    }

    [Fact]
    public async Task GetUserAsync_WithInvalidId_ReturnsNull()
    {
        // Arrange
        _mockRepository
            .Setup(r => r.FindAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _sut.GetUserAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public async Task GetUserAsync_WithInvalidId_ThrowsArgumentException(int invalidId)
    {
        // Act & Assert
        await _sut.Invoking(s => s.GetUserAsync(invalidId))
            .Should().ThrowAsync<ArgumentException>();
    }
}
```

### Integration Testing

```csharp
// ✅ CORRECT: Integration test with WebApplicationFactory
public class UsersControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public UsersControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Replace services with test implementations
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });
            });
        });

        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetUser_WithValidId_ReturnsOk()
    {
        // Arrange
        var userId = 1;

        // Act
        var response = await _client.GetAsync($"/api/users/{userId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var user = JsonSerializer.Deserialize<UserDto>(content);
        user.Should().NotBeNull();
        user!.Id.Should().Be(userId);
    }
}
```

### Test Naming Convention

**Pattern**: `MethodName_Scenario_ExpectedBehavior`

Examples:
- `GetUserAsync_WithValidId_ReturnsUser`
- `CreateUser_WithInvalidEmail_ThrowsValidationException`
- `DeleteUser_WhenUserDoesNotExist_ReturnsNotFound`

### Code Coverage Requirements

- **Minimum coverage**: 80% for business logic
- **Critical paths**: 100% coverage required
- **Infrastructure code**: Not required (e.g., Program.cs, Startup)

## Security Practices

### Authentication and Authorization

```csharp
// ✅ CORRECT: JWT authentication setup
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("RequireEmailVerified", policy =>
        policy.RequireClaim("email_verified", "true"));
});

// ✅ CORRECT: Authorize endpoint
[Authorize(Policy = "RequireAdminRole")]
[HttpDelete("{id}")]
public async Task<IActionResult> DeleteUser(int id)
{
    await _userService.DeleteUserAsync(id);
    return NoContent();
}
```

### User Secrets (Development)

```bash
# Initialize user secrets
dotnet user-secrets init

# Set secret
dotnet user-secrets set "Jwt:Key" "your-secret-key-here"
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;..."
```

```csharp
// Access in code
var jwtKey = builder.Configuration["Jwt:Key"];
```

### Input Validation and Sanitization

```csharp
// ✅ CORRECT: Validate and sanitize input
[HttpPost]
public async Task<ActionResult> CreatePost(CreatePostRequest request)
{
    // Validate
    if (!ModelState.IsValid)
        return BadRequest(ModelState);

    // Sanitize HTML to prevent XSS
    var sanitizedContent = _htmlSanitizer.Sanitize(request.Content);

    var post = new Post
    {
        Title = request.Title,
        Content = sanitizedContent,
        AuthorId = User.GetUserId()
    };

    await _postRepository.SaveAsync(post);
    return Ok(post);
}
```

### SQL Injection Prevention

```csharp
// ✅ CORRECT: Parameterized queries (EF Core)
var users = await _context.Users
    .Where(u => u.Email == email)
    .ToListAsync();

// ✅ CORRECT: Parameterized queries (Dapper)
var users = await _connection.QueryAsync<User>(
    "SELECT * FROM Users WHERE Email = @Email",
    new { Email = email });

// ❌ INCORRECT: String concatenation (SQL injection risk)
var query = $"SELECT * FROM Users WHERE Email = '{email}'";
var users = await _connection.QueryAsync<User>(query);
```

### CORS Configuration

```csharp
// ✅ CORRECT: Explicit CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("https://example.com", "https://app.example.com")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Use the policy
app.UseCors("AllowSpecificOrigin");

// ❌ INCORRECT: Allow all origins in production
app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
```

## Performance Guidelines

### Asynchronous Operations

```csharp
// ✅ CORRECT: Async all the way
public async Task<ActionResult<List<User>>> GetUsersAsync(CancellationToken cancellationToken)
{
    var users = await _repository.GetAllAsync(cancellationToken);
    return Ok(users);
}

// ❌ INCORRECT: Mixing sync and async
public ActionResult<List<User>> GetUsers()
{
    var users = _repository.GetAllAsync().Result; // Blocking!
    return Ok(users);
}
```

### Database Query Optimization

```csharp
// ✅ CORRECT: Eager loading to prevent N+1
var users = await _context.Users
    .Include(u => u.Posts)
    .ThenInclude(p => p.Comments)
    .Where(u => u.IsActive)
    .ToListAsync(cancellationToken);

// ✅ CORRECT: Projection to load only needed fields
var userSummaries = await _context.Users
    .Where(u => u.IsActive)
    .Select(u => new UserSummaryDto
    {
        Id = u.Id,
        Email = u.Email,
        PostCount = u.Posts.Count
    })
    .ToListAsync(cancellationToken);

// ❌ INCORRECT: N+1 query problem
var users = await _context.Users.ToListAsync();
foreach (var user in users)
{
    var posts = await _context.Posts.Where(p => p.AuthorId == user.Id).ToListAsync();
}
```

### Caching

```csharp
// ✅ CORRECT: Distributed caching
public class CachedUserRepository(IUserRepository repository, IDistributedCache cache)
{
    public async Task<User?> GetUserAsync(int id, CancellationToken cancellationToken)
    {
        var cacheKey = $"user:{id}";
        var cachedUser = await cache.GetStringAsync(cacheKey, cancellationToken);

        if (cachedUser != null)
        {
            return JsonSerializer.Deserialize<User>(cachedUser);
        }

        var user = await repository.FindAsync(id, cancellationToken);

        if (user != null)
        {
            var serialized = JsonSerializer.Serialize(user);
            await cache.SetStringAsync(
                cacheKey,
                serialized,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                },
                cancellationToken);
        }

        return user;
    }
}
```

### Response Compression

```csharp
// Program.cs
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});

app.UseResponseCompression();
```

## Logging and Monitoring

### Structured Logging with Serilog

```csharp
// Program.cs
using Serilog;

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/app-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Usage in services
public class UserService(ILogger<UserService> logger)
{
    public async Task<User> CreateUserAsync(CreateUserRequest request)
    {
        logger.LogInformation(
            "Creating user with email {Email}",
            request.Email);

        try
        {
            var user = await _repository.CreateAsync(request);

            logger.LogInformation(
                "User {UserId} created successfully",
                user.Id);

            return user;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to create user with email {Email}",
                request.Email);
            throw;
        }
    }
}
```

### Log Levels

| Level | When to Use | Example |
|-------|-------------|---------|
| **Trace** | Detailed diagnostic info | Method entry/exit, loop iterations |
| **Debug** | Development debugging | Variable values, conditional branches |
| **Information** | General flow | Request started, user logged in |
| **Warning** | Unexpected but handled | Retry attempt, fallback used |
| **Error** | Failed operation | Exception caught, operation failed |
| **Critical** | System failure | Database unavailable, service crashed |

### Health Checks

```csharp
// Program.cs
builder.Services.AddHealthChecks()
    .AddDbContextCheck<AppDbContext>()
    .AddUrlGroup(new Uri("https://api.example.com/health"), "External API");

app.MapHealthChecks("/health");
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});
```

## Build and Deployment

### Build Configuration

```xml
<!-- ✅ CORRECT: Production build settings -->
<PropertyGroup Condition="'$(Configuration)' == 'Release'">
  <Optimize>true</Optimize>
  <DebugType>portable</DebugType>
  <DebugSymbols>true</DebugSymbols>
  <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
</PropertyGroup>
```

### Publish Profile

```bash
# Publish for production
dotnet publish -c Release -o ./publish

# Publish self-contained
dotnet publish -c Release -r win-x64 --self-contained -o ./publish
```

### Environment-Specific Configuration

```csharp
// Program.cs
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else if (app.Environment.IsStaging())
{
    app.UseExceptionHandler("/Error");
}
else // Production
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
```

### Docker Support

```dockerfile
# Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["MyApp.Api/MyApp.Api.csproj", "MyApp.Api/"]
RUN dotnet restore "MyApp.Api/MyApp.Api.csproj"
COPY . .
WORKDIR "/src/MyApp.Api"
RUN dotnet build "MyApp.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyApp.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyApp.Api.dll"]
```

## Quality Checklist

### Project Setup
- [ ] Target framework set to `net9.0`
- [ ] `Nullable` reference types enabled
- [ ] `ImplicitUsings` enabled
- [ ] `LangVersion` set to `latest`
- [ ] `TreatWarningsAsErrors` enabled
- [ ] `EnforceCodeStyleInBuild` enabled

### Code Quality
- [ ] All methods use appropriate async/await patterns
- [ ] Proper null handling throughout
- [ ] File-scoped namespaces used
- [ ] Primary constructors or proper DI patterns
- [ ] No compiler warnings
- [ ] XML documentation for public APIs (libraries)

### ASP.NET Core Standards
- [ ] Dependency injection properly configured
- [ ] Middleware in correct order
- [ ] Configuration strongly-typed
- [ ] Model validation implemented
- [ ] Proper HTTP status codes returned
- [ ] OpenAPI/Swagger documentation

### Security
- [ ] User secrets for development configuration
- [ ] No secrets in source code
- [ ] Authentication/authorization configured
- [ ] CORS properly restricted
- [ ] Input validation and sanitization
- [ ] Parameterized queries (no SQL injection)

### Testing
- [ ] Unit tests for business logic (≥80% coverage)
- [ ] Integration tests for API endpoints
- [ ] Tests follow naming convention
- [ ] FluentAssertions used for readable assertions
- [ ] Mocking used appropriately

### Performance
- [ ] Async operations throughout
- [ ] No blocking calls on async code
- [ ] Database queries optimized
- [ ] Caching implemented where appropriate
- [ ] Response compression enabled

### Logging and Monitoring
- [ ] Structured logging configured
- [ ] Appropriate log levels used
- [ ] Sensitive data not logged
- [ ] Health checks implemented
- [ ] Application Insights or similar monitoring

### Deployment
- [ ] Release build configuration optimized
- [ ] Environment-specific settings
- [ ] Docker support (if applicable)
- [ ] CI/CD pipeline configured

## Examples

### Complete Minimal API Example

```csharp
// Program.cs
using Microsoft.EntityFrameworkCore;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Define endpoints
var users = app.MapGroup("/api/users")
    .WithTags("Users")
    .WithOpenApi();

users.MapGet("/", async (IUserService service, CancellationToken ct) =>
{
    var users = await service.GetAllUsersAsync(ct);
    return Results.Ok(users);
})
.WithName("GetAllUsers")
.Produces<List<UserDto>>();

users.MapGet("/{id}", async (int id, IUserService service, CancellationToken ct) =>
{
    var user = await service.GetUserAsync(id, ct);
    return user is not null ? Results.Ok(user) : Results.NotFound();
})
.WithName("GetUser")
.Produces<UserDto>()
.Produces(StatusCodes.Status404NotFound);

users.MapPost("/", async (
    CreateUserRequest request,
    IValidator<CreateUserRequest> validator,
    IUserService service,
    CancellationToken ct) =>
{
    var validationResult = await validator.ValidateAsync(request, ct);
    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }

    var user = await service.CreateUserAsync(request, ct);
    return Results.Created($"/api/users/{user.Id}", user);
})
.WithName("CreateUser")
.Produces<UserDto>(StatusCodes.Status201Created)
.ProducesValidationProblem();

app.Run();

// Make Program accessible for testing
public partial class Program { }
```

### Complete Service Implementation

```csharp
// IUserService.cs
namespace MyApp.Services;

public interface IUserService
{
    Task<List<UserDto>> GetAllUsersAsync(CancellationToken cancellationToken = default);
    Task<UserDto?> GetUserAsync(int id, CancellationToken cancellationToken = default);
    Task<UserDto> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default);
    Task UpdateUserAsync(int id, UpdateUserRequest request, CancellationToken cancellationToken = default);
    Task DeleteUserAsync(int id, CancellationToken cancellationToken = default);
}

// UserService.cs
namespace MyApp.Services;

public class UserService(
    IUserRepository repository,
    ILogger<UserService> logger) : IUserService
{
    public async Task<List<UserDto>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Fetching all users");

        var users = await repository.GetAllAsync(cancellationToken);

        return users.Select(u => new UserDto(u.Id, u.Email, u.Name)).ToList();
    }

    public async Task<UserDto?> GetUserAsync(int id, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Fetching user {UserId}", id);

        var user = await repository.FindAsync(id, cancellationToken);

        return user is not null
            ? new UserDto(user.Id, user.Email, user.Name)
            : null;
    }

    public async Task<UserDto> CreateUserAsync(
        CreateUserRequest request,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Creating user with email {Email}", request.Email);

        var user = new User
        {
            Email = request.Email,
            Name = request.Name,
            CreatedAt = DateTime.UtcNow
        };

        await repository.AddAsync(user, cancellationToken);

        logger.LogInformation("User {UserId} created successfully", user.Id);

        return new UserDto(user.Id, user.Email, user.Name);
    }

    public async Task UpdateUserAsync(
        int id,
        UpdateUserRequest request,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Updating user {UserId}", id);

        var user = await repository.FindAsync(id, cancellationToken)
            ?? throw new NotFoundException($"User {id} not found");

        user.Email = request.Email;
        user.Name = request.Name;
        user.UpdatedAt = DateTime.UtcNow;

        await repository.UpdateAsync(user, cancellationToken);

        logger.LogInformation("User {UserId} updated successfully", id);
    }

    public async Task DeleteUserAsync(int id, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Deleting user {UserId}", id);

        var user = await repository.FindAsync(id, cancellationToken)
            ?? throw new NotFoundException($"User {id} not found");

        await repository.DeleteAsync(user, cancellationToken);

        logger.LogInformation("User {UserId} deleted successfully", id);
    }
}
```

---

**Document Metadata:**
- **Version**: 1.0.0
- **Last Updated**: 2026-02-10
- **Maintainer**: johnmillerATcodemag-com
- **Related Standards**: Evergreen Development, AI-Assisted Output
