using UnityEngine;

/// <summary>
/// A script for the DifficultySelect scene
/// </summary>
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
