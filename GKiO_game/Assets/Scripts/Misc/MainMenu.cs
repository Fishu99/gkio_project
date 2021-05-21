using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.instance;
    }

    public void StartGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        gameManager.LoadFirstLevel();
    }

    public void ExitGame()
    {
        Debug.Log("Exit!");
        Application.Quit();
    }
}
