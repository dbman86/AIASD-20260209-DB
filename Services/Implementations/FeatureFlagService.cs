using Microsoft.Extensions.Options;
using PostHubAPI.Models;
using PostHubAPI.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace PostHubAPI.Services.Implementations;

/// <summary>
/// Implementation of feature flag service
/// </summary>
public class FeatureFlagService : IFeatureFlagService
{
    private readonly FeatureFlagOptions _options;
    private readonly ILogger<FeatureFlagService> _logger;

    public FeatureFlagService(IOptions<FeatureFlagOptions> options, ILogger<FeatureFlagService> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public bool IsEnabled(string featureName, string? userId = null)
    {
        var flag = GetFeatureFlag(featureName);
        if (flag == null)
        {
            _logger.LogWarning("Feature flag '{FeatureName}' not found. Returning false.", featureName);
            return false;
        }

        // Check if user is in allowed list (override)
        if (!string.IsNullOrEmpty(userId) && flag.AllowedUsers?.Contains(userId) == true)
        {
            _logger.LogDebug("Feature '{FeatureName}' enabled for user '{UserId}' (allowed list)", featureName, userId);
            return true;
        }

        // If feature is not enabled globally, return false
        if (!flag.Enabled)
        {
            return false;
        }

        // Check rollout percentage
        if (flag.RolloutPercentage.HasValue)
        {
            var userHash = GetUserHash(featureName, userId);
            var isInRollout = userHash <= flag.RolloutPercentage.Value;
            
            _logger.LogDebug(
                "Feature '{FeatureName}' rollout check for user '{UserId}': {IsInRollout} (hash: {Hash}, threshold: {Threshold})",
                featureName, userId, isInRollout, userHash, flag.RolloutPercentage.Value);
            
            return isInRollout;
        }

        return true;
    }

    public string? GetVariant(string featureName, string? userId = null)
    {
        var flag = GetFeatureFlag(featureName);
        if (flag == null || !flag.Enabled || flag.Variants == null || flag.Variants.Count == 0)
        {
            return null;
        }

        // If not in rollout, return null
        if (!IsEnabled(featureName, userId))
        {
            return null;
        }

        // Consistently assign user to a variant based on hash
        var userHash = GetUserHash(featureName, userId);
        var variantIndex = userHash % flag.Variants.Count;
        var variant = flag.Variants[variantIndex];

        _logger.LogDebug(
            "Feature '{FeatureName}' assigned variant '{Variant}' to user '{UserId}'",
            featureName, variant, userId);

        return variant;
    }

    public Dictionary<string, bool> GetAllFlags(string? userId = null)
    {
        var result = new Dictionary<string, bool>();
        
        foreach (var flag in _options.Flags)
        {
            result[flag.Name] = IsEnabled(flag.Name, userId);
        }

        return result;
    }

    public bool FeatureExists(string featureName)
    {
        return GetFeatureFlag(featureName) != null;
    }

    private FeatureFlag? GetFeatureFlag(string featureName)
    {
        return _options.Flags.FirstOrDefault(f => 
            f.Name.Equals(featureName, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Generate a consistent hash for a user and feature combination (0-99)
    /// This ensures users get the same experience across requests
    /// </summary>
    private int GetUserHash(string featureName, string? userId)
    {
        // Use a combination of feature name and userId for consistent hashing
        var input = $"{featureName}:{userId ?? "anonymous"}";
        var hashBytes = MD5.HashData(Encoding.UTF8.GetBytes(input));
        
        // Convert first 4 bytes to int and mod by 100 for percentage
        var hashValue = Math.Abs(BitConverter.ToInt32(hashBytes, 0));
        return hashValue % 100;
    }
}
