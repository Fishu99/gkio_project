using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultySelectController : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.instance;
    }

    void Update()
    {
        
    }

    public void SelectEasy()
    {
        gameManager.SelectDifficultyAndGoNext(Difficulty.Easy);
    }

    public void SelectMedium()
    {
        gameManager.SelectDifficultyAndGoNext(Difficulty.Medium);
    }

    public void SelectHard()
    {
        gameManager.SelectDifficultyAndGoNext(Difficulty.Hard);
    }
}
