using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public static ShopUI shopUI;
    public Text bootPrice;
    public Text upgradedExplosionPrice;
    public Text extraDynamitePrice;
    public Text healthIncreasePrice;
    public Text baricadePrice;

    public Button extraDynamiteButton;
    public Button upgradedDynamiteButton;
    public Button bootsButton;
    public Button healthDynamiteButton;

    private void OnEnable()
    {
        if (ShopUI.shopUI == null)
        {
            ShopUI.shopUI = this;
        }
    }

    private void Start()
    {
        bootPrice.text = GameSettings.GS.bootPrice.ToString() + "g";
        upgradedExplosionPrice.text = GameSettings.GS.upgradedExplosionPrice.ToString() + "g  | " + GameSettings.GS.upgradedExplosionTimesBought + "/2";
        extraDynamitePrice.text = GameSettings.GS.extraDynamitePrice.ToString() + "g  | " + GameSettings.GS.extraDynamiteTimesBought + "/2";
        healthIncreasePrice.text = GameSettings.GS.healthIncreasePrice.ToString() + "g";
        baricadePrice.text = GameSettings.GS.healthIncreasePrice.ToString() + "g";
    }

    private void Update()
    {
        if(GameSettings.GS.extraDynamiteTimesBought >= 2)
        {
            extraDynamiteButton.interactable = false;
            extraDynamitePrice.text = "Out of Stock";
        }
        if (GameSettings.GS.upgradedExplosionTimesBought >= 2)
        {
            upgradedDynamiteButton.interactable = false;
            upgradedExplosionPrice.text = "Out of Stock";
        }
    }

}
