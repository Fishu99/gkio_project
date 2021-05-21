using UnityEngine;

/// <summary>
/// A script controlling the "Game Over" window.
/// </summary>
public class GameOverMenuController : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    private GameManager gameManager;
    private bool active = false;
    void Start()
    {
        gameManager = GameManager.instance;
        gameOverPanel.SetActive(false);
        active = false;
    }

    void Update()
    {
        if(!active && gameManager.IsGameOver)
        {
            active = true;
            gameOverPanel.SetActive(true);
        }
        if(active && !gameManager.IsGameOver)
        {
            active = false;
            gameOverPanel.SetActive(false);
        }
    }

    public void Restart()
    {
        gameManager.LoadFirstLevel();
    }

    public void BackToMenu()
    {
        gameManager.LoadMainMenu();
    }
}
