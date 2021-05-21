using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script for collectible items such as coins, diamonds and sacks.
/// </summary>
public class Collectible : MonoBehaviour
{
    [Tooltip("Value of the item")]
    [SerializeField] private int value;
    [Tooltip("Type of the item")]
    [SerializeField] private string typeOfCollectible;

    public int Value {
        get => value;
    }

    public string TypeOfCollectible
    {
        get => typeOfCollectible;
    }

    public int Collect()
    {
        Destroy(gameObject);
        return Value;
    }
}
