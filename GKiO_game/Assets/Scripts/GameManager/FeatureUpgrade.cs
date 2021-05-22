/// <summary>
/// A class for managing upgrades of particular feature such as sword damage or bow force.
/// </summary>
public class FeatureUpgrade
{
    private FeatureLevel[] levels;
    private int levelIndex = 0;

    /// <summary>
    /// Current value of the feature.
    /// </summary>
    public float CurrentFeatureValue
    {
        get => levels[levelIndex].FeatureValue;
    }

    /// <summary>
    /// The next value of the feature.
    /// </summary>
    public float NextFeatureValue
    {
        get => levels[levelIndex+1].FeatureValue;
    }

    /// <summary>
    /// Cost of the next upgrade
    /// </summary>
    public int UpgradeCost
    {
        get => levels[levelIndex + 1].Cost;
    }

    /// <summary>
    /// Is true if an upgrade is possible, ie. the feature hasn't been fully upgraded yet.
    /// </summary>
    public bool IsUpgradePossible
    {
        get => levelIndex + 1 < levels.Length;
    }

    /// <summary>
    /// Construtor
    /// </summary>
    /// <param name="levels">Feature levels with their cost.</param>
    public FeatureUpgrade(FeatureLevel[] levels)
    {
        this.levels = levels;
    }

    /// <summary>
    /// Upgrades the feature.
    /// </summary>
    public void Upgrade()
    {
        if(levelIndex + 1 < levels.Length)
        {
            levelIndex++;
        }
    }

    /// <summary>
    /// Resets the upgrade level to its initial value.
    /// </summary>
    public void Reset()
    {
        levelIndex = 0;
    }


}

