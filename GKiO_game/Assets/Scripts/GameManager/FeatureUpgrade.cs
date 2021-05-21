/// <summary>
/// A class for managing upgrades of particular feature such as sword damage or bow force.
/// </summary>
public class FeatureUpgrade
{
    private FeatureLevel[] levels;
    int levelIndex = 0;

    public float CurrentFeatureValue
    {
        get => levels[levelIndex].FeatureValue;
    }

    public float NextFeatureValue
    {
        get => levels[levelIndex+1].FeatureValue;
    }

    public int UpgradeCost
    {
        get => levels[levelIndex + 1].Cost;
    }

    public bool IsUpgradePossible
    {
        get => levelIndex + 1 < levels.Length;
    }

    public FeatureUpgrade(FeatureLevel[] levels)
    {
        this.levels = levels;
    }

    public void Upgrade()
    {
        if(levelIndex + 1 < levels.Length)
        {
            levelIndex++;
        }
    }

    public void Reset()
    {
        levelIndex = 0;
    }


}

