using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script controlling the Workshop scene.
/// It creates the WorkshopPurchase objects which are used SimpleWorkshopItemControllers.
/// </summary>
public class WorkshopController : MonoBehaviour
{
    private GameManager gameManager;
    private Dictionary<string, WorkshopPurchase> purchases;

    void Start()
    {
        gameManager = GameManager.instance;
        CreatePurchases();
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

    private void CreatePurchases()
    {
        purchases = new Dictionary<string, WorkshopPurchase>
        {
            ["life"] = new WorkshopPurchase(OnBuyLife, 30),
            ["arrows"] = new WorkshopPurchase(OnBuyArrows, 10),
            ["bowDamageUpgrade"] = new WorkshopPurchase(OnBuyBowUpgrade),
            ["swordDamageUpgrade"] = new WorkshopPurchase(OnBuySwordUpgrade),
            ["bowForceUpgrade"] = new WorkshopPurchase(OnBuyBowForceUpgrade)
        };
        ConfigurePurchaseByFeature(purchases["bowDamageUpgrade"], gameManager.BowDamageUpgrade);
        ConfigurePurchaseByFeature(purchases["swordDamageUpgrade"], gameManager.SwordDamageUpgrade);
        ConfigurePurchaseByFeature(purchases["bowForceUpgrade"], gameManager.BowForceUpgrade);
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

    private void OnBuyBowForceUpgrade(WorkshopPurchase purchase)
    {
        OnUpgradePurchase(purchase, gameManager.BowForceUpgrade);
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
