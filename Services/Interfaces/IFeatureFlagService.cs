namespace PostHubAPI.Services.Interfaces;

/// <summary>
/// Service for managing and evaluating feature flags
/// </summary>
public interface IFeatureFlagService
{
    /// <summary>
    /// Check if a feature flag is enabled
    /// </summary>
    /// <param name="featureName">Name of the feature flag</param>
    /// <param name="userId">Optional user ID for user-specific checks</param>
    /// <returns>True if the feature is enabled for the user/request</returns>
    bool IsEnabled(string featureName, string? userId = null);

    /// <summary>
    /// Get the assigned variant for A/B testing
    /// </summary>
    /// <param name="featureName">Name of the feature flag</param>
    /// <param name="userId">User ID for consistent variant assignment</param>
    /// <returns>The variant name, or null if feature is disabled or has no variants</returns>
    string? GetVariant(string featureName, string? userId = null);

    /// <summary>
    /// Get all feature flags
    /// </summary>
    /// <returns>Dictionary of feature name to enabled status</returns>
    Dictionary<string, bool> GetAllFlags(string? userId = null);

    /// <summary>
    /// Check if a feature flag exists
    /// </summary>
    /// <param name="featureName">Name of the feature flag</param>
    /// <returns>True if the feature flag is configured</returns>
    bool FeatureExists(string featureName);
}
