using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkshopController : MonoBehaviour
{
    private GameManager gameManager;
    private Dictionary<string, WorkshopPurchase> purchases;
    void Start()
    {
        gameManager = GameManager.instance;
        CreatePurchases();
    }

    private void CreatePurchases()
    {
        purchases = new Dictionary<string, WorkshopPurchase>
        {
            ["life"] = new WorkshopPurchase(OnBuyLife, 10),
            ["arrows"] = new WorkshopPurchase(OnBuyArrows, 4),
            ["bowDamageUpgrade"] = new WorkshopPurchase(OnBuyBowUpgrade),
            ["swordDamageUpgrade"] = new WorkshopPurchase(OnBuySwordUpgrade)
        };
        ConfigurePurchaseByFeature(purchases["bowDamageUpgrade"], gameManager.BowDamageUpgrade);
        ConfigurePurchaseByFeature(purchases["swordDamageUpgrade"], gameManager.SwordDamageUpgrade);
    }


    public void Next()
    {
        gameManager.LoadNextLevel();
    }

    public WorkshopPurchase GetPurchaseByName(string name)
    {
        gameManager = GameManager.instance;
        if (purchases == null)
            CreatePurchases();
        return purchases[name];
    }


    private void OnBuyLife(WorkshopPurchase purchase)
    {
        gameManager.PlayerLives++;
    }

    private void OnBuyArrows(WorkshopPurchase purchase)
    {
        gameManager.PlayerArrows += 20;
    }

    private void OnBuyBowUpgrade(WorkshopPurchase purchase)
    {
        OnUpgradePurchase(purchase, gameManager.BowDamageUpgrade);
    }

    private void OnBuySwordUpgrade(WorkshopPurchase purchase)
    {
        OnUpgradePurchase(purchase, gameManager.SwordDamageUpgrade);
    }

    private void OnUpgradePurchase(WorkshopPurchase purchase, FeatureUpgrade featureUpgrade)
    {
        featureUpgrade.Upgrade();
        ConfigurePurchaseByFeature(purchase, featureUpgrade);
    }

    private void ConfigurePurchaseByFeature(WorkshopPurchase purchase, FeatureUpgrade featureUpgrade)
    {
        purchase.IsAvailable = featureUpgrade.IsUpgradePossible;
        if (featureUpgrade.IsUpgradePossible)
        {
            purchase.Subtitle = "from " + featureUpgrade.CurrentFeatureValue + " to " + featureUpgrade.NextFeatureValue;
            purchase.Price = featureUpgrade.UpgradeCost;
        }
        else
        {
            purchase.Subtitle = "Fully upgraded to " + featureUpgrade.CurrentFeatureValue;
            purchase.UnavailableReason = "No more upgrades";
        }
    }
    
}
