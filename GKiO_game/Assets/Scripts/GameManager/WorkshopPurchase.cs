using System;

/// <summary>
/// A class representing an item in the Workshop scene.
/// </summary>
public class WorkshopPurchase 
{
    public int Price { get; set; }
    public string Subtitle { get; set; }
    public string UnavailableReason { get; set; } 
    public bool IsAvailable { get; set; } = true;

    public Action<WorkshopPurchase> BuyAction { get; set; }

    public WorkshopPurchase() { }
    public WorkshopPurchase(Action<WorkshopPurchase> buyAction)
    {
        BuyAction = buyAction;
    }
    public WorkshopPurchase(Action<WorkshopPurchase> buyAction, int price) : this(buyAction)
    {
        Price = price;
    }

    public void Buy()
    {
        if (IsAffordable())
        {
            GameManager.instance.PlayerMoney -= Price;
            BuyAction(this);
        }
    }

    public bool IsAffordable()
    {
        return Price <= GameManager.instance.PlayerMoney;
    }
}
