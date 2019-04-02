using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public Text bootPrice;
    public Text upgradedExplosionPrice;
    public Text extraDynamitePrice;
    public Text healthIncreasePrice;
    public Text baricadePrice;

    private void Start()
    {
        bootPrice.text = GameSettings.GS.bootPrice.ToString() + "g";
        upgradedExplosionPrice.text = GameSettings.GS.upgradedExplosionPrice.ToString() + "g";
        extraDynamitePrice.text = GameSettings.GS.extraDynamitePrice.ToString() + "g";
        healthIncreasePrice.text = GameSettings.GS.healthIncreasePrice.ToString() + "g";
        baricadePrice.text = GameSettings.GS.healthIncreasePrice.ToString() + "g";
    }



}
