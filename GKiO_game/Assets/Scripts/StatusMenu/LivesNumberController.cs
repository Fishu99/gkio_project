using TMPro;
using UnityEngine;

/// <summary>
/// Script for displaying the number of player lives.
/// </summary>
public class LivesNumberController : MonoBehaviour
{
    private TMP_Text textMesh;
    private int displayedLives;
    GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.instance;
        textMesh = GetComponent<TMP_Text>();
    }

    void Update()
    {
        CheckLives();
    }

    void CheckLives()
    {
        int newLives = gameManager.PlayerLives;
        if (newLives != displayedLives)
        {
            displayedLives = newLives;
            textMesh.text = newLives.ToString();
        }
    }
}
