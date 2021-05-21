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

    public void Play()
    {
        gameManager.LoadDifficultySelect();
    }

    public void Options()
    {
        optionsMenu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
