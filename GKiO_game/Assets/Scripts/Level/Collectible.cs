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

    /// <summary>
    /// Returns the value of the collectible item.
    /// </summary>
    public int Value {
        get => value;
    }

    /// <summary>
    /// Returns type of the collectible.
    /// </summary>
    public string TypeOfCollectible
    {
        get => typeOfCollectible;
    }

    /// <summary>
    /// Collects the item. It makes the item disappear.
    /// </summary>
    /// <returns>Value of the item.</returns>
    public int Collect()
    {
        Destroy(gameObject);
        return Value;
    }
}
