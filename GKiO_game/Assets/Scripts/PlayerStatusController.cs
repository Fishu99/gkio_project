using UnityEngine;

public class PlayerStatusController : MonoBehaviour
{

    GameManager gameManager;
    PlayerController playerController;
    HealthManager healthManager;
    void Start()
    {
        GetComponents();
        if (gameManager != null)
        {
            GetStatusFromGameManager();
        }
    }

    private void GetComponents()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerController = GetComponent<PlayerController>();
        healthManager = GetComponent<HealthManager>();
    }

    private void GetStatusFromGameManager()
    {
        healthManager.MaxHealth = gameManager.PlayerMaxHealth;
        healthManager.Health = gameManager.PlayerHealth;
        playerController.playerScore = gameManager.PlayerMoney;
        playerController.lives = gameManager.PlayerLives;
    }

    private void SetStatusInGameManager()
    {
        gameManager.PlayerHealth = healthManager.Health;
        gameManager.PlayerMoney = playerController.playerScore;
        gameManager.PlayerLives = playerController.lives;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager != null)
        {
            SetStatusInGameManager();
            if (playerController.HasFinishedLevel)
            {
                gameManager.FinishLevel();
            }
        }
    }
}
