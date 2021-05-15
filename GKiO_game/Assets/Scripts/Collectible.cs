using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private int value;
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
