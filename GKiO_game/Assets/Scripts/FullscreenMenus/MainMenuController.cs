using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
