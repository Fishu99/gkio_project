using UnityEngine;

/// <summary>
/// A script controlling the MainMenu scene
/// </summary>
public class MainMenuController : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] GameObject optionsMenu;
    void Start()
    {
        gameManager = GameManager.instance;
    }

    /// <summary>
    /// Starts new game
    /// </summary>
    public void Play()
    {
        gameManager.LoadDifficultySelect();
    }

    /// <summary>
    /// Shows options menu.
    /// </summary>
    public void Options()
    {
        optionsMenu.SetActive(true);
    }

    /// <summary>
    /// Quits the game.
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }
}
