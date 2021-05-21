/// <summary>
/// Data class representing an upgradeable feature level.
/// </summary>
public class FeatureLevel
{
    /// <summary>
    /// The value of the feature such as weapon damage, speed, etc.
    /// </summary>
    public float FeatureValue { get; set; }
    /// <summary>
    /// The money player must spend to get the level of feature.
    /// </summary>
    public int Cost { get; set; }
}
