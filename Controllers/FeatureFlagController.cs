using Microsoft.AspNetCore.Mvc;
using PostHubAPI.Attributes;
using PostHubAPI.Services.Interfaces;

namespace PostHubAPI.Controllers;

/// <summary>
/// Controller demonstrating feature flag usage and providing feature flag information
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class FeatureFlagController : ControllerBase
{
    private readonly IFeatureFlagService _featureFlagService;
    private readonly ILogger<FeatureFlagController> _logger;

    public FeatureFlagController(
        IFeatureFlagService featureFlagService,
        ILogger<FeatureFlagController> logger)
    {
        _featureFlagService = featureFlagService;
        _logger = logger;
    }

    /// <summary>
    /// Get all feature flags for the current user
    /// </summary>
    [HttpGet]
    public IActionResult GetAllFlags()
    {
        var userId = HttpContext.Items["UserId"]?.ToString();
        var flags = _featureFlagService.GetAllFlags(userId);

        return Ok(new
        {
            userId = userId ?? "anonymous",
            flags = flags.Select(f => new
            {
                name = f.Key,
                enabled = f.Value,
                variant = _featureFlagService.GetVariant(f.Key, userId)
            })
        });
    }

    /// <summary>
    /// Check if a specific feature is enabled
    /// </summary>
    [HttpGet("{featureName}")]
    public IActionResult CheckFeature(string featureName)
    {
        if (!_featureFlagService.FeatureExists(featureName))
        {
            return NotFound(new { error = $"Feature '{featureName}' not found" });
        }

        var userId = HttpContext.Items["UserId"]?.ToString();
        var isEnabled = _featureFlagService.IsEnabled(featureName, userId);
        var variant = _featureFlagService.GetVariant(featureName, userId);

        return Ok(new
        {
            feature = featureName,
            enabled = isEnabled,
            variant = variant,
            userId = userId ?? "anonymous"
        });
    }

    /// <summary>
    /// Example endpoint gated by a feature flag
    /// </summary>
    [HttpGet("experimental/new-feature")]
    [RequireFeatureFlag("NewFeature")]
    public IActionResult ExperimentalFeature()
    {
        return Ok(new
        {
            message = "You have access to the new feature!",
            timestamp = DateTime.UtcNow
        });
    }

    /// <summary>
    /// Example endpoint with A/B testing - Control variant
    /// </summary>
    [HttpGet("ab-test/control")]
    [RequireFeatureFlag("ABTestFeature", "control")]
    public IActionResult ABTestControl()
    {
        return Ok(new
        {
            variant = "control",
            message = "This is the control version",
            design = "classic"
        });
    }

    /// <summary>
    /// Example endpoint with A/B testing - Variant A
    /// </summary>
    [HttpGet("ab-test/variant-a")]
    [RequireFeatureFlag("ABTestFeature", "variant-a")]
    public IActionResult ABTestVariantA()
    {
        return Ok(new
        {
            variant = "variant-a",
            message = "This is variant A with new design",
            design = "modern"
        });
    }

    /// <summary>
    /// Example endpoint with A/B testing - Variant B
    /// </summary>
    [HttpGet("ab-test/variant-b")]
    [RequireFeatureFlag("ABTestFeature", "variant-b")]
    public IActionResult ABTestVariantB()
    {
        return Ok(new
        {
            variant = "variant-b",
            message = "This is variant B with experimental design",
            design = "futuristic"
        });
    }

    /// <summary>
    /// Get assigned A/B test variant for a feature
    /// </summary>
    [HttpGet("ab-test/my-variant")]
    public IActionResult GetMyVariant([FromQuery] string feature = "ABTestFeature")
    {
        var userId = HttpContext.Items["UserId"]?.ToString();
        
        if (!_featureFlagService.FeatureExists(feature))
        {
            return NotFound(new { error = $"Feature '{feature}' not found" });
        }

        var variant = _featureFlagService.GetVariant(feature, userId);
        var isEnabled = _featureFlagService.IsEnabled(feature, userId);

        return Ok(new
        {
            feature = feature,
            enabled = isEnabled,
            assignedVariant = variant,
            userId = userId ?? "anonymous",
            message = variant != null 
                ? $"You are assigned to variant: {variant}" 
                : "Feature not enabled or no variants configured"
        });
    }
}
