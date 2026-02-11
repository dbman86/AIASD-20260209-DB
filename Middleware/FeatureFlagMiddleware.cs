using PostHubAPI.Services.Interfaces;

namespace PostHubAPI.Middleware;

/// <summary>
/// Middleware for attaching feature flag information to HTTP context
/// </summary>
public class FeatureFlagMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<FeatureFlagMiddleware> _logger;

    public FeatureFlagMiddleware(RequestDelegate next, ILogger<FeatureFlagMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IFeatureFlagService featureFlagService)
    {
        // Try to get user ID from claims (if authenticated)
        var userId = context.User?.Identity?.IsAuthenticated == true 
            ? context.User.Identity.Name 
            : null;

        // Store user ID in HttpContext items for later use
        context.Items["UserId"] = userId;

        // Optionally add feature flags to response headers for debugging (only in development)
        if (context.RequestServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment())
        {
            var flags = featureFlagService.GetAllFlags(userId);
            context.Response.Headers.Append("X-Feature-Flags", 
                string.Join(",", flags.Where(f => f.Value).Select(f => f.Key)));
        }

        _logger.LogDebug("Feature flag middleware processed for user: {UserId}", userId ?? "anonymous");

        await _next(context);
    }
}

/// <summary>
/// Extension methods for registering feature flag middleware
/// </summary>
public static class FeatureFlagMiddlewareExtensions
{
    public static IApplicationBuilder UseFeatureFlags(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<FeatureFlagMiddleware>();
    }
}
