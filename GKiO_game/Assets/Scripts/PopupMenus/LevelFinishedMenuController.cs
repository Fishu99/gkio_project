using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    // Update is called once per frame
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

    public void NextLevel()
    {
        Time.timeScale = 1;
        gameManager.LoadNextLevel();
    }

    private void ShowScore()
    {
        currentValue.GetComponent<TMP_Text>().text = gameManager.ScoreCounter.CurrentSceneScore.ToString();
        totalScene.GetComponent<TMP_Text>().text = gameManager.ScoreCounter.TotalSceneScore.ToString();
        totalGame.GetComponent<TMP_Text>().text = gameManager.ScoreCounter.TotalGameScore.ToString();
    }

}
