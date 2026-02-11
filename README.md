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