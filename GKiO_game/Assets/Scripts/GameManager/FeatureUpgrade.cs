using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatureUpgrade<T>
{
    private FeatureLevel<T>[] levels;
    int levelIndex = 0;

    public T CurrentFeatureValue
    {
        get => levels[levelIndex].FeatureValue;
    }

    public int UpgradeCost
    {
        get => levels[levelIndex + 1].Cost;
    }

    public bool IsUpgradePossible
    {
        get => levelIndex + 1 < levels.Length;
    }

    public FeatureUpgrade(FeatureLevel<T>[] levels)
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


}
