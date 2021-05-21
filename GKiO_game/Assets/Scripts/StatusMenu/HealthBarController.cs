using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script for displaying player's health on a health bar.
/// </summary>
public class HealthBarController : MonoBehaviour
{
    private Slider slider;
    private float displayedHealth;
    GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.instance;
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        CheckHealth();
    }

    void CheckHealth()
    {
        float newHealth = gameManager.PlayerHealth;
        float maxHealth = gameManager.PlayerMaxHealth;
        if (newHealth != displayedHealth)
        {
            displayedHealth = newHealth;
            slider.value = displayedHealth / maxHealth;
        }
    }
}
