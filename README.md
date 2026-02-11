# PostHubAPI

The **PostHubAPI** is a blog API that provides complete CRUD (Create, Read, Update, Delete) functionalities for Posts and Comments, along with user registration capabilities.

## Technologies Used

- **C#**: Programming language used for API logic development.
- **ASP.NET Web API**: Framework used to create RESTful APIs on the .NET platform.
- **Entity Framework In Memory**: Object-Relational Mapping (ORM) tool that simplifies database access and manipulation.
- **AutoMapper**: Library simplifying object mapping in .NET applications.

## Features

- **CRUD for Posts**: Create, read, update, and delete blog posts.
- **CRUD for Comments**: Manage comments associated with each post.
- **User Registration**: Enable user registration and management to interact with the blog.
- **Feature Flags**: Comprehensive feature flag system for controlled rollout and A/B testing.
  - Configuration-based feature flags
  - Percentage-based gradual rollouts
  - A/B testing with multiple variants
  - User-specific feature overrides
  - API endpoints to query feature status

## How to Use

1. **Clone the Repository**:
```

git clone https://github.com/your-username/PostHubAPI.git

```
2. **Environment Setup**:
- Ensure you have Visual Studio or another .NET compatible IDE installed.
- Confirm that Entity Framework and AutoMapper are configured in the environment.

3. **Execution**:
- Open the project in your IDE.
- Run the application.

4. **Using the API**:
- Utilize the provided routes and endpoints to interact with the API functionalities.

## Feature Flags

PostHubAPI includes a comprehensive feature flag system for controlled rollout and A/B testing.

### Configuration

Feature flags are configured in `appsettings.json`:

```json
{
  "FeatureFlags": {
    "Flags": [
      {
        "Name": "NewFeature",
        "Enabled": false,
        "Description": "Example feature flag",
        "RolloutPercentage": 50
      },
      {
        "Name": "ABTestFeature",
        "Enabled": true,
        "Description": "A/B testing example",
        "Variants": ["control", "variant-a", "variant-b"]
      },
      {
        "Name": "PremiumFeature",
        "Enabled": true,
        "Description": "Feature for specific users",
        "AllowedUsers": ["admin@example.com"]
      }
    ]
  }
}
```

### Feature Flag Properties

- **Name**: Unique identifier for the feature flag
- **Enabled**: Master switch for the feature (true/false)
- **Description**: Human-readable description
- **RolloutPercentage**: Percentage of users who see the feature (0-100)
- **Variants**: Array of variant names for A/B testing
- **AllowedUsers**: List of user IDs that always have access (override)

### API Endpoints

- `GET /api/FeatureFlag` - Get all feature flags for current user
- `GET /api/FeatureFlag/{featureName}` - Check specific feature status
- `GET /api/FeatureFlag/ab-test/my-variant` - Get assigned A/B test variant
- `GET /api/FeatureFlag/experimental/new-feature` - Example feature-gated endpoint

### Usage in Code

#### 1. Inject the Service

```csharp
public class MyController : ControllerBase
{
    private readonly IFeatureFlagService _featureFlagService;

    public MyController(IFeatureFlagService featureFlagService)
    {
        _featureFlagService = featureFlagService;
    }
}
```

#### 2. Check Feature Flags Programmatically

```csharp
public IActionResult MyAction()
{
    if (_featureFlagService.IsEnabled("NewFeature", userId))
    {
        // New feature code
        return Ok("New feature enabled!");
    }
    else
    {
        // Fallback code
        return Ok("Standard feature");
    }
}
```

#### 3. Use the Attribute for Controller Actions

```csharp
[HttpGet("new-endpoint")]
[RequireFeatureFlag("NewFeature")]
public IActionResult NewEndpoint()
{
    return Ok("This endpoint is only accessible when NewFeature is enabled");
}
```

#### 4. A/B Testing

```csharp
var variant = _featureFlagService.GetVariant("ABTestFeature", userId);
switch (variant)
{
    case "control":
        return Ok("Control experience");
    case "variant-a":
        return Ok("Variant A experience");
    case "variant-b":
        return Ok("Variant B experience");
    default:
        return Ok("Default experience");
}
```

### How It Works

1. **Consistent Hashing**: Users are consistently assigned to rollout groups or variants based on a hash of the feature name and user ID
2. **Middleware**: Feature flag context is automatically attached to each request
3. **Development Headers**: In development mode, enabled feature flags are included in `X-Feature-Flags` response header
4. **Attribute-Based Gating**: Use `[RequireFeatureFlag]` attribute to gate entire endpoints
5. **Graceful Degradation**: Disabled features return 404 to maintain API contract

### Best Practices

- **Start Small**: Roll out features gradually (10% → 25% → 50% → 100%)
- **Monitor Metrics**: Track usage and errors for each variant
- **Clean Up**: Remove feature flags once features are fully rolled out
- **Document**: Keep feature flag descriptions up to date
- **Test**: Verify both enabled and disabled states


## Evergreen Development & Automation

This project implements evergreen software development practices to maintain security, quality, and sustainability.

### Automated Dependency Management

- **Dependabot Configuration** ([`.github/dependabot.yml`](.github/dependabot.yml))
  - Automated NuGet package updates (weekly schedule)
  - Security vulnerability prioritization
  - Grouped minor/patch updates for efficiency
  - Chat log: [tech-stack-review-20260211](ai-logs/2026/02/11/tech-stack-review-20260211/conversation.md)

### CI/CD Quality Gates

- **Quality Gates Workflow** ([`.github/workflows/quality-gates.yml`](.github/workflows/quality-gates.yml))
  - Security scanning (TruffleHog secret detection, vulnerability checks)
  - AI provenance validation for AI-generated files
  - Code quality enforcement (formatting, linting, Roslyn analyzers)
  - Test coverage threshold (>80% required)
  - Multi-OS build verification (Ubuntu, Windows)
  - SBOM (Software Bill of Materials) generation
  - Chat log: [tech-stack-review-20260211](ai-logs/2026/02/11/tech-stack-review-20260211/conversation.md)

### Policy & Guidelines

Development practices are governed by instruction files in [`.github/instructions/`](.github/instructions/):

- **AI-Assisted Output Policy**: [`ai-assisted-output.instructions.md`](.github/instructions/ai-assisted-output.instructions.md)
  - Required provenance metadata for all AI-generated content
  - Conversation logging and session summaries
  - README updates for new artifacts

- **Evergreen Development Policy**: [`evergreen-development.instructions.md`](.github/instructions/evergreen-development.instructions.md)
  - Continuous dependency updates
  - Security vulnerability management
  - Technical debt prevention
  - Code quality standards

## AI-Assisted Artifacts

This project includes AI-generated content with full provenance tracking:

### Technology Reviews

- **Technology Stack Review** (2026-02-11)
  - [Conversation Log](ai-logs/2026/02/11/tech-stack-review-20260211/conversation.md)
  - [Session Summary](ai-logs/2026/02/11/tech-stack-review-20260211/summary.md)
  - **Purpose**: Comprehensive technology inventory and instruction file applicability analysis
  - **Deliverables**: Dependabot config, quality gates workflow, compliance assessment
  - **Model**: anthropic/claude-3.5-sonnet@2024-10-22

### Configuration Files

All configuration files created through AI assistance include provenance metadata:
- `.github/dependabot.yml` - Dependency automation
- `.github/workflows/quality-gates.yml` - CI/CD quality enforcement

For full AI conversation history and resumability context, see the linked conversation logs above.