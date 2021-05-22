using UnityEngine;

/// <summary>
/// Class for managing health of player and enemies.
/// Weapons such as swords, daggers and arrows check if they hit an object with a HealthManager.
/// If so, they reduce the health by using Damage method.
/// </summary>
public class HealthManager : MonoBehaviour
{
    private AudioManager audioManager;
    [SerializeField]
    private float maxHealth = 100f;
    /// <summary>
    /// Max health value if the character.
    /// </summary>
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
    /// <summary>
    /// Current health of the character. The health is always between 0 and MaxHealth.
    /// If a value outside this range is set, it will be clamped.
    /// </summary>
    public float Health {
        get => health;
        set {
            if (value > MaxHealth) health = MaxHealth;
            else if (value < 0) health = 0;
            else health = value;
        }
    }

    /// <summary>
    /// Allows to turn on protection of the player.
    /// </summary>
    public bool IsProtected { get; set; } = false;

    void Start()
    {
        audioManager = AudioManager.instance;
    }

    /// <summary>
    /// Reduces character's health.
    /// </summary>
    /// <param name="points">Number of health points to be subtracted from current Health value</param>
    public void Damage(float points)
    {
        if (gameObject.CompareTag("Player"))
        {
            DamagePlayer(points);
        }
        else
        {
            Health -= points;
        }
    }

    /// <summary>
    /// Increases character health.
    /// </summary>
    /// <param name="points">Number of health points to be added to current Health value</param>
    public void Restore(float points)
    {
        Health += points;
    }

    /// <summary>
    /// Sets Health to MaxHealth.
    /// </summary>
    public void SetMax()
    {
        health = maxHealth;
    }

    /// <summary>
    /// Sets Health to 0.
    /// </summary>
    public void SetZero()
    {
        health = 0;
    }

    /// <summary>
    /// Returns if the character is alive. Character is considered alive if Health is more than 0.
    /// </summary>
    /// <returns>true ih Health is greater than 0</returns>
    public bool IsAlive()
    {
        return health > 0;
    }

    private void DamagePlayer(float points)
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
}
