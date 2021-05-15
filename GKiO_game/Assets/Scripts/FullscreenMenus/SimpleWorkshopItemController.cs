using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SimpleWorkshopItemController : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private GameObject itemTitle;
    [SerializeField] private GameObject itemSubtitle;
    [SerializeField] private GameObject priceGroup;
    [SerializeField] private GameObject priceValue;
    [SerializeField] private GameObject textUnavailable;
    [SerializeField] private GameObject buyButton;
    [SerializeField] private WorkshopController workshopController;
    [SerializeField] private string purchaseName;
    private WorkshopPurchase purchase;
    private int previousMoney;
    void Start()
    {
        gameManager = GameManager.instance;
        purchase = workshopController.GetPurchaseByName(purchaseName);
        buyButton.GetComponent<Button>().onClick.AddListener(BuyItem);
        previousMoney = gameManager.PlayerMoney;
        RefreshItem();
    }


    void Update()
    {
        if (gameManager.PlayerMoney != previousMoney)
        {
            previousMoney = gameManager.PlayerMoney;
            RefreshItem();
        }
    }

    private void RefreshItem()
    {
        CheckIfAffordable();
        SetPrice(purchase.Price);
    }

    void CheckIfAffordable()
    {
        buyButton.GetComponent<Button>().interactable = purchase.IsAffordable();
    }

    private void SetSubtitle(string subtitle)
    {
        itemSubtitle.GetComponent<TMP_Text>().text = subtitle;
    }

    private void SetPrice(int price)
    {
        priceValue.GetComponent<TMP_Text>().text = price.ToString();
    }

    private void SetUnavailableMessage(string message)
    {
        textUnavailable.GetComponent<TMP_Text>().text = message;
    }

    private void ShowUnavailableMessage()
    {
        textUnavailable.SetActive(true);
        priceGroup.SetActive(false);
        buyButton.SetActive(false);
    }

    public void BuyItem()
    {
        purchase.Buy();
        RefreshItem();
    }
}
