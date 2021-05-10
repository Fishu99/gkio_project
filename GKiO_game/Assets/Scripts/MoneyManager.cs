using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public int Money { get; set; } = 0;
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Collectible collectible = other.GetComponent<Collectible>();
        if(collectible != null)
        {
            int collectedMoney = collectible.Collect();
            Money += collectedMoney;
            gameManager.AddCollectedMoney(collectedMoney);
        }
    }
}
