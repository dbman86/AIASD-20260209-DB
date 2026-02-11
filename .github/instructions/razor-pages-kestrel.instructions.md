---
ai_generated: true
model: "anthropic/claude-3.5-sonnet@2024-10-22"
operator: "assistant"
chat_id: "razor-kestrel-instructions-20260210"
prompt: |
  create an instruction containing the standards and practices for these technologies
  Web Framework
  Razor Pages - Page-based programming model for UI
  Kestrel - Cross-platform web server (implied by ASP.NET Core)
started: "2026-02-10T18:00:00Z"
ended: "2026-02-10T18:30:00Z"
task_durations:
  - task: "requirements analysis"
    duration: "00:05:00"
  - task: "content generation"
    duration: "00:20:00"
  - task: "examples and formatting"
    duration: "00:05:00"
total_duration: "00:30:00"
ai_log: "ai-logs/2026/02/10/razor-kestrel-instructions-20260210/conversation.md"
source: "user-request"
applyTo: "**/*.cshtml,**/*.cshtml.cs,**/Program.cs"
---

# Razor Pages and Kestrel Standards and Practices

## Overview

This document establishes standards, best practices, and architectural patterns for developing ASP.NET Core web applications using **Razor Pages** for UI and **Kestrel** as the web server. These guidelines ensure maintainable, performant, secure, and testable web applications.

### Technology Stack

- **Razor Pages**: Page-based programming model for building server-rendered web UI
- **Kestrel**: High-performance, cross-platform web server for ASP.NET Core
- **ASP.NET Core**: Modern, cloud-ready web framework

### Scope

This instruction applies to:

- All Razor Pages applications (`.cshtml` and `.cshtml.cs` files)
- Kestrel configuration in `Program.cs` and `appsettings.json`
- Web application architecture and patterns
- Server-side rendering and page models
- HTTP request/response handling

## Razor Pages Architecture

### File Organization

**Page Structure**:

```
Pages/
├── _ViewImports.cshtml           # Global using directives and tag helpers
├── _ViewStart.cshtml              # Shared layout configuration
├── Index.cshtml                   # View markup
├── Index.cshtml.cs                # PageModel (code-behind)
├── Privacy.cshtml
├── Privacy.cshtml.cs
├── Admin/                         # Feature-based folders
│   ├── Users.cshtml
│   └── Users.cshtml.cs
├── Shared/                        # Shared UI components
│   ├── _Layout.cshtml             # Master layout
│   ├── _ValidationScriptsPartial.cshtml
│   └── Components/                # View Components
│       └── Alert/
│           ├── Default.cshtml
│           └── AlertViewComponent.cs
└── Error.cshtml                   # Error page
```

**Organizational Principles**:

- ✅ Group related pages by feature or area (e.g., `Pages/Admin/`, `Pages/Account/`)
- ✅ Keep page models (`.cshtml.cs`) alongside views (`.cshtml`)
- ✅ Use `Shared/` folder for reusable components and layouts
- ✅ Separate large page models into dedicated service classes
- ❌ Avoid deep nesting (max 3-4 levels)
- ❌ Don't mix business logic in page models

### PageModel Pattern

**Proper PageModel Structure**:

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyApp.Services;
using MyApp.Models;

namespace MyApp.Pages;

public class IndexModel : PageModel
{
    private readonly ICalculatorService _calculatorService;
    private readonly ILogger<IndexModel> _logger;

    // Constructor injection for dependencies
    public IndexModel(
        ICalculatorService calculatorService,
        ILogger<IndexModel> logger)
    {
        _calculatorService = calculatorService;
        _logger = logger;
    }

    // Properties for data binding
    [BindProperty]
    public string Expression { get; set; } = string.Empty;

    public decimal? Result { get; set; }
    public string? ErrorMessage { get; set; }

    // Handler methods
    public void OnGet()
    {
        // Initialize page state
        _logger.LogInformation("Index page loaded");
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            Result = _calculatorService.Evaluate(Expression);
            _logger.LogInformation(
                "Expression evaluated: {Expression} = {Result}",
                Expression, Result);
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error evaluating expression: {Expression}", Expression);
            ErrorMessage = "Invalid expression. Please try again.";
            return Page();
        }
    }
}
```

**PageModel Best Practices**:

1. **Dependency Injection**:
   - ✅ Use constructor injection for all dependencies
   - ✅ Inject services, not concrete implementations
   - ❌ Avoid using `HttpContext.RequestServices` directly

2. **Property Binding**:
   - ✅ Use `[BindProperty]` for form fields
   - ✅ Use `[BindProperty(SupportsGet = true)]` for query parameters
   - ✅ Initialize properties with default values
   - ❌ Don't bind sensitive data without validation

3. **Handler Methods**:
   - ✅ Use `OnGet()` for GET requests (display)
   - ✅ Use `OnPost()` for POST requests (form submission)
   - ✅ Use `OnGetAsync()` / `OnPostAsync()` for async operations
   - ✅ Return `IActionResult` for flexibility (Page(), Redirect(), etc.)
   - ❌ Don't perform long-running operations synchronously

4. **Naming Conventions**:
   - ✅ PageModel class name: `{PageName}Model` (e.g., `IndexModel`)
   - ✅ Handler methods: `On{HttpVerb}[{Action}][Async]` (e.g., `OnPostDeleteAsync`)
   - ✅ Named handlers: `OnPost{ActionName}Async` (e.g., `OnPostCalculateAsync`)

### View (Razor) Best Practices

**Proper Razor View Structure**:

```cshtml
@page
@model MyApp.Pages.IndexModel
@{
    ViewData["Title"] = "Calculator";
}

<div class="container">
    <h1>@ViewData["Title"]</h1>

    @if (!string.IsNullOrEmpty(Model.ErrorMessage))
    {
        <div class="alert alert-danger" role="alert">
            @Model.ErrorMessage
        </div>
    }

    @if (Model.Result.HasValue)
    {
        <div class="alert alert-success" role="alert">
            Result: <strong>@Model.Result.Value</strong>
        </div>
    }

    <form method="post" asp-page-handler="Calculate">
        <div class="form-group mb-3">
            <label asp-for="Expression" class="form-label"></label>
            <input asp-for="Expression" class="form-control" placeholder="e.g., 2 + 2" />
            <span asp-validation-for="Expression" class="text-danger"></span>
        </div>

        <button type="submit" class="btn btn-primary">Calculate</button>
    </form>
</div>

@section Scripts {
    <partial name="_ValidationScriptsPartial" />
}
```

**View Best Practices**:

1. **Page Directive**:
   - ✅ Always start with `@page` directive
   - ✅ Use route templates: `@page "{id:int}"` for parameters
   - ✅ Specify route constraints for type safety

2. **Model Binding**:
   - ✅ Use strongly-typed models with `@model`
   - ✅ Use Tag Helpers (`asp-for`, `asp-validation-for`)
   - ❌ Avoid inline C# logic for business rules
   - ❌ Don't access services directly from views

3. **Tag Helpers**:
   - ✅ Use `asp-page` for navigation: `<a asp-page="/Index">Home</a>`
   - ✅ Use `asp-for` for form binding: `<input asp-for="PropertyName" />`
   - ✅ Use `asp-validation-for` for client-side validation
   - ✅ Use `asp-page-handler` for named handlers

4. **HTML Structure**:
   - ✅ Use semantic HTML5 elements
   - ✅ Include accessibility attributes (ARIA)
   - ✅ Use CSS classes from design system
   - ❌ Avoid inline styles (use CSS classes)

5. **Code Blocks**:
   - ✅ Keep code blocks minimal and for display logic only
   - ✅ Use `@functions` for view-specific helper methods
   - ❌ Don't put business logic in views
   - ❌ Avoid complex LINQ queries in views

### Layout and Partials

**Master Layout (\_Layout.cshtml)**:

```cshtml
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"] - MyApp</title>

    <link rel="stylesheet" href="~/lib/bootstrap/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="~/css/site.css" asp-append-version="true" />
</head>
<body>
    <header>
        <nav class="navbar navbar-expand-sm navbar-light bg-white border-bottom">
            <div class="container">
                <a class="navbar-brand" asp-page="/Index">MyApp</a>
                <button class="navbar-toggler" type="button" data-bs-toggle="collapse"
                        data-bs-target="#navbarNav">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navbarNav">
                    <ul class="navbar-nav">
                        <li class="nav-item">
                            <a class="nav-link" asp-page="/Index">Home</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" asp-page="/Privacy">Privacy</a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
    </header>

    <main role="main" class="pb-3">
        @RenderBody()
    </main>

    <footer class="border-top footer text-muted">
        <div class="container">
            &copy; 2026 - MyApp - <a asp-page="/Privacy">Privacy</a>
        </div>
    </footer>

    <script src="~/lib/jquery/dist/jquery.min.js"></script>
    <script src="~/lib/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
    <script src="~/js/site.js" asp-append-version="true"></script>

    @await RenderSectionAsync("Scripts", required: false)
</body>
</html>
```

**Layout Best Practices**:

- ✅ Use `asp-append-version="true"` for cache busting
- ✅ Define optional sections with `@await RenderSectionAsync("Name", required: false)`
- ✅ Use CDN with fallbacks for external libraries
- ✅ Include meta tags for SEO and responsiveness
- ❌ Don't embed page-specific content in layouts

**Partial Views**:

```cshtml
@* _AlertPartial.cshtml *@
@model string

@if (!string.IsNullOrEmpty(Model))
{
    <div class="alert alert-info alert-dismissible fade show" role="alert">
        @Model
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    </div>
}
```

Usage:

```cshtml
<partial name="_AlertPartial" model="@Model.Message" />
```

### View Components

**Creating View Components**:

```csharp
// ViewComponents/AlertViewComponent.cs
using Microsoft.AspNetCore.Mvc;

namespace MyApp.ViewComponents;

public class AlertViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(string message, string type = "info")
    {
        var model = new AlertViewModel
        {
            Message = message,
            Type = type
        };
        return View(model);
    }
}

public class AlertViewModel
{
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = "info";
}
```

**View Component View**:

```cshtml
@* Views/Shared/Components/Alert/Default.cshtml *@
@model MyApp.ViewComponents.AlertViewModel

<div class="alert alert-@Model.Type" role="alert">
    @Model.Message
</div>
```

**Usage**:

```cshtml
@await Component.InvokeAsync("Alert", new { message = "Success!", type = "success" })
```

**When to Use View Components**:

- ✅ Complex rendering logic needed
- ✅ Reusable UI components with dependencies
- ✅ Components requiring data access
- ❌ Simple markup (use partials instead)

## Routing and Navigation

### Route Configuration

**Page Routes**:

```cshtml
@* Default route: /Index *@
@page

@* Custom route: /About *@
@page "/About"

@* Route with parameter: /Product/123 *@
@page "{id:int}"

@* Optional parameter: /Product or /Product/123 *@
@page "{id:int?}"

@* Multiple parameters: /Category/Electronics/Product/123 *@
@page "{category}/{id:int}"

@* Catch-all route: /Blog/2026/02/10/post-title *@
@page "{*slug}"
```

**Route Constraints**:

| Constraint       | Example                      | Description          |
| ---------------- | ---------------------------- | -------------------- |
| `int`            | `{id:int}`                   | Integer only         |
| `bool`           | `{active:bool}`              | Boolean (true/false) |
| `datetime`       | `{date:datetime}`            | DateTime value       |
| `decimal`        | `{price:decimal}`            | Decimal number       |
| `double`         | `{rating:double}`            | Double value         |
| `long`           | `{id:long}`                  | Long integer         |
| `guid`           | `{id:guid}`                  | GUID format          |
| `minlength(n)`   | `{code:minlength(5)}`        | Minimum length       |
| `maxlength(n)`   | `{code:maxlength(10)}`       | Maximum length       |
| `length(n)`      | `{code:length(8)}`           | Exact length         |
| `min(n)`         | `{age:min(18)}`              | Minimum value        |
| `max(n)`         | `{age:max(120)}`             | Maximum value        |
| `range(min,max)` | `{age:range(18,120)}`        | Value range          |
| `alpha`          | `{name:alpha}`               | Alphabetic only      |
| `regex(expr)`    | `{code:regex(^[A-Z]{{3}}$)}` | Regular expression   |

**Navigation Best Practices**:

```cshtml
@* Absolute page path *@
<a asp-page="/Admin/Users">Manage Users</a>

@* Relative to current folder *@
<a asp-page="./Details" asp-route-id="@Model.Id">Details</a>

@* With route parameters *@
<a asp-page="/Product/Details" asp-route-id="123">View Product</a>

@* With query string *@
<a asp-page="/Search" asp-route-query="razor pages" asp-route-page="2">Search</a>

@* Generates: /Search?query=razor+pages&page=2 *@
```

**Redirects in PageModel**:

```csharp
// Redirect to page
return RedirectToPage("/Index");

// Redirect with route data
return RedirectToPage("/Product/Details", new { id = 123 });

// Redirect to external URL
return Redirect("https://example.com");

// Return current page with updated state
return Page();

// Return not found
return NotFound();

// Return unauthorized
return Unauthorized();
```

## Data Validation

### Model Validation

**Validation Attributes**:

```csharp
using System.ComponentModel.DataAnnotations;

public class ContactModel : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be 2-100 characters")]
    public string Name { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    [Required]
    [Phone(ErrorMessage = "Invalid phone number")]
    public string Phone { get; set; } = string.Empty;

    [BindProperty]
    [Range(18, 120, ErrorMessage = "Age must be between 18 and 120")]
    public int Age { get; set; }

    [BindProperty]
    [Url(ErrorMessage = "Invalid URL format")]
    public string? Website { get; set; }

    [BindProperty]
    [RegularExpression(@"^[A-Z]{3}\d{4}$", ErrorMessage = "Invalid code format (e.g., ABC1234)")]
    public string? Code { get; set; }

    [BindProperty]
    [Compare("Email", ErrorMessage = "Emails do not match")]
    public string ConfirmEmail { get; set; } = string.Empty;

    [BindProperty]
    [CreditCard(ErrorMessage = "Invalid credit card number")]
    public string? CreditCard { get; set; }
}
```

**Server-Side Validation**:

```csharp
public IActionResult OnPost()
{
    // Check ModelState
    if (!ModelState.IsValid)
    {
        // Return page with validation errors
        return Page();
    }

    // Custom validation
    if (!_service.IsValidEmail(Email))
    {
        ModelState.AddModelError(nameof(Email), "Email domain not allowed");
        return Page();
    }

    // Process valid data
    _service.SaveContact(Name, Email);
    return RedirectToPage("/Success");
}
```

**Client-Side Validation**:

```cshtml
@* Include validation scripts *@
@section Scripts {
    <partial name="_ValidationScriptsPartial" />
}

@* Display validation summary *@
<div asp-validation-summary="All" class="text-danger"></div>

@* Display field-specific errors *@
<input asp-for="Email" class="form-control" />
<span asp-validation-for="Email" class="text-danger"></span>
```

**Custom Validation Attributes**:

```csharp
public class FutureDateAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext context)
    {
        if (value is DateTime date)
        {
            if (date > DateTime.Now)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Date must be in the future");
        }
        return new ValidationResult("Invalid date");
    }
}

// Usage
[BindProperty]
[FutureDate(ErrorMessage = "Appointment must be in the future")]
public DateTime AppointmentDate { get; set; }
```

## Security Best Practices

### Anti-Forgery Protection

**Automatic Protection**:

- Razor Pages automatically include anti-forgery tokens in forms
- POST, PUT, DELETE requests are protected by default
- `[ValidateAntiForgeryToken]` is implicit for non-GET handlers

**Explicit Configuration**:

```csharp
// Require anti-forgery for specific handler
[IgnoreAntiforgeryToken]  // Disable (use cautiously)
public IActionResult OnPostPublicApi()
{
    // API endpoint without anti-forgery check
}

// Configure globally in Program.cs
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
    options.Cookie.Name = "X-CSRF-TOKEN";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});
```

**Manual Token Generation (for AJAX)**:

```csharp
// Inject IAntiforgery in page model
private readonly IAntiforgery _antiforgery;

[IgnoreAntiforgeryToken]
public IActionResult OnGet()
{
    var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
    ViewData["RequestVerificationToken"] = tokens.RequestToken;
    return Page();
}
```

```cshtml
@* Include token in AJAX request *@
<script>
    const token = '@ViewData["RequestVerificationToken"]';
    fetch('/api/data', {
        method: 'POST',
        headers: {
            'X-CSRF-TOKEN': token,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    });
</script>
```

### Input Sanitization

**HTML Encoding**:

```cshtml
@* Automatic encoding (safe) *@
<p>@Model.UserInput</p>

@* Raw HTML (dangerous - avoid unless necessary) *@
<div>@Html.Raw(Model.TrustedHtml)</div>

@* Use sanitization library for user content *@
@inject HtmlSanitizer Sanitizer
<div>@Html.Raw(Sanitizer.Sanitize(Model.UserHtml))</div>
```

**SQL Injection Prevention**:

```csharp
// ✅ Good: Parameterized queries (Entity Framework)
var users = await _context.Users
    .Where(u => u.Email == email)
    .ToListAsync();

// ✅ Good: Stored procedures with parameters
var result = await _context.Database
    .ExecuteSqlInterpolatedAsync($"EXEC GetUser {email}");

// ❌ Bad: String concatenation (SQL injection risk)
var query = $"SELECT * FROM Users WHERE Email = '{email}'";
```

**XSS Prevention**:

```csharp
// ✅ Good: Use ViewData/Model properties (auto-encoded)
ViewData["Message"] = userInput;

// ✅ Good: Sanitize HTML content
var sanitizer = new HtmlSanitizer();
var cleanHtml = sanitizer.Sanitize(userHtml);

// ❌ Bad: Raw output in JavaScript
<script>
    var message = '@Model.UserInput'; // XSS risk
</script>

// ✅ Good: JSON encode for JavaScript
<script>
    var message = @Html.Raw(Json.Serialize(Model.UserInput));
</script>
```

### Authentication and Authorization

**Configure Authentication**:

```csharp
// Program.cs
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
    options.AddPolicy("EditContent", policy =>
        policy.RequireClaim("Permission", "Edit"));
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
```

**Page-Level Authorization**:

```csharp
using Microsoft.AspNetCore.Authorization;

// Require authentication
[Authorize]
public class AdminModel : PageModel
{
    public void OnGet() { }
}

// Require specific role
[Authorize(Roles = "Administrator,Manager")]
public class ManageUsersModel : PageModel { }

// Require policy
[Authorize(Policy = "AdminOnly")]
public class SettingsModel : PageModel { }

// Allow anonymous access
[AllowAnonymous]
public class PublicModel : PageModel { }
```

**Convention-Based Authorization**:

```csharp
// Program.cs
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizePage("/Admin");
    options.Conventions.AuthorizeFolder("/Admin");
    options.Conventions.AllowAnonymousToPage("/Public");
    options.Conventions.AllowAnonymousToFolder("/Public");
});
```

### HTTPS and Security Headers

**Enforce HTTPS**:

```csharp
// Program.cs
builder.Services.AddHttpsRedirection(options =>
{
    options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
    options.HttpsPort = 443;
});

builder.Services.AddHsts(options =>
{
    options.Preload = true;
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(365);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
```

**Security Headers Configuration**:

```csharp
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
    context.Response.Headers.Add("Content-Security-Policy",
        "default-src 'self'; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline'");

    await next();
});
```

## Kestrel Web Server Configuration

### Basic Kestrel Setup

**Program.cs Configuration**:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel
builder.WebHost.ConfigureKestrel(options =>
{
    // Listen on specific port
    options.ListenAnyIP(5000);

    // Listen on HTTPS
    options.ListenAnyIP(5001, listenOptions =>
    {
        listenOptions.UseHttps();
    });

    // Configure limits
    options.Limits.MaxConcurrentConnections = 100;
    options.Limits.MaxConcurrentUpgradedConnections = 100;
    options.Limits.MaxRequestBodySize = 10 * 1024 * 1024; // 10 MB
    options.Limits.MinRequestBodyDataRate = new MinDataRate(
        bytesPerSecond: 100,
        gracePeriod: TimeSpan.FromSeconds(10));
    options.Limits.MinResponseDataRate = new MinDataRate(
        bytesPerSecond: 100,
        gracePeriod: TimeSpan.FromSeconds(10));
    options.Limits.RequestHeadersTimeout = TimeSpan.FromSeconds(30);
    options.Limits.KeepAliveTimeout = TimeSpan.FromSeconds(120);
});

var app = builder.Build();
```

### Production Configuration

**appsettings.Production.json**:

```json
{
  "Kestrel": {
    "Endpoints": {
      "Http": {
        "Url": "http://0.0.0.0:80"
      },
      "Https": {
        "Url": "https://0.0.0.0:443",
        "Certificate": {
          "Path": "/app/cert.pfx",
          "Password": "${CERT_PASSWORD}"
        }
      }
    },
    "Limits": {
      "MaxConcurrentConnections": 1000,
      "MaxConcurrentUpgradedConnections": 100,
      "MaxRequestBodySize": 30000000,
      "KeepAliveTimeout": "00:02:00",
      "RequestHeadersTimeout": "00:00:30"
    }
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### HTTPS and Certificates

**Configure HTTPS**:

```csharp
builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Any, 443, listenOptions =>
    {
        // Use certificate from file
        listenOptions.UseHttps("certificate.pfx", "password");

        // Or from certificate store
        listenOptions.UseHttps(storeCert =>
        {
            storeCert.ServerCertificate = LoadCertificate();
        });

        // Configure TLS
        listenOptions.UseHttps(httpsOptions =>
        {
            httpsOptions.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13;
        });
    });
});

X509Certificate2 LoadCertificate()
{
    using var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
    store.Open(OpenFlags.ReadOnly);
    var cert = store.Certificates
        .Find(X509FindType.FindByThumbprint, "thumbprint", validOnly: true)
        .FirstOrDefault();
    return cert ?? throw new InvalidOperationException("Certificate not found");
}
```

**Developer Certificate (Development)**:

```bash
# Generate development certificate
dotnet dev-certs https --trust

# Export certificate
dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p password

# Use in Docker
docker run -p 5001:443 -e ASPNETCORE_Kestrel__Certificates__Default__Password="password" \
  -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx \
  -v ${HOME}/.aspnet/https:/https/ myapp
```

### Request Limits and Timeouts

**Configure Request Limits**:

```csharp
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 52428800; // 50 MB
    options.Limits.MaxRequestBufferSize = 1048576; // 1 MB
    options.Limits.MaxRequestHeaderCount = 100;
    options.Limits.MaxRequestHeadersTotalSize = 32768; // 32 KB
    options.Limits.MaxRequestLineSize = 8192; // 8 KB

    // Timeouts
    options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
    options.Limits.RequestHeadersTimeout = TimeSpan.FromSeconds(30);

    // Rate limiting
    options.Limits.MaxConcurrentConnections = 100;
    options.Limits.MaxConcurrentUpgradedConnections = 100;

    // HTTP/2 limits
    options.Limits.Http2.MaxStreamsPerConnection = 100;
    options.Limits.Http2.HeaderTableSize = 4096;
    options.Limits.Http2.MaxFrameSize = 16384;
    options.Limits.Http2.MaxRequestHeaderFieldSize = 8192;
});
```

**Per-Request Limits**:

```csharp
// Disable request size limit for specific endpoint
[RequestSizeLimit(100_000_000)] // 100 MB
public class UploadModel : PageModel
{
    public async Task<IActionResult> OnPostAsync(IFormFile file)
    {
        // Handle large file upload
    }
}

// Disable entirely
[DisableRequestSizeLimit]
public class StreamingModel : PageModel { }
```

### HTTP/2 and HTTP/3 Configuration

**Enable HTTP/2**:

```csharp
builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Any, 443, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
        listenOptions.UseHttps();
    });
});
```

**Enable HTTP/3 (QUIC)**:

```json
{
  "Kestrel": {
    "Endpoints": {
      "Https": {
        "Url": "https://0.0.0.0:443",
        "Protocols": "Http1AndHttp2AndHttp3"
      }
    }
  }
}
```

```csharp
builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Any, 443, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
        listenOptions.UseHttps();
    });
});
```

### Logging and Diagnostics

**Configure Kestrel Logging**:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.AspNetCore.Server.Kestrel": "Information"
    }
  }
}
```

**Request Logging Middleware**:

```csharp
app.UseHttpLogging();

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("X-Request-ID");
    logging.ResponseHeaders.Add("X-Response-Time");
    logging.MediaTypeOptions.AddText("application/json");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});
```

### Reverse Proxy Configuration

**Behind Nginx/Apache**:

```csharp
// Program.cs - Configure forwarded headers
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto |
        ForwardedHeaders.XForwardedHost;

    options.KnownProxies.Add(IPAddress.Parse("10.0.0.1"));
    options.KnownNetworks.Add(new IPNetwork(IPAddress.Parse("10.0.0.0"), 8));
});

var app = builder.Build();

app.UseForwardedHeaders();
```

**Nginx Configuration**:

```nginx
server {
    listen 80;
    server_name example.com;

    location / {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
        proxy_cache_bypass $http_upgrade;
    }
}
```

### Load Balancing Considerations

**Health Checks**:

```csharp
// Program.cs
builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy())
    .AddDbContextCheck<AppDbContext>();

var app = builder.Build();

app.MapHealthChecks("/health");
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false
});
```

**Sticky Sessions (When Required)**:

```csharp
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".MyApp.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

var app = builder.Build();
app.UseSession();
```

## Performance Optimization

### Response Caching

**Output Caching**:

```csharp
// Program.cs
builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(builder =>
        builder.Expire(TimeSpan.FromMinutes(10)));

    options.AddPolicy("CachePublic", builder =>
        builder.Expire(TimeSpan.FromHours(1))
               .SetVaryByQuery("page", "sort"));
});

var app = builder.Build();
app.UseOutputCache();
```

```csharp
// PageModel
[OutputCache(Duration = 3600, VaryByQueryKeys = new[] { "id" })]
public class ProductModel : PageModel
{
    public void OnGet(int id) { }
}
```

**Response Caching Middleware**:

```csharp
builder.Services.AddResponseCaching();

var app = builder.Build();
app.UseResponseCaching();
```

```csharp
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new[] { "id" })]
public class CachedPageModel : PageModel { }
```

### Response Compression

**Enable Compression**:

```csharp
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/json", "text/plain" });
});

builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.SmallestSize;
});

var app = builder.Build();
app.UseResponseCompression();
```

### Static File Optimization

**Static File Configuration**:

```csharp
var app = builder.Build();

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        // Cache static files for 1 year
        ctx.Context.Response.Headers.Append(
            "Cache-Control", "public,max-age=31536000");
    }
});

// Serve files from additional location
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "StaticFiles")),
    RequestPath = "/files"
});
```

**Cache Busting with Tag Helpers**:

```cshtml
<link rel="stylesheet" href="~/css/site.css" asp-append-version="true" />
<script src="~/js/site.js" asp-append-version="true"></script>

@* Generates: /css/site.css?v=hash *@
```

### Database Query Optimization

**Efficient Data Loading**:

```csharp
public class ProductsModel : PageModel
{
    private readonly AppDbContext _context;

    public ProductsModel(AppDbContext context)
    {
        _context = context;
    }

    public List<Product> Products { get; set; } = new();

    public async Task OnGetAsync()
    {
        // ✅ Good: Async, projection, filtering
        Products = await _context.Products
            .Where(p => p.IsActive)
            .Select(p => new Product
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price
            })
            .AsNoTracking()
            .ToListAsync();

        // ❌ Bad: Synchronous, loads all columns, change tracking
        // Products = _context.Products.ToList();
    }
}
```

### Asynchronous Operations

**Async Handler Methods**:

```csharp
public class AsyncPageModel : PageModel
{
    private readonly IDataService _dataService;
    private readonly ILogger<AsyncPageModel> _logger;

    public AsyncPageModel(IDataService dataService, ILogger<AsyncPageModel> logger)
    {
        _dataService = dataService;
        _logger = logger;
    }

    public List<Item> Items { get; set; } = new();

    // ✅ Good: Async for I/O operations
    public async Task OnGetAsync()
    {
        Items = await _dataService.GetItemsAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            await _dataService.SaveAsync();
            return RedirectToPage("/Success");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving data");
            ModelState.AddModelError("", "An error occurred");
            return Page();
        }
    }
}
```

## Testing Strategies

### Unit Testing Page Models

**Basic Page Model Test**:

```csharp
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;

public class IndexModelTests
{
    [Fact]
    public async Task OnPostAsync_ValidInput_ReturnsPageResult()
    {
        // Arrange
        var mockService = new Mock<ICalculatorService>();
        mockService.Setup(s => s.EvaluateAsync("2+2"))
            .ReturnsAsync(4m);

        var pageModel = new IndexModel(mockService.Object)
        {
            Expression = "2+2"
        };

        // Act
        var result = await pageModel.OnPostAsync();

        // Assert
        Assert.IsType<PageResult>(result);
        Assert.Equal(4m, pageModel.Result);
        mockService.Verify(s => s.EvaluateAsync("2+2"), Times.Once);
    }

    [Fact]
    public async Task OnPostAsync_InvalidInput_AddsModelError()
    {
        // Arrange
        var mockService = new Mock<ICalculatorService>();
        mockService.Setup(s => s.EvaluateAsync(It.IsAny<string>()))
            .ThrowsAsync(new ArgumentException("Invalid expression"));

        var pageModel = new IndexModel(mockService.Object)
        {
            Expression = "invalid"
        };

        // Act
        var result = await pageModel.OnPostAsync();

        // Assert
        Assert.IsType<PageResult>(result);
        Assert.NotNull(pageModel.ErrorMessage);
    }
}
```

### Integration Testing

**WebApplicationFactory Testing**:

```csharp
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public IntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/Index")]
    [InlineData("/Privacy")]
    public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal("text/html; charset=utf-8",
            response.Content.Headers.ContentType?.ToString());
    }

    [Fact]
    public async Task Post_CalculateExpression_ReturnsCorrectResult()
    {
        // Arrange
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("Expression", "2+2")
        });

        // Act
        var response = await client.PostAsync("/", content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var html = await response.Content.ReadAsStringAsync();
        Assert.Contains("4", html);
    }
}
```

**Custom WebApplicationFactory**:

```csharp
public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove real database context
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Add in-memory database
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });

            // Build service provider and seed data
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();
            SeedTestData(db);
        });
    }

    private static void SeedTestData(AppDbContext db)
    {
        db.Products.AddRange(
            new Product { Id = 1, Name = "Test Product 1", Price = 10.00m },
            new Product { Id = 2, Name = "Test Product 2", Price = 20.00m }
        );
        db.SaveChanges();
    }
}
```

### End-to-End Testing

**Playwright/Selenium Tests**:

```csharp
using Microsoft.Playwright;
using Xunit;

public class E2ETests : IAsyncLifetime
{
    private IPlaywright _playwright = null!;
    private IBrowser _browser = null!;
    private IBrowserContext _context = null!;
    private IPage _page = null!;

    public async Task InitializeAsync()
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync();
        _context = await _browser.NewContextAsync();
        _page = await _context.NewPageAsync();
    }

    public async Task DisposeAsync()
    {
        await _page.CloseAsync();
        await _context.CloseAsync();
        await _browser.CloseAsync();
        _playwright.Dispose();
    }

    [Fact]
    public async Task Calculator_EnterExpression_ShowsResult()
    {
        // Navigate to page
        await _page.GotoAsync("https://localhost:5001");

        // Enter expression
        await _page.FillAsync("input[name='Expression']", "2+2");

        // Submit form
        await _page.ClickAsync("button[type='submit']");

        // Wait for result
        await _page.WaitForSelectorAsync(".alert-success");

        // Verify result
        var resultText = await _page.TextContentAsync(".alert-success");
        Assert.Contains("4", resultText);
    }
}
```

## Error Handling and Logging

### Global Error Handling

**Configure Error Pages**:

```csharp
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseStatusCodePagesWithReExecute("/Error/{0}");
    app.UseHsts();
}
```

**Error Page Model**:

```csharp
[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel : PageModel
{
    public string? RequestId { get; set; }
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    public string? ErrorMessage { get; set; }
    public int StatusCode { get; set; }

    private readonly ILogger<ErrorModel> _logger;

    public ErrorModel(ILogger<ErrorModel> logger)
    {
        _logger = logger;
    }

    public void OnGet(int? statusCode = null)
    {
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        StatusCode = statusCode ?? Response.StatusCode;

        ErrorMessage = StatusCode switch
        {
            404 => "Page not found",
            403 => "Access denied",
            500 => "Internal server error",
            _ => "An error occurred"
        };

        _logger.LogWarning("Error page displayed. Status: {StatusCode}, RequestId: {RequestId}",
            StatusCode, RequestId);
    }
}
```

### Structured Logging

**Configure Logging**:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "Serilog": {
    "Using": ["Serilog.Sinks.Console", "Serilog.Sinks.File"],
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    },
    "WriteTo": [
      { "Name": "Console" },
      {
        "Name": "File",
        "Args": {
          "path": "logs/log-.txt",
          "rollingInterval": "Day",
          "outputTemplate": "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
        }
      }
    ]
  }
}
```

```csharp
// Program.cs with Serilog
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

var app = builder.Build();
app.UseSerilogRequestLogging();
```

**Logging Best Practices**:

```csharp
public class CalculatorModel : PageModel
{
    private readonly ILogger<CalculatorModel> _logger;

    public CalculatorModel(ILogger<CalculatorModel> logger)
    {
        _logger = logger;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        // ✅ Good: Structured logging with parameters
        _logger.LogInformation(
            "User {UserId} evaluating expression: {Expression}",
            User.Identity?.Name, Expression);

        try
        {
            var result = await _service.EvaluateAsync(Expression);

            _logger.LogInformation(
                "Expression evaluated successfully: {Expression} = {Result}",
                Expression, result);

            return Page();
        }
        catch (Exception ex)
        {
            // ✅ Good: Log exception with context
            _logger.LogError(ex,
                "Error evaluating expression: {Expression} for user {UserId}",
                Expression, User.Identity?.Name);

            ModelState.AddModelError("", "An error occurred");
            return Page();
        }

        // ❌ Bad: String interpolation in logging
        // _logger.LogInformation($"User {User.Identity?.Name} evaluated {Expression}");
    }
}
```

## Common Patterns and Anti-Patterns

### Patterns

**Repository Pattern with Dependency Injection**:

```csharp
// Interface
public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<Product> AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int id);
}

// Implementation
public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _context.Products
            .AsNoTracking()
            .ToListAsync();
    }

    // Other methods...
}

// Registration
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Usage in PageModel
public class ProductsModel : PageModel
{
    private readonly IProductRepository _repository;

    public ProductsModel(IProductRepository repository)
    {
        _repository = repository;
    }

    public List<Product> Products { get; set; } = new();

    public async Task OnGetAsync()
    {
        Products = await _repository.GetAllAsync();
    }
}
```

**PRG (Post-Redirect-Get) Pattern**:

```csharp
public class CreateProductModel : PageModel
{
    [BindProperty]
    public Product Product { get; set; } = new();

    [TempData]
    public string StatusMessage { get; set; } = string.Empty;

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await _repository.AddAsync(Product);

        // Set success message
        StatusMessage = "Product created successfully!";

        // Redirect to prevent duplicate submission
        return RedirectToPage("/Products/Index");
    }
}
```

**View Model Pattern**:

```csharp
// View Model
public class ProductViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string CategoryName { get; set; } = string.Empty;
}

// PageModel
public class ProductModel : PageModel
{
    public ProductViewModel Product { get; set; } = new();

    public async Task OnGetAsync(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product != null)
        {
            Product = new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                CategoryName = product.Category.Name
            };
        }
    }
}
```

### Anti-Patterns to Avoid

**❌ Business Logic in Views**:

```cshtml
@* Bad: Business logic in view *@
@{
    var discount = Model.Price > 100 ? Model.Price * 0.1m : 0;
    var finalPrice = Model.Price - discount;
}
<p>Price: @finalPrice</p>

@* Good: Logic in PageModel or service *@
<p>Price: @Model.FinalPrice</p>
```

**❌ Accessing DbContext Directly from View**:

```cshtml
@* Bad: Database query in view *@
@inject AppDbContext DbContext
<ul>
    @foreach (var item in DbContext.Products.ToList())
    {
        <li>@item.Name</li>
    }
</ul>

@* Good: Load data in PageModel *@
<ul>
    @foreach (var item in Model.Products)
    {
        <li>@item.Name</li>
    }
</ul>
```

**❌ Synchronous I/O Operations**:

```csharp
// Bad: Synchronous database call
public IActionResult OnPost()
{
    var result = _context.Products.ToList(); // Blocks thread
    return Page();
}

// Good: Async/await
public async Task<IActionResult> OnPostAsync()
{
    var result = await _context.Products.ToListAsync();
    return Page();
}
```

**❌ Not Using Dependency Injection**:

```csharp
// Bad: Direct instantiation
public class BadPageModel : PageModel
{
    public void OnGet()
    {
        var service = new MyService(); // Hard-coded dependency
        var data = service.GetData();
    }
}

// Good: Constructor injection
public class GoodPageModel : PageModel
{
    private readonly IMyService _service;

    public GoodPageModel(IMyService service)
    {
        _service = service;
    }

    public void OnGet()
    {
        var data = _service.GetData();
    }
}
```

**❌ Ignoring ModelState Validation**:

```csharp
// Bad: Not checking ModelState
public IActionResult OnPost()
{
    _service.Save(Model.Data); // May have invalid data
    return RedirectToPage("/Success");
}

// Good: Validate first
public IActionResult OnPost()
{
    if (!ModelState.IsValid)
    {
        return Page();
    }

    _service.Save(Model.Data);
    return RedirectToPage("/Success");
}
```

## Deployment Considerations

### Publishing Configuration

**Publish Profile**:

```xml
<!-- Properties/PublishProfiles/Production.pubxml -->
<Project>
  <PropertyGroup>
    <WebPublishMethod>FileSystem</WebPublishMethod>
    <PublishProvider>FileSystem</PublishProvider>
    <LastUsedBuildConfiguration>Release</LastUsedBuildConfiguration>
    <LastUsedPlatform>Any CPU</LastUsedPlatform>
    <PublishUrl>bin\Release\net9.0\publish</PublishUrl>
    <DeleteExistingFiles>true</DeleteExistingFiles>
    <TargetFramework>net9.0</TargetFramework>
    <RuntimeIdentifier>linux-x64</RuntimeIdentifier>
    <SelfContained>false</SelfContained>
  </PropertyGroup>
</Project>
```

**Publish Commands**:

```bash
# Framework-dependent deployment
dotnet publish -c Release

# Self-contained deployment (Windows)
dotnet publish -c Release -r win-x64 --self-contained true

# Self-contained deployment (Linux)
dotnet publish -c Release -r linux-x64 --self-contained true

# Single file deployment
dotnet publish -c Release -r linux-x64 -p:PublishSingleFile=true
```

### Docker Deployment

**Dockerfile**:

```dockerfile
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY ["web-calculator.csproj", "./"]
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet build -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Create non-root user
RUN adduser --disabled-password --gecos '' appuser && \
    chown -R appuser /app
USER appuser

COPY --from=publish /app/publish .

# Configure Kestrel
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080

ENTRYPOINT ["dotnet", "web-calculator.dll"]
```

**Docker Compose**:

```yaml
version: "3.8"

services:
  web:
    build: .
    ports:
      - "8080:8080"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__DefaultConnection=Server=db;Database=myapp;User=sa;Password=YourPassword123
    depends_on:
      - db

  db:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=YourPassword123
    ports:
      - "1433:1433"
    volumes:
      - sqldata:/var/opt/mssql

volumes:
  sqldata:
```

### Environment Configuration

**appsettings per Environment**:

```
appsettings.json                 # Base configuration
appsettings.Development.json     # Development overrides
appsettings.Staging.json         # Staging overrides
appsettings.Production.json      # Production overrides
```

**Loading Configuration**:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Automatically loads based on ASPNETCORE_ENVIRONMENT
// Priority: appsettings.{Environment}.json > appsettings.json

// Access configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var setting = builder.Configuration["Kestrel:Endpoints:Http:Url"];

// Bind to strongly-typed options
builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection("AppSettings"));
```

### Monitoring and Health Checks

**Health Check Endpoints**:

```csharp
builder.Services.AddHealthChecks()
    .AddDbContextCheck<AppDbContext>()
    .AddUrlGroup(new Uri("https://api.example.com/health"), "External API")
    .AddCheck("memory", () =>
    {
        var allocated = GC.GetTotalMemory(forceFullCollection: false);
        var threshold = 1024L * 1024L * 1024L; // 1 GB

        return allocated < threshold
            ? HealthCheckResult.Healthy()
            : HealthCheckResult.Degraded($"Memory usage: {allocated / 1024 / 1024} MB");
    });

var app = builder.Build();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description,
                duration = e.Value.Duration
            })
        });
        await context.Response.WriteAsync(result);
    }
});
```

## Conclusion

This instruction document provides comprehensive standards and best practices for developing ASP.NET Core applications with Razor Pages and Kestrel. Following these guidelines ensures:

- **Maintainable Code**: Clear separation of concerns and consistent patterns
- **Security**: Built-in protections and secure configurations
- **Performance**: Optimized for production workloads
- **Testability**: Easy to unit test and integration test
- **Scalability**: Ready for load balancing and horizontal scaling

### Quick Reference Checklist

**For Every Page**:

- [ ] Use `@page` directive with appropriate route template
- [ ] Implement PageModel with constructor injection
- [ ] Use `[BindProperty]` for form fields
- [ ] Return `IActionResult` from handler methods
- [ ] Validate ModelState before processing
- [ ] Use async/await for I/O operations
- [ ] Log important operations and errors
- [ ] Apply appropriate authorization attributes
- [ ] Include anti-forgery token protection
- [ ] Use Tag Helpers for forms and navigation

**For Production**:

- [ ] Configure HTTPS and certificates
- [ ] Enable response compression
- [ ] Configure static file caching
- [ ] Set up health check endpoints
- [ ] Configure appropriate request limits
- [ ] Enable structured logging
- [ ] Set up error pages and logging
- [ ] Configure security headers
- [ ] Test under load
- [ ] Monitor performance metrics

**For Security**:

- [ ] Enforce HTTPS
- [ ] Enable anti-forgery tokens
- [ ] Sanitize user input
- [ ] Use parameterized queries
- [ ] Implement authentication/authorization
- [ ] Configure CORS appropriately
- [ ] Set security headers
- [ ] Don't expose sensitive data in logs
- [ ] Keep dependencies updated
- [ ] Review security regularly

---

**Document Version**: 1.0.0
**Last Updated**: 2026-02-10
**Next Review**: 2026-05-10 (Quarterly)
**Related Standards**:

- [AI-Assisted Output Instructions](.github/instructions/ai-assisted-output.instructions.md)
- [Evergreen Development Instructions](.github/instructions/evergreen-development.instructions.md)
