using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeableWorkshopItemController : MonoBehaviour
{
    private GameManager gameManager;
    private FeatureUpgrade featureUpgrade;
    [SerializeField] private GameObject itemTitle;
    [SerializeField] private GameObject itemSubtitle;
    [SerializeField] private GameObject priceGroup;
    [SerializeField] private GameObject priceValue;
    [SerializeField] private GameObject textUnavailable;
    [SerializeField] private GameObject buyButton;
    private float previousValue;
    private int previousMoney;
    void Start()
    {
        gameManager = GameManager.instance;
        featureUpgrade = gameManager.SwordDamageUpgrade;
        previousMoney = gameManager.PlayerMoney;
        DisplayUpgradeData();
    }

    
    void Update()
    {
        if(gameManager.PlayerMoney != previousMoney)
        {
            previousMoney = gameManager.PlayerMoney;
            DisplayUpgradeData();
        }
    }

    private void DisplayUpgradeData()
    {
        if (featureUpgrade.IsUpgradePossible)
        {
            SetSubtitle("from " + featureUpgrade.CurrentFeatureValue + " to " + featureUpgrade.NextFeatureValue);
            SetPrice(featureUpgrade.UpgradeCost);
            CheckIfAffordable();
        }
        else
        {
            SetSubtitle("Fully upgraded to " + featureUpgrade.CurrentFeatureValue);
            SetUnavailableMessage("No more upgrades");
            ShowUnavailableMessage();
        }
    }

    void CheckIfAffordable()
    {
        bool affordable = featureUpgrade.UpgradeCost <= gameManager.PlayerMoney;
        buyButton.GetComponent<Button>().interactable = affordable;
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
        gameManager.BuyUpgrade(featureUpgrade);
        DisplayUpgradeData();
    }
}
