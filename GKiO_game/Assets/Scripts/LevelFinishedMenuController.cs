using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinishedMenuController : MonoBehaviour
{
    [SerializeField] private GameObject levelFinishedPanel;
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
        }
    }

    public void NextLevel()
    {
        Time.timeScale = 1;
        gameManager.LoadNextLevel();
    }

}
