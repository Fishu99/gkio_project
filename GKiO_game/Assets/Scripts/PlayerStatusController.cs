using UnityEngine;

public class PlayerStatusController : MonoBehaviour
{

    GameManager gameManager;
    PlayerController playerController;
    HealthManager healthManager;
    MoneyManager moneyManager;
    SwordAttack swordAttack;
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
        gameManager = GameManager.instance;
        playerController = GetComponent<PlayerController>();
        healthManager = GetComponent<HealthManager>();
        moneyManager = GetComponent<MoneyManager>();
        swordAttack = GetComponent<SwordAttack>();
    }

    private void GetStatusFromGameManager()
    {
        healthManager.MaxHealth = gameManager.PlayerMaxHealth;
        healthManager.Health = gameManager.PlayerHealth;
        moneyManager.Money = gameManager.PlayerMoney;
        playerController.lives = gameManager.PlayerLives;
        swordAttack.swordDamage = gameManager.PlayerSwordDamage;
    }

    private void SetStatusInGameManager()
    {
        gameManager.PlayerHealth = healthManager.Health;
        gameManager.PlayerMoney = moneyManager.Money;
        gameManager.PlayerLives = playerController.lives;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager != null)
        {
            SetStatusInGameManager();
            /*
            if (playerController.HasFinishedLevel && !gameManager.IsLevelFinished)
            {
                Debug.Log("Detected level finish");
                gameManager.FinishLevel();
            }*/
        }
    }
    public void FinishLevel()
    {
        Debug.Log("Detected level finish");
        gameManager.FinishLevel();
    }
}
