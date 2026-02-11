namespace PostHubAPI.Models;

/// <summary>
/// Configuration options for feature flags
/// </summary>
public class FeatureFlagOptions
{
    /// <summary>
    /// Configuration section name
    /// </summary>
    public const string SectionName = "FeatureFlags";

    /// <summary>
    /// List of feature flags
    /// </summary>
    public List<FeatureFlag> Flags { get; set; } = new();
}
