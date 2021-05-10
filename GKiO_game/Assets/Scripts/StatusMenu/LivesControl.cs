using TMPro;
using UnityEngine;

public class LivesControl : MonoBehaviour
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
            textMesh.text = "Lives: " + newLives;
        }
    }


}
