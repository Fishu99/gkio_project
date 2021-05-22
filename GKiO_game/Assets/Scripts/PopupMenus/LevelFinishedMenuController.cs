using TMPro;
using UnityEngine;

/// <summary>
/// A script for controlling the "Level finished" window.
/// </summary>
public class LevelFinishedMenuController : MonoBehaviour
{
    [SerializeField] private GameObject levelFinishedPanel;
    [SerializeField] private GameObject currentValue;
    [SerializeField] private GameObject totalScene;
    [SerializeField] private GameObject totalGame;
    private GameManager gameManager;
    private bool active = false;
    void Start()
    {
        gameManager = GameManager.instance;
        levelFinishedPanel.SetActive(false);
        active = false;
    }

    void Update()
    {
        if (!active && gameManager.IsLevelFinished)
        {
            Time.timeScale = 0;
            active = true;
            levelFinishedPanel.SetActive(true);
            ShowScore();
        }
    }

    /// <summary>
    /// Starts next level.
    /// </summary>
    public void NextLevel()
    {
        Time.timeScale = 1;
        gameManager.LoadWorkshopOrWinScene();
    }

    private void ShowScore()
    {
        currentValue.GetComponent<TMP_Text>().text = gameManager.ScoreCounter.CurrentSceneScore.ToString();
        totalScene.GetComponent<TMP_Text>().text = gameManager.ScoreCounter.TotalSceneScore.ToString();
        totalGame.GetComponent<TMP_Text>().text = gameManager.ScoreCounter.TotalGameScore.ToString();
    }

}
