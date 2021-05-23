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

    /// <summary>
    /// Selects the easy mode and moves on to next scene.
    /// </summary>
    public void SelectEasy()
    {
        gameManager.SelectDifficultyAndGoNext(Difficulty.Easy);
    }

    /// <summary>
    /// Selects the medium mode and moves on to next scene.
    /// </summary>
    public void SelectMedium()
    {
        gameManager.SelectDifficultyAndGoNext(Difficulty.Medium);
    }

    /// <summary>
    /// Selects the hard mode and moves on to next scene.
    /// </summary>
    public void SelectHard()
    {
        gameManager.SelectDifficultyAndGoNext(Difficulty.Hard);
    }
}
