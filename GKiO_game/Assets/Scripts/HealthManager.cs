using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100f;
    public float MaxHealth {
        get => maxHealth;
        set {
            maxHealth = value;
            if (health > maxHealth)
                health = maxHealth;
        }
    } 

    [SerializeField]
    private float health = 100f;
    public float Health {
        get => health;
        set {
            if (value > MaxHealth) health = MaxHealth;
            else if (value < 0) health = 0;
            else health = value;
        }
    }
    
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(float points)
    {
        Health -= points;
    }

    public void Restore(float points)
    {
        Health += points;
    }

    public void SetMax()
    {
        health = maxHealth;
    }

    public void SetZero()
    {
        health = 0;
    }
}
