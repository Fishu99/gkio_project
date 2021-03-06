using UnityEngine;

/// <summary>
/// A class which allows player to collect valuable objects, such as coins and diamonds.
/// </summary>
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

    private void OnTriggerEnter(Collider other)
    {
        Collectible collectible = other.GetComponent<Collectible>();
        if(collectible != null)
        {

            int collectedMoney = collectible.Collect();
            ParticleSystem.MainModule particleMain = particle.main;
            if (collectible.TypeOfCollectible.Equals("Coin") || collectible.TypeOfCollectible.Equals("Sack"))
            {
                particleMain.startColor = Color.yellow;
            }
            else if (collectible.TypeOfCollectible.Equals("Diamond"))
            {
                particleMain.startColor = Color.cyan;
            }
            particle.Play();
            audioManager.PlayWithRandomPitch("CollectiblePickup", 20, 70);
            Money += collectedMoney;
            gameManager.AddCollectedMoney(collectedMoney);
        }
    }
}
