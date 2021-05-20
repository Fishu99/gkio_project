using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinSceneController : MonoBehaviour
{
    [SerializeField] private GameObject scoreObject;
    private GameManager gameManager;
    void Start()
    {
        gameManager = GameManager.instance;
        scoreObject.GetComponent<TMP_Text>().text = gameManager.ScoreCounter.TotalGameScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MenuClick()
    {
        gameManager.LoadMainMenu();
    }
}
