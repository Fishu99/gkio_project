using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    private GameManager gameManager;
    void Start()
    {
        gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        gameManager.LoadDifficultySelect();
    }

    public void Options()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }
}
