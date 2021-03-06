using UnityEngine;

/// <summary>
/// The script reads the player status from the GameManager at the beginning of each level
/// and updates the status in GameManager during game.
/// </summary>
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

    void Update()
    {
        if (gameManager != null)
        {
            SetStatusInGameManager();
        }
    }

    public void FinishLevel()
    {
        Debug.Log("Detected level finish");
        gameManager.FinishLevel();
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
        playerController.arrowDamage = gameManager.BowArrowDamage;
        playerController.arrows = gameManager.PlayerArrows;
        playerController.arrowForce = gameManager.BowArrowForce;
    }

    private void SetStatusInGameManager()
    {
        gameManager.PlayerHealth = healthManager.Health;
        gameManager.PlayerMoney = moneyManager.Money;
        gameManager.PlayerLives = playerController.lives;
        gameManager.PlayerArrows = playerController.arrows;
    }
}
