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
            ["life"] = new WorkshopPurchase(10, OnBuyLife),
            ["arrows"] = new WorkshopPurchase(4, OnBuyArrows)
        };
    }


    public void Next()
    {
        gameManager.LoadNextLevel();
    }

    public WorkshopPurchase GetPurchaseByName(string name)
    {
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
    
}
