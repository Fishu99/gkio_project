using System;
using UnityEngine;

public class WorkshopPurchase 
{
    public int Price { get; set; }
    public Action<WorkshopPurchase> BuyAction { get; set; }

    public WorkshopPurchase() { }
    public WorkshopPurchase(int price, Action<WorkshopPurchase> buyAction)
    {
        Price = price;
        BuyAction = buyAction;
    }

    public void Buy()
    {
        if (IsAffordable())
        {
            GameManager.instance.PlayerMoney -= Price;
        }
        BuyAction(this);
    }

    public bool IsAffordable()
    {
        return Price <= GameManager.instance.PlayerMoney;
    }
}
