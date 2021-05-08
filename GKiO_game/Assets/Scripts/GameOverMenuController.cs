using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenuController : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    private GameManager gameManager;
    private bool active = false;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameOverPanel.SetActive(false);
        active = false;
    }

    // Update is called once per frame
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
        gameManager.ReturnToMainMenu();
    }
}
