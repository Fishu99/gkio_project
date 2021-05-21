using TMPro;
using UnityEngine;

/// <summary>
/// Scirpt for displaying the number of arrows.
/// </summary>
public class ArrowControl : MonoBehaviour
{
    private TMP_Text textMesh;
    private int displayedArrows;
    GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.instance;
        textMesh = GetComponent<TMP_Text>();
        CheckArrows();
        DisplayArrows();
    }

    void Update()
    {
        CheckArrows();
    }

    private void CheckArrows()
    {
        int newArrows = gameManager.PlayerArrows;
        if (newArrows != displayedArrows)
        {
            displayedArrows = newArrows;
            DisplayArrows();
        }
    }

    private void DisplayArrows()
    {
        textMesh.text = displayedArrows.ToString();
    }



}
