using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PostHubAPI.Services.Interfaces;

namespace PostHubAPI.Attributes;

/// <summary>
/// Attribute to gate controller actions behind a feature flag
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class RequireFeatureFlagAttribute : Attribute, IAsyncActionFilter
{
    private readonly string _featureName;
    private readonly string? _requiredVariant;

    /// <summary>
    /// Creates a feature flag requirement
    /// </summary>
    /// <param name="featureName">Name of the feature flag</param>
    /// <param name="requiredVariant">Optional: Specific variant required (for A/B testing)</param>
    public RequireFeatureFlagAttribute(string featureName, string? requiredVariant = null)
    {
        _featureName = featureName;
        _requiredVariant = requiredVariant;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var featureFlagService = context.HttpContext.RequestServices
            .GetRequiredService<IFeatureFlagService>();

        // Get user ID from HttpContext items (set by middleware)
        var userId = context.HttpContext.Items["UserId"]?.ToString();

        // Check if feature exists
        if (!featureFlagService.FeatureExists(_featureName))
        {
            context.Result = new NotFoundObjectResult(new 
            { 
                error = $"Feature '{_featureName}' not configured" 
            });
            return;
        }

        // Check if feature is enabled
        if (!featureFlagService.IsEnabled(_featureName, userId))
        {
            context.Result = new NotFoundObjectResult(new 
            { 
                error = "Feature not available",
                feature = _featureName
            });
            return;
        }

        // If specific variant required, check variant
        if (!string.IsNullOrEmpty(_requiredVariant))
        {
            var assignedVariant = featureFlagService.GetVariant(_featureName, userId);
            if (assignedVariant != _requiredVariant)
            {
                context.Result = new NotFoundObjectResult(new 
                { 
                    error = "Feature variant not assigned",
                    feature = _featureName,
                    requiredVariant = _requiredVariant,
                    assignedVariant = assignedVariant
                });
                return;
            }
        }

        // Feature is enabled, continue
        await next();
    }
}
