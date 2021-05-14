using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public int Money { get; set; } = 0;
    private GameManager gameManager;
    private AudioManager audioManager;

    void Start()
    {
        gameManager = GameManager.instance;
        audioManager = AudioManager.instance;
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
            audioManager.PlayWithRandomPitch("CollectiblePickup", 20, 70);
            Money += collectedMoney;
            gameManager.AddCollectedMoney(collectedMoney);
        }
    }
}
