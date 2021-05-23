using UnityEngine;
using TMPro;

/// <summary>
/// A script controlling the scene shown after player wins the game.
/// </summary>
public class WinSceneController : MonoBehaviour
{
    [SerializeField] private GameObject scoreObject;
    private GameManager gameManager;
    void Start()
    {
        gameManager = GameManager.instance;
        scoreObject.GetComponent<TMP_Text>().text = gameManager.ScoreCounter.TotalGameScore.ToString();
    }

    /// <summary>
    /// Returns to main menu.
    /// </summary>
    public void MenuClick()
    {
        gameManager.LoadMainMenu();
    }
}
