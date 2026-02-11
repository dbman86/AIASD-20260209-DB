namespace PostHubAPI.Models;

/// <summary>
/// Represents a feature flag configuration
/// </summary>
public class FeatureFlag
{
    /// <summary>
    /// Name/Key of the feature flag
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Whether the feature is enabled
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// Description of what this feature flag controls
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Percentage of users who should see this feature (0-100)
    /// Used for gradual rollouts. If null, uses Enabled flag only.
    /// </summary>
    public int? RolloutPercentage { get; set; }

    /// <summary>
    /// Variants for A/B testing. If specified, users are assigned to one of these variants.
    /// Example: ["control", "variant-a", "variant-b"]
    /// </summary>
    public List<string>? Variants { get; set; }

    /// <summary>
    /// User IDs that should always have this feature enabled (override)
    /// </summary>
    public List<string>? AllowedUsers { get; set; }
}
