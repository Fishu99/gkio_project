using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private int value;
    public int Value {
        get => value;
    }

    public int Collect()
    {
        Destroy(gameObject);
        return Value;
    }
}
