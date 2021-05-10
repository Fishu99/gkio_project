using TMPro;
using UnityEngine;

public class MoneyControl : MonoBehaviour
{
    private TMP_Text textMesh;
    private int displayedMoney;
    GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.instance;
        textMesh = GetComponent<TMP_Text>();
        CheckMoney();
        DisplayMoney();
    }

    void Update()
    {
        CheckMoney();
    }

    private void CheckMoney()
    {
        int newMoney = gameManager.PlayerMoney;
        if (newMoney != displayedMoney)
        {
            displayedMoney = newMoney;
            DisplayMoney();
        }
    }

    private void DisplayMoney()
    {
        textMesh.text = "Money: " + displayedMoney;
    }



}
