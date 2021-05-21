using TMPro;
using UnityEngine;

/// <summary>
/// Script for displaying current level score
/// </summary>
public class ScoreTextController : MonoBehaviour
{
    private TMP_Text textMesh;
    private int displayedScore;
    GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.instance;
        textMesh = GetComponent<TMP_Text>();
        CheckScore();
        DisplayScore();
    }

    void Update()
    {
        CheckScore();
    }

    private void CheckScore()
    {
        int newScore = gameManager.ScoreCounter.CurrentSceneScore;
        if (newScore != displayedScore)
        {
            displayedScore = newScore;
            DisplayScore();
        }
    }

    private void DisplayScore()
    {
        textMesh.text = "score: " + displayedScore;
    }
}
