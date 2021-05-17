using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public int Money { get; set; } = 0;
    private GameManager gameManager;
    private AudioManager audioManager;
    private ParticleSystem particle;

    void Start()
    {
        gameManager = GameManager.instance;
        audioManager = AudioManager.instance;
        particle = GetComponent<ParticleSystem>();
        particle.Stop();
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
            ParticleSystem.MainModule particleMain = particle.main;
            if (collectible.TypeOfCollectible.Equals("Coin"))
            {
                particleMain.startColor = Color.yellow;
                //particle.startColor = Color.yellow;
            }
            else if (collectible.TypeOfCollectible.Equals("Diamond"))
            {
                particleMain.startColor = Color.cyan;
                //particle.startColor = Color.cyan;
            }
            particle.Play();
            audioManager.PlayWithRandomPitch("CollectiblePickup", 20, 70);
            Money += collectedMoney;
            gameManager.AddCollectedMoney(collectedMoney);
        }
    }
}
