using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    private AudioManager audioManager;
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

    public bool IsProtected { get; set; } = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        audioManager = AudioManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(float points)
    {
        if (gameObject.CompareTag("Player"))
        {
            Animator playerAnimator = GetComponent<Animator>();
            if (!IsProtected)
            {
                audioManager.Play("PlayerHurt");
                playerAnimator.SetTrigger("PlayerHit");
                Health -= points;
            }
            else
            {                
                playerAnimator.SetTrigger("BlockHit");
                audioManager.Play("PlayerBlockHit");
            }
        }
        else
        {
            Health -= points;
        }
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

    public bool IsAlive()
    {
        return health > 0;
    }
}
