---
ai_generated: true
model: "anthropic/claude-3.5-sonnet@2024-10-22"
operator: "johnmillerATcodemag-com"
chat_id: "arch-patterns-di-service-20260210"
prompt: |
  create an instruction containing the standards and practices for these technologies

  Architecture Patterns

  Dependency Injection - Built-in DI container (AddScoped, AddRazorPages)
  Service Layer Pattern - Separation of concerns with ICalculatorService interface
started: "2026-02-10T12:00:00Z"
ended: "2026-02-10T12:45:00Z"
task_durations:
  - task: "requirements analysis"
    duration: "00:05:00"
  - task: "content generation"
    duration: "00:35:00"
  - task: "examples and validation"
    duration: "00:05:00"
total_duration: "00:45:00"
ai_log: "ai-logs/2026/02/10/arch-patterns-di-service-20260210/conversation.md"
source: "johnmillerATcodemag-com"
applyTo: "**/*.cs"
---

# Architecture Patterns: Dependency Injection and Service Layer

## Overview

This document defines the standards and best practices for implementing Dependency Injection (DI) and Service Layer patterns in .NET applications. These architectural patterns are fundamental to building maintainable, testable, and loosely-coupled applications.

### Purpose

- Establish consistent patterns for dependency management
- Define service layer architecture standards
- Ensure testability and maintainability
- Promote separation of concerns
- Enable scalable application design

### Scope

These standards apply to:

- ASP.NET Core web applications
- Razor Pages applications
- API controllers and services
- Business logic layer
- Infrastructure and data access layers

## Table of Contents

- [Dependency Injection](#dependency-injection)
  - [Overview](#di-overview)
  - [Service Lifetimes](#service-lifetimes)
  - [Registration Patterns](#registration-patterns)
  - [Constructor Injection](#constructor-injection)
  - [Best Practices](#di-best-practices)
  - [Anti-Patterns](#di-anti-patterns)
- [Service Layer Pattern](#service-layer-pattern)
  - [Overview](#service-layer-overview)
  - [Interface Design](#interface-design)
  - [Implementation Guidelines](#implementation-guidelines)
  - [Best Practices](#service-layer-best-practices)
  - [Anti-Patterns](#service-layer-anti-patterns)
- [Integration Patterns](#integration-patterns)
- [Testing Strategies](#testing-strategies)
- [Code Examples](#code-examples)
- [Migration Strategies](#migration-strategies)

---

## Dependency Injection

### DI Overview

**Definition**: Dependency Injection is a design pattern that implements Inversion of Control (IoC) for resolving dependencies. Instead of classes creating their dependencies, they receive them from an external source.

**Benefits**:

- **Testability**: Easy to mock dependencies in unit tests
- **Maintainability**: Loose coupling between components
- **Flexibility**: Easy to swap implementations
- **Single Responsibility**: Classes focus on their purpose, not dependency creation
- **Lifetime Management**: Automatic disposal of resources

**.NET Built-in DI Container**:

ASP.NET Core includes a built-in dependency injection container that handles service registration, resolution, and lifetime management without requiring third-party libraries.

### Service Lifetimes

The .NET DI container supports three service lifetimes:

#### 1. Transient (`AddTransient`)

**Behavior**: A new instance is created every time the service is requested.

**Use Cases**:

- Lightweight, stateless services
- Services with no shared state
- Short-lived operations
- Services that are inexpensive to create

**Example**:

```csharp
builder.Services.AddTransient<IEmailService, EmailService>();
```

**Characteristics**:

- ✅ Thread-safe (each request gets its own instance)
- ✅ No state management concerns
- ⚠️ Higher memory usage with frequent requests
- ⚠️ Not suitable for services with expensive initialization

#### 2. Scoped (`AddScoped`)

**Behavior**: One instance per request/scope. In web applications, one instance per HTTP request.

**Use Cases** (⭐ **RECOMMENDED FOR MOST SERVICES**):

- Database contexts (Entity Framework DbContext)
- Unit of Work pattern implementations
- Services that maintain state during a request
- Services accessing request-specific data
- Most business logic services

**Example**:

```csharp
builder.Services.AddScoped<ICalculatorService, CalculatorService>();
builder.Services.AddScoped<IOrderService, OrderService>();
```

**Characteristics**:

- ✅ Efficient memory usage (one instance per request)
- ✅ Safe for request-level state management
- ✅ Automatic disposal at end of request
- ⚠️ Not thread-safe across different requests
- ⚠️ Cannot be injected into singleton services

#### 3. Singleton (`AddSingleton`)

**Behavior**: One instance for the entire application lifetime.

**Use Cases**:

- Configuration objects
- Caching services
- Logging frameworks
- Services with expensive initialization
- Truly stateless services shared across all requests

**Example**:

```csharp
builder.Services.AddSingleton<IMemoryCache, MemoryCache>();
builder.Services.AddSingleton<IConfigurationService, ConfigurationService>();
```

**Characteristics**:

- ✅ Best performance (single instance for all requests)
- ✅ Ideal for expensive-to-create services
- ⚠️ Must be thread-safe
- ⚠️ Cannot depend on scoped or transient services
- ⚠️ Careful with state management
- ⚠️ Disposal only at application shutdown

**Common Mistake Warning**:

```csharp
// ❌ BAD: Singleton depending on Scoped service
builder.Services.AddSingleton<ICachingService, CachingService>();
builder.Services.AddScoped<IDbContext, AppDbContext>(); // DbContext is scoped

// CachingService constructor:
public CachingService(IDbContext dbContext) { } // ERROR!
```

### Service Lifetime Decision Tree

```
Is the service expensive to create OR truly stateless?
├─ YES → Consider Singleton (ensure thread-safety)
└─ NO → Is the service managing request-specific state?
    ├─ YES → Use Scoped (DEFAULT CHOICE)
    └─ NO → Is the service lightweight and stateless?
        ├─ YES → Use Transient
        └─ UNSURE → Use Scoped (safe default)
```

### Registration Patterns

#### Standard Registration (Program.cs)

```csharp
var builder = WebApplication.CreateBuilder(args);

// Framework services
builder.Services.AddRazorPages();
builder.Services.AddControllers();

// Application services (grouped by layer)
// Business Logic Layer
builder.Services.AddScoped<ICalculatorService, CalculatorService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

// Data Access Layer
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Infrastructure Services
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddSingleton<ICacheService, RedisCacheService>();

var app = builder.Build();
```

#### Extension Method Pattern (Recommended for Large Projects)

**ServiceCollectionExtensions.cs**:

```csharp
namespace web_calculator.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers application business logic services
    /// </summary>
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        services.AddScoped<ICalculatorService, CalculatorService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IPaymentService, PaymentService>();

        return services;
    }

    /// <summary>
    /// Registers data access repositories
    /// </summary>
    public static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }

    /// <summary>
    /// Registers infrastructure services
    /// </summary>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
    {
        services.AddSingleton<IEmailService, EmailService>();
        services.AddSingleton<ICacheService, RedisCacheService>();

        return services;
    }
}
```

**Program.cs** (cleaner):

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddApplicationServices();
builder.Services.AddRepositories();
builder.Services.AddInfrastructure();

var app = builder.Build();
```

#### Conditional Registration

```csharp
// Register different implementations based on environment
if (app.Environment.IsDevelopment())
{
    builder.Services.AddScoped<IEmailService, FakeEmailService>();
}
else
{
    builder.Services.AddScoped<IEmailService, SendGridEmailService>();
}

// Register based on configuration
if (builder.Configuration.GetValue<bool>("Features:UseRedisCache"))
{
    builder.Services.AddSingleton<ICacheService, RedisCacheService>();
}
else
{
    builder.Services.AddSingleton<ICacheService, MemoryCacheService>();
}
```

#### Generic Type Registration

```csharp
// Register generic repository pattern
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Usage in services:
public class OrderService
{
    private readonly IRepository<Order> _orderRepo;
    private readonly IRepository<Customer> _customerRepo;

    public OrderService(
        IRepository<Order> orderRepo,
        IRepository<Customer> customerRepo)
    {
        _orderRepo = orderRepo;
        _customerRepo = customerRepo;
    }
}
```

### Constructor Injection

**REQUIRED**: Always use constructor injection. Property injection and method injection are NOT supported by the built-in container.

#### Basic Constructor Injection

```csharp
public class CalculatorService : ICalculatorService
{
    private readonly ILogger<CalculatorService> _logger;

    // Constructor receives dependencies
    public CalculatorService(ILogger<CalculatorService> logger)
    {
        _logger = logger;
    }

    public string EvaluateExpression(string expression)
    {
        _logger.LogInformation("Evaluating: {Expression}", expression);
        // Implementation
    }
}
```

#### Multiple Dependencies

```csharp
public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentService _paymentService;
    private readonly IEmailService _emailService;
    private readonly ILogger<OrderService> _logger;

    public OrderService(
        IOrderRepository orderRepository,
        IPaymentService paymentService,
        IEmailService emailService,
        ILogger<OrderService> logger)
    {
        _orderRepository = orderRepository;
        _paymentService = paymentService;
        _emailService = emailService;
        _logger = logger;
    }
}
```

#### C# 12 Primary Constructor Pattern (Modern)

```csharp
// Simplified syntax (C# 12+)
public class CalculatorService(
    ILogger<CalculatorService> logger) : ICalculatorService
{
    public string EvaluateExpression(string expression)
    {
        logger.LogInformation("Evaluating: {Expression}", expression);
        // Implementation
    }
}
```

#### Razor Pages Constructor Injection

```csharp
public class IndexModel : PageModel
{
    private readonly ICalculatorService _calculatorService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(
        ICalculatorService calculatorService,
        ILogger<IndexModel> logger)
    {
        _calculatorService = calculatorService;
        _logger = logger;
    }

    public void OnGet()
    {
        // Use injected services
    }

    public IActionResult OnPost(string expression)
    {
        try
        {
            Result = _calculatorService.EvaluateExpression(expression);
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Calculation failed");
            return Page();
        }
    }
}
```

### DI Best Practices

#### 1. Depend on Abstractions (Interfaces), Not Implementations

```csharp
// ✅ GOOD: Depend on interface
public class OrderService
{
    private readonly IPaymentService _paymentService;

    public OrderService(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }
}

// ❌ BAD: Depend on concrete class
public class OrderService
{
    private readonly StripePaymentService _paymentService;

    public OrderService(StripePaymentService paymentService)
    {
        _paymentService = paymentService;
    }
}
```

#### 2. Keep Dependencies to a Minimum

```csharp
// ⚠️ WARNING: Too many dependencies (Code Smell)
public class OrderService
{
    // 8+ dependencies is a sign of Single Responsibility violation
    public OrderService(
        IOrderRepository orderRepo,
        IProductRepository productRepo,
        ICustomerRepository customerRepo,
        IPaymentService paymentService,
        IEmailService emailService,
        INotificationService notificationService,
        IAuditService auditService,
        ILogger<OrderService> logger) { }
}

// ✅ BETTER: Refactor into smaller services
public class OrderService
{
    private readonly IOrderRepository _orderRepo;
    private readonly IOrderProcessingService _processingService;
    private readonly ILogger<OrderService> _logger;

    public OrderService(
        IOrderRepository orderRepo,
        IOrderProcessingService processingService,
        ILogger<OrderService> logger) { }
}
```

**Rule of Thumb**: If a class has more than 5 dependencies, consider refactoring.

#### 3. Validate Dependencies (Null Safety)

```csharp
// ✅ GOOD: C# 11+ nullable reference types
public class CalculatorService : ICalculatorService
{
    private readonly ILogger<CalculatorService> _logger;

    public CalculatorService(ILogger<CalculatorService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
}

// ✅ BETTER: Enable nullable reference types in .csproj
<Nullable>enable</Nullable>

// Then compiler warns about null assignments
public CalculatorService(ILogger<CalculatorService> logger)
{
    _logger = logger; // Compiler enforces non-null
}
```

#### 4. Use ILogger<T> for Type-Safe Logging

```csharp
// ✅ GOOD: Generic ILogger<T> provides automatic categorization
public class CalculatorService : ICalculatorService
{
    private readonly ILogger<CalculatorService> _logger;

    public CalculatorService(ILogger<CalculatorService> logger)
    {
        _logger = logger;
    }

    public string EvaluateExpression(string expression)
    {
        _logger.LogInformation("Evaluating expression: {Expression}", expression);
        // Implementation
    }
}

// ❌ BAD: Non-generic ILogger loses type context
public CalculatorService(ILogger logger) { }
```

#### 5. Dispose Resources Properly

```csharp
// ✅ GOOD: DI container handles disposal automatically
builder.Services.AddScoped<ICalculatorService, CalculatorService>();

// Services are automatically disposed at end of scope/request

// ⚠️ MANUAL DISPOSAL: Only if you manually resolve services
using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider.GetRequiredService<ICalculatorService>();
    // Use service
} // Scope is disposed here
```

#### 6. Avoid Service Locator Anti-Pattern

```csharp
// ❌ BAD: Service Locator Pattern (accessing IServiceProvider directly)
public class OrderService
{
    private readonly IServiceProvider _serviceProvider;

    public OrderService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void ProcessOrder()
    {
        // Anti-pattern: Resolving services on-demand
        var paymentService = _serviceProvider.GetRequiredService<IPaymentService>();
        paymentService.ProcessPayment();
    }
}

// ✅ GOOD: Constructor Injection (explicit dependencies)
public class OrderService
{
    private readonly IPaymentService _paymentService;

    public OrderService(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    public void ProcessOrder()
    {
        _paymentService.ProcessPayment();
    }
}
```

**Exception**: Service Locator is acceptable in:

- Factory patterns
- Plugin architectures
- Dynamic service resolution based on runtime conditions

#### 7. Register Services in Logical Groups

```csharp
// ✅ GOOD: Organized registration
var builder = WebApplication.CreateBuilder(args);

// Framework Services
builder.Services.AddRazorPages();
builder.Services.AddControllers();

// Application Services (Business Logic)
builder.Services.AddScoped<ICalculatorService, CalculatorService>();
builder.Services.AddScoped<IOrderService, OrderService>();

// Data Access Services
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Infrastructure Services
builder.Services.AddSingleton<IEmailService, EmailService>();

// External Services
builder.Services.AddHttpClient<IApiClient, ApiClient>();

var app = builder.Build();
```

### DI Anti-Patterns

#### ❌ 1. Captive Dependencies

**Problem**: Longer-lived service capturing shorter-lived dependency.

```csharp
// ❌ BAD: Singleton capturing Scoped service
builder.Services.AddSingleton<IReportService, ReportService>();
builder.Services.AddScoped<IDbContext, AppDbContext>();

public class ReportService : IReportService
{
    private readonly IDbContext _dbContext; // PROBLEM!

    // Singleton holds reference to Scoped service
    // DbContext will never be disposed properly
    public ReportService(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}

// ✅ FIX: Change ReportService to Scoped
builder.Services.AddScoped<IReportService, ReportService>();
```

#### ❌ 2. New Keyword for Dependencies

```csharp
// ❌ BAD: Manually creating dependencies
public class OrderService
{
    public void ProcessOrder()
    {
        var paymentService = new PaymentService();
        paymentService.ProcessPayment();
    }
}

// ✅ GOOD: Inject dependency
public class OrderService
{
    private readonly IPaymentService _paymentService;

    public OrderService(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }
}
```

#### ❌ 3. Static Service Access

```csharp
// ❌ BAD: Static service accessor
public static class ServiceLocator
{
    public static IServiceProvider Services { get; set; }
}

public class OrderService
{
    public void ProcessOrder()
    {
        var paymentService = ServiceLocator.Services
            .GetRequiredService<IPaymentService>();
    }
}

// ✅ GOOD: Use constructor injection
```

#### ❌ 4. Circular Dependencies

```csharp
// ❌ BAD: Circular dependency
public class ServiceA
{
    public ServiceA(IServiceB serviceB) { }
}

public class ServiceB : IServiceB
{
    public ServiceB(IServiceA serviceA) { } // Circular!
}

// ✅ FIX: Extract shared logic to third service
public class ServiceA
{
    public ServiceA(ISharedService sharedService) { }
}

public class ServiceB : IServiceB
{
    public ServiceB(ISharedService sharedService) { }
}
```

---

## Service Layer Pattern

### Service Layer Overview

**Definition**: The Service Layer pattern encapsulates business logic in a separate layer, providing a well-defined API for the application's operations. Services orchestrate domain logic, coordinate transactions, and manage interactions between different layers.

**Benefits**:

- **Separation of Concerns**: Business logic separated from presentation and data access
- **Reusability**: Services can be consumed by multiple clients (web, API, background jobs)
- **Testability**: Business logic can be unit tested independently
- **Maintainability**: Changes to business logic isolated from UI and data layers
- **Transaction Boundaries**: Natural place to define transaction scopes

**Layer Architecture**:

```
┌─────────────────────────────────────┐
│   Presentation Layer (Razor Pages)  │  ← UI, Controllers, Page Models
├─────────────────────────────────────┤
│   Service Layer (Business Logic)    │  ← ICalculatorService, IOrderService
├─────────────────────────────────────┤
│   Data Access Layer (Repositories)  │  ← IRepository<T>, DbContext
├─────────────────────────────────────┤
│   Domain Layer (Entities, Models)   │  ← Domain models, Value objects
└─────────────────────────────────────┘
```

### Interface Design

#### Interface Naming Conventions

**Standard**: Prefix with `I`, use descriptive names ending in `Service`

```csharp
// ✅ GOOD: Clear, descriptive names
public interface ICalculatorService { }
public interface IOrderService { }
public interface IPaymentService { }
public interface INotificationService { }

// ❌ BAD: Vague or missing 'Service' suffix
public interface ICalculator { }  // Missing 'Service'
public interface IManager { }     // Too generic
public interface IHandler { }     // Unclear purpose
```

#### Method Naming Conventions

**Standards**:

- Use verbs for actions: `Calculate`, `Process`, `Validate`, `Create`, `Update`, `Delete`
- Return meaningful types (avoid `void` when possible)
- Use async suffix for async methods: `CalculateAsync`, `ProcessOrderAsync`

```csharp
public interface ICalculatorService
{
    /// <summary>
    /// Evaluates a mathematical expression and returns the result
    /// </summary>
    string EvaluateExpression(string expression);

    // ✅ GOOD: Async variant with Async suffix
    Task<string> EvaluateExpressionAsync(string expression);
}

public interface IOrderService
{
    /// <summary>
    /// Creates a new order
    /// </summary>
    Task<OrderResult> CreateOrderAsync(CreateOrderRequest request);

    /// <summary>
    /// Retrieves order by ID
    /// </summary>
    Task<Order?> GetOrderByIdAsync(int orderId);

    /// <summary>
    /// Updates existing order
    /// </summary>
    Task<OrderResult> UpdateOrderAsync(int orderId, UpdateOrderRequest request);

    /// <summary>
    /// Cancels an order
    /// </summary>
    Task<bool> CancelOrderAsync(int orderId);
}
```

#### Result Types

**Pattern**: Use result types instead of throwing exceptions for expected failures.

```csharp
// ✅ GOOD: Result type pattern
public class OrderResult
{
    public bool IsSuccess { get; init; }
    public Order? Order { get; init; }
    public string? ErrorMessage { get; init; }
    public IEnumerable<string> Errors { get; init; } = Array.Empty<string>();

    public static OrderResult Success(Order order) =>
        new() { IsSuccess = true, Order = order };

    public static OrderResult Failure(string error) =>
        new() { IsSuccess = false, ErrorMessage = error };
}

public interface IOrderService
{
    Task<OrderResult> CreateOrderAsync(CreateOrderRequest request);
}

// Usage in controller/page model:
var result = await _orderService.CreateOrderAsync(request);
if (result.IsSuccess)
{
    return RedirectToPage("Success", new { orderId = result.Order!.Id });
}
else
{
    ModelState.AddModelError("", result.ErrorMessage ?? "Order creation failed");
    return Page();
}
```

#### XML Documentation (Required)

**Standard**: All public interfaces and methods MUST have XML documentation.

```csharp
namespace web_calculator.Services;

/// <summary>
/// Interface for calculator operations
/// </summary>
public interface ICalculatorService
{
    /// <summary>
    /// Evaluates a mathematical expression and returns the result
    /// </summary>
    /// <param name="expression">The mathematical expression to evaluate</param>
    /// <returns>The result of the calculation as a string</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the expression is null or whitespace
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the expression cannot be evaluated
    /// </exception>
    string EvaluateExpression(string expression);
}
```

### Implementation Guidelines

#### Service Implementation Structure

```csharp
namespace web_calculator.Services;

/// <summary>
/// Service for performing calculator operations
/// </summary>
public class CalculatorService : ICalculatorService
{
    // 1. Dependencies (readonly fields)
    private readonly ILogger<CalculatorService> _logger;

    // 2. Constructor with dependency injection
    public CalculatorService(ILogger<CalculatorService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // 3. Public interface methods
    /// <inheritdoc />
    public string EvaluateExpression(string expression)
    {
        // Input validation
        if (string.IsNullOrWhiteSpace(expression))
        {
            _logger.LogWarning("Attempted to evaluate null or empty expression");
            throw new ArgumentException(
                "Expression cannot be null or whitespace.",
                nameof(expression));
        }

        try
        {
            // Logging
            _logger.LogInformation("Evaluating expression: {Expression}", expression);

            // Business logic
            var result = EvaluateInternal(expression);

            // Success logging
            _logger.LogInformation(
                "Successfully evaluated expression: {Expression} = {Result}",
                expression,
                result);

            return result;
        }
        catch (Exception ex)
        {
            // Error logging
            _logger.LogError(ex, "Error evaluating expression: {Expression}", expression);
            throw new InvalidOperationException(
                $"Unable to evaluate expression: {ex.Message}",
                ex);
        }
    }

    // 4. Private helper methods
    private string EvaluateInternal(string expression)
    {
        var table = new DataTable();
        var value = table.Compute(expression, string.Empty);
        return value.ToString() ?? "0";
    }
}
```

#### Validation Pattern (Recommended)

```csharp
public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<OrderService> _logger;

    public OrderService(
        IOrderRepository orderRepository,
        ILogger<OrderService> logger)
    {
        _orderRepository = orderRepository;
        _logger = logger;
    }

    public async Task<OrderResult> CreateOrderAsync(CreateOrderRequest request)
    {
        // 1. Input validation
        var validationErrors = ValidateCreateOrderRequest(request);
        if (validationErrors.Any())
        {
            _logger.LogWarning("Order creation validation failed: {Errors}", validationErrors);
            return OrderResult.Failure(validationErrors);
        }

        try
        {
            // 2. Business logic
            _logger.LogInformation("Creating order for customer {CustomerId}", request.CustomerId);

            var order = new Order
            {
                CustomerId = request.CustomerId,
                OrderDate = DateTime.UtcNow,
                Items = request.Items
            };

            // 3. Data persistence
            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();

            _logger.LogInformation("Order {OrderId} created successfully", order.Id);

            // 4. Return result
            return OrderResult.Success(order);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create order");
            return OrderResult.Failure("An error occurred while creating the order");
        }
    }

    private List<string> ValidateCreateOrderRequest(CreateOrderRequest request)
    {
        var errors = new List<string>();

        if (request.CustomerId <= 0)
            errors.Add("Invalid customer ID");

        if (request.Items == null || !request.Items.Any())
            errors.Add("Order must contain at least one item");

        if (request.Items?.Any(i => i.Quantity <= 0) == true)
            errors.Add("All items must have positive quantity");

        return errors;
    }
}
```

#### Async/Await Best Practices

```csharp
public interface IOrderService
{
    // ✅ GOOD: Async methods return Task or Task<T>
    Task<OrderResult> CreateOrderAsync(CreateOrderRequest request);
    Task<Order?> GetOrderByIdAsync(int orderId);

    // ❌ BAD: Async void (only for event handlers)
    // async void ProcessOrderAsync(int orderId);
}

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    // ✅ GOOD: Properly implemented async method
    public async Task<Order?> GetOrderByIdAsync(int orderId)
    {
        return await _orderRepository.GetByIdAsync(orderId);
    }

    // ❌ BAD: Unnecessary async/await (no actual async work)
    public async Task<decimal> CalculateTotalAsync(Order order)
    {
        return await Task.FromResult(order.Items.Sum(i => i.Price * i.Quantity));
    }

    // ✅ BETTER: Synchronous method when no async operations
    public decimal CalculateTotal(Order order)
    {
        return order.Items.Sum(i => i.Price * i.Quantity);
    }

    // ✅ GOOD: Combining multiple async operations
    public async Task<OrderSummary> GetOrderSummaryAsync(int orderId)
    {
        // Run multiple async operations in parallel
        var orderTask = _orderRepository.GetByIdAsync(orderId);
        var customerTask = _customerRepository.GetByIdAsync(orderId);

        await Task.WhenAll(orderTask, customerTask);

        var order = await orderTask;
        var customer = await customerTask;

        return new OrderSummary(order, customer);
    }
}
```

### Service Layer Best Practices

#### 1. Single Responsibility Principle

```csharp
// ❌ BAD: Service doing too much
public class OrderService
{
    public Task<Order> CreateOrderAsync() { }
    public Task ProcessPaymentAsync() { }      // Payment logic
    public Task SendEmailAsync() { }           // Email logic
    public Task UpdateInventoryAsync() { }     // Inventory logic
}

// ✅ GOOD: Focused services
public class OrderService
{
    private readonly IPaymentService _paymentService;
    private readonly IEmailService _emailService;
    private readonly IInventoryService _inventoryService;

    public async Task<OrderResult> CreateOrderAsync(CreateOrderRequest request)
    {
        var order = await SaveOrderAsync(request);
        await _paymentService.ProcessPaymentAsync(order.Id);
        await _emailService.SendOrderConfirmationAsync(order.Id);
        await _inventoryService.ReserveItemsAsync(order.Items);
        return OrderResult.Success(order);
    }
}
```

#### 2. Avoid Business Logic in Controllers/Page Models

```csharp
// ❌ BAD: Business logic in Razor Page Model
public class IndexModel : PageModel
{
    private readonly IOrderRepository _orderRepository;

    public async Task<IActionResult> OnPostAsync()
    {
        // Business logic should NOT be here
        var order = new Order
        {
            CustomerId = CustomerId,
            OrderDate = DateTime.UtcNow,
            Total = Items.Sum(i => i.Price * i.Quantity)
        };

        if (order.Total > 1000)
            order.DiscountPercent = 10;

        await _orderRepository.AddAsync(order);
        return RedirectToPage("Success");
    }
}

// ✅ GOOD: Business logic in service
public class IndexModel : PageModel
{
    private readonly IOrderService _orderService;

    public async Task<IActionResult> OnPostAsync()
    {
        var result = await _orderService.CreateOrderAsync(new CreateOrderRequest
        {
            CustomerId = CustomerId,
            Items = Items
        });

        if (result.IsSuccess)
            return RedirectToPage("Success");

        ModelState.AddModelError("", result.ErrorMessage);
        return Page();
    }
}
```

#### 3. Use DTOs for Service Boundaries

```csharp
// Request DTOs
public record CreateOrderRequest
{
    public int CustomerId { get; init; }
    public List<OrderItemDto> Items { get; init; } = new();
}

public record OrderItemDto
{
    public int ProductId { get; init; }
    public int Quantity { get; init; }
}

// Response DTOs
public record OrderDto
{
    public int Id { get; init; }
    public int CustomerId { get; init; }
    public DateTime OrderDate { get; init; }
    public decimal Total { get; init; }
    public List<OrderItemDto> Items { get; init; } = new();
}

// ✅ GOOD: Service uses DTOs
public interface IOrderService
{
    Task<OrderResult> CreateOrderAsync(CreateOrderRequest request);
    Task<OrderDto?> GetOrderByIdAsync(int orderId);
    Task<List<OrderDto>> GetOrdersForCustomerAsync(int customerId);
}
```

#### 4. Transaction Management

```csharp
public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPaymentService _paymentService;

    // ✅ GOOD: Transaction spanning multiple operations
    public async Task<OrderResult> CreateOrderAsync(CreateOrderRequest request)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            // 1. Create order
            var order = new Order { /* ... */ };
            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();

            // 2. Process payment
            var paymentResult = await _paymentService.ProcessPaymentAsync(order.Id);
            if (!paymentResult.IsSuccess)
            {
                await transaction.RollbackAsync();
                return OrderResult.Failure("Payment processing failed");
            }

            // 3. Update inventory
            foreach (var item in order.Items)
            {
                var product = await _dbContext.Products.FindAsync(item.ProductId);
                product.Stock -= item.Quantity;
            }
            await _dbContext.SaveChangesAsync();

            // 4. Commit transaction
            await transaction.CommitAsync();
            return OrderResult.Success(order);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Order creation failed");
            return OrderResult.Failure("An error occurred");
        }
    }
}
```

#### 5. Logging Standards

```csharp
public class CalculatorService : ICalculatorService
{
    private readonly ILogger<CalculatorService> _logger;

    public string EvaluateExpression(string expression)
    {
        // ✅ GOOD: Structured logging with parameters
        _logger.LogInformation("Evaluating expression: {Expression}", expression);

        try
        {
            var result = Evaluate(expression);

            // ✅ GOOD: Log success with context
            _logger.LogInformation(
                "Expression evaluated successfully: {Expression} = {Result}",
                expression,
                result);

            return result;
        }
        catch (Exception ex)
        {
            // ✅ GOOD: Log errors with exception
            _logger.LogError(
                ex,
                "Failed to evaluate expression: {Expression}",
                expression);
            throw;
        }
    }

    // ❌ BAD: String concatenation in logging
    // _logger.LogInformation("Evaluating: " + expression);

    // ❌ BAD: Logging without context
    // _logger.LogInformation("Evaluation successful");
}
```

### Service Layer Anti-Patterns

#### ❌ 1. Anemic Domain Model (Service as Data Holder)

```csharp
// ❌ BAD: Service is just a pass-through
public class OrderService
{
    private readonly IOrderRepository _repository;

    public Task<Order> GetByIdAsync(int id) =>
        _repository.GetByIdAsync(id);

    public Task AddAsync(Order order) =>
        _repository.AddAsync(order);
}

// ✅ GOOD: Service adds business logic
public class OrderService
{
    public async Task<OrderResult> CreateOrderAsync(CreateOrderRequest request)
    {
        // Validation
        if (!await ValidateInventoryAsync(request.Items))
            return OrderResult.Failure("Insufficient inventory");

        // Business logic
        var order = await BuildOrderAsync(request);
        await ApplyDiscountsAsync(order);

        // Persistence
        await _repository.AddAsync(order);
        return OrderResult.Success(order);
    }
}
```

#### ❌ 2. God Service (Service Doing Everything)

```csharp
// ❌ BAD: One massive service
public class ApplicationService
{
    public Task ManageOrdersAsync() { }
    public Task ManageCustomersAsync() { }
    public Task ManageProductsAsync() { }
    public Task ManagePaymentsAsync() { }
    // ... 20 more methods
}

// ✅ GOOD: Focused services
public class OrderService { }
public class CustomerService { }
public class ProductService { }
public class PaymentService { }
```

#### ❌ 3. Exposing Domain Entities Directly

```csharp
// ❌ BAD: Exposing EF entities
public interface IOrderService
{
    Task<Order> GetOrderByIdAsync(int id); // Order is EF entity
}

// ✅ GOOD: Return DTOs
public interface IOrderService
{
    Task<OrderDto> GetOrderByIdAsync(int id); // OrderDto is data transfer object
}
```

---

## Integration Patterns

### Combining DI and Service Layer

#### Full Example: Calculator Service

**1. Define Interface** (`Services/ICalculatorService.cs`):

```csharp
namespace web_calculator.Services;

/// <summary>
/// Interface for calculator operations
/// </summary>
public interface ICalculatorService
{
    /// <summary>
    /// Evaluates a mathematical expression and returns the result
    /// </summary>
    string EvaluateExpression(string expression);
}
```

**2. Implement Service** (`Services/CalculatorService.cs`):

```csharp
using System.Data;

namespace web_calculator.Services;

/// <summary>
/// Service for performing calculator operations
/// </summary>
public class CalculatorService : ICalculatorService
{
    private readonly ILogger<CalculatorService> _logger;

    public CalculatorService(ILogger<CalculatorService> logger)
    {
        _logger = logger;
    }

    public string EvaluateExpression(string expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
        {
            _logger.LogWarning("Attempted to evaluate null or empty expression");
            throw new ArgumentException(
                "Expression cannot be null or whitespace.",
                nameof(expression));
        }

        try
        {
            _logger.LogInformation("Evaluating expression: {Expression}", expression);

            var table = new DataTable();
            var value = table.Compute(expression, string.Empty);
            var result = value.ToString() ?? "0";

            _logger.LogInformation(
                "Successfully evaluated: {Expression} = {Result}",
                expression,
                result);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error evaluating expression: {Expression}", expression);
            throw new InvalidOperationException(
                $"Unable to evaluate expression: {ex.Message}",
                ex);
        }
    }
}
```

**3. Register Service** (`Program.cs`):

```csharp
using web_calculator.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorPages();

// Register calculator service with Scoped lifetime
builder.Services.AddScoped<ICalculatorService, CalculatorService>();

var app = builder.Build();

// Configure pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();

app.Run();
```

**4. Consume Service in Razor Page** (`Pages/Index.cshtml.cs`):

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using web_calculator.Services;

namespace web_calculator.Pages;

public class IndexModel : PageModel
{
    private readonly ICalculatorService _calculatorService;
    private readonly ILogger<IndexModel> _logger;

    [BindProperty]
    public string Expression { get; set; } = string.Empty;

    public string? Result { get; set; }
    public string? ErrorMessage { get; set; }

    public IndexModel(
        ICalculatorService calculatorService,
        ILogger<IndexModel> logger)
    {
        _calculatorService = calculatorService;
        _logger = logger;
    }

    public void OnGet()
    {
        // Initial page load
    }

    public IActionResult OnPost()
    {
        if (string.IsNullOrWhiteSpace(Expression))
        {
            ErrorMessage = "Please enter an expression";
            return Page();
        }

        try
        {
            Result = _calculatorService.EvaluateExpression(Expression);
            return Page();
        }
        catch (ArgumentException ex)
        {
            ErrorMessage = ex.Message;
            _logger.LogWarning("Invalid expression: {Expression}", Expression);
            return Page();
        }
        catch (InvalidOperationException ex)
        {
            ErrorMessage = "Unable to calculate result. Please check your expression.";
            _logger.LogError(ex, "Calculation error for expression: {Expression}", Expression);
            return Page();
        }
    }
}
```

---

## Testing Strategies

### Unit Testing Services

#### Test Setup with Mocking

```csharp
using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using web_calculator.Services;

public class CalculatorServiceTests
{
    private readonly Mock<ILogger<CalculatorService>> _mockLogger;
    private readonly ICalculatorService _calculatorService;

    public CalculatorServiceTests()
    {
        _mockLogger = new Mock<ILogger<CalculatorService>>();
        _calculatorService = new CalculatorService(_mockLogger.Object);
    }

    [Fact]
    public void EvaluateExpression_ValidExpression_ReturnsCorrectResult()
    {
        // Arrange
        var expression = "2 + 2";

        // Act
        var result = _calculatorService.EvaluateExpression(expression);

        // Assert
        Assert.Equal("4", result);
    }

    [Theory]
    [InlineData("10 + 5", "15")]
    [InlineData("10 - 5", "5")]
    [InlineData("10 * 5", "50")]
    [InlineData("10 / 5", "2")]
    public void EvaluateExpression_BasicOperations_ReturnsCorrectResults(
        string expression,
        string expected)
    {
        // Act
        var result = _calculatorService.EvaluateExpression(expression);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void EvaluateExpression_NullOrWhitespace_ThrowsArgumentException(
        string expression)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            _calculatorService.EvaluateExpression(expression));
    }

    [Fact]
    public void EvaluateExpression_InvalidExpression_ThrowsInvalidOperationException()
    {
        // Arrange
        var expression = "invalid expression";

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            _calculatorService.EvaluateExpression(expression));
    }

    [Fact]
    public void EvaluateExpression_ValidExpression_LogsInformation()
    {
        // Arrange
        var expression = "2 + 2";

        // Act
        _calculatorService.EvaluateExpression(expression);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Evaluating expression")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.AtLeastOnce);
    }
}
```

### Integration Testing with Services

```csharp
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

public class CalculatorIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public CalculatorIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CalculatorService_CanBeResolvedFromDI()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();

        // Act
        var calculatorService = scope.ServiceProvider
            .GetRequiredService<ICalculatorService>();

        // Assert
        Assert.NotNull(calculatorService);

        var result = calculatorService.EvaluateExpression("5 + 5");
        Assert.Equal("10", result);
    }

    [Fact]
    public async Task CalculatorPage_PostRequest_UsesService()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsync("/Index",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["Expression"] = "10 + 5"
            }));

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("15", content);
    }
}
```

### Test Doubles and Mocking

```csharp
// Fake implementation for testing
public class FakeCalculatorService : ICalculatorService
{
    public string EvaluateExpression(string expression)
    {
        // Simple fake implementation for testing
        return expression switch
        {
            "2 + 2" => "4",
            "10 * 5" => "50",
            _ => "0"
        };
    }
}

// Register fake in test configuration
builder.Services.AddScoped<ICalculatorService, FakeCalculatorService>();
```

---

## Migration Strategies

### Refactoring to Service Layer

#### Before: Logic in Controller/Page Model

```csharp
// ❌ Before: Business logic in Razor Page
public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _dbContext;

    public async Task<IActionResult> OnPostAsync()
    {
        // Business logic mixed with presentation
        if (string.IsNullOrWhiteSpace(Expression))
        {
            ErrorMessage = "Expression required";
            return Page();
        }

        try
        {
            var table = new DataTable();
            var result = table.Compute(Expression, string.Empty);
            Result = result.ToString();

            // Save to database
            _dbContext.Calculations.Add(new Calculation
            {
                Expression = Expression,
                Result = Result,
                Timestamp = DateTime.UtcNow
            });
            await _dbContext.SaveChangesAsync();

            return Page();
        }
        catch
        {
            ErrorMessage = "Invalid expression";
            return Page();
        }
    }
}
```

#### After: Service Layer Extracted

```csharp
// ✅ After: Service with business logic
public class CalculatorService : ICalculatorService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CalculatorService> _logger;

    public CalculatorService(
        ApplicationDbContext dbContext,
        ILogger<CalculatorService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<CalculationResult> EvaluateAndSaveAsync(string expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
        {
            return CalculationResult.Failure("Expression required");
        }

        try
        {
            _logger.LogInformation("Evaluating: {Expression}", expression);

            var table = new DataTable();
            var value = table.Compute(expression, string.Empty);
            var result = value.ToString() ?? "0";

            var calculation = new Calculation
            {
                Expression = expression,
                Result = result,
                Timestamp = DateTime.UtcNow
            };

            _dbContext.Calculations.Add(calculation);
            await _dbContext.SaveChangesAsync();

            return CalculationResult.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Evaluation failed: {Expression}", expression);
            return CalculationResult.Failure("Invalid expression");
        }
    }
}

// ✅ Simplified Razor Page
public class IndexModel : PageModel
{
    private readonly ICalculatorService _calculatorService;

    public IndexModel(ICalculatorService calculatorService)
    {
        _calculatorService = calculatorService;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var result = await _calculatorService.EvaluateAndSaveAsync(Expression);

        if (result.IsSuccess)
        {
            Result = result.Value;
        }
        else
        {
            ErrorMessage = result.ErrorMessage;
        }

        return Page();
    }
}
```

### Incremental Adoption

**Phase 1**: Extract service interface

```csharp
// Week 1: Define interface
public interface ICalculatorService
{
    string EvaluateExpression(string expression);
}
```

**Phase 2**: Implement service

```csharp
// Week 2: Create implementation
public class CalculatorService : ICalculatorService
{
    // Implementation
}
```

**Phase 3**: Register in DI

```csharp
// Week 3: Register service
builder.Services.AddScoped<ICalculatorService, CalculatorService>();
```

**Phase 4**: Refactor consumers

```csharp
// Week 4: Update Razor Pages to use service
public IndexModel(ICalculatorService calculatorService) { }
```

---

## Summary

### Key Takeaways

**Dependency Injection**:

- ✅ Use built-in .NET DI container
- ✅ Prefer **Scoped** lifetime for most services
- ✅ Always use constructor injection
- ✅ Depend on interfaces, not implementations
- ❌ Avoid Service Locator anti-pattern
- ❌ Prevent captive dependencies

**Service Layer**:

- ✅ Encapsulate business logic in services
- ✅ Use clear interface contracts
- ✅ Return result types for expected failures
- ✅ Use DTOs for service boundaries
- ✅ Implement proper logging and exception handling
- ❌ Avoid anemic services (pass-through only)
- ❌ Avoid God services (doing everything)

**Integration**:

- Register services in `Program.cs` or extension methods
- Inject services via constructors
- Keep Razor Pages/Controllers thin (delegate to services)
- Write unit tests with mocked dependencies
- Use integration tests to verify DI container configuration

### Quick Reference

**Service Registration**:

```csharp
// Scoped (recommended for most services)
builder.Services.AddScoped<ICalculatorService, CalculatorService>();

// Transient (lightweight, stateless)
builder.Services.AddTransient<IEmailService, EmailService>();

// Singleton (expensive to create, thread-safe)
builder.Services.AddSingleton<ICacheService, CacheService>();
```

**Service Consumption**:

```csharp
public class IndexModel : PageModel
{
    private readonly ICalculatorService _calculatorService;

    public IndexModel(ICalculatorService calculatorService)
    {
        _calculatorService = calculatorService;
    }
}
```

**Testing**:

```csharp
var mockService = new Mock<ICalculatorService>();
mockService.Setup(s => s.EvaluateExpression("2+2")).Returns("4");
```

---

## Additional Resources

### Official Documentation

- [ASP.NET Core Dependency Injection](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection)
- [Service Lifetimes](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection#service-lifetimes)
- [.NET Design Patterns](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/architectural-principles)

### Related Patterns

- Repository Pattern (Data Access)
- Unit of Work Pattern (Transaction Management)
- Factory Pattern (Complex Object Creation)
- Strategy Pattern (Algorithm Selection)

### Troubleshooting

**Problem**: "Cannot resolve scoped service from singleton"

**Solution**: Change singleton to scoped or inject `IServiceProvider` and create scope manually.

**Problem**: Services not resolved in tests

**Solution**: Ensure `WebApplicationFactory` is properly configured with test services.

---

**Document Version**: 1.0.0
**Last Updated**: 2026-02-10
**Next Review**: 2026-05-10
