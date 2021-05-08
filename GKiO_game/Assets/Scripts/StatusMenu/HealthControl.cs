using TMPro;
using UnityEngine;

public class HealthControl : MonoBehaviour
{
    private TMP_Text healthTextMesh;
    private float displayedHealth;
    GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        healthTextMesh = GetComponent<TMP_Text>();
    }

    void Update()
    {
        CheckHealth();
    }

    void CheckHealth()
    {
        float newHealth = gameManager.PlayerHealth;
        if (newHealth != displayedHealth)
        {
            displayedHealth = newHealth;
            healthTextMesh.text = "Health: " + newHealth;
        }
    }


}
