using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSettings : MonoBehaviour
{
    public static GameSettings GS;
    public Transform[] spawnPoints;



    public Slider healthBar;
    public TMP_Text text;
    public GameObject shopWindow;

    [Header("Local Players Reference")]
    public PlayerMovement localPlayer;
    public AvatarSetup localPlayerAvatar;
    public int bootPrice;
    public int upgradedExplosionPrice;
    public int extraDynamitePrice;
    public int healthIncreasePrice;
    public int baricadePrice;



    private void OnEnable()
    {
        if (GameSettings.GS == null)
        {
            GameSettings.GS = this;
        }
    }

    public void OnClickUpgradeDynamite()
    {
        if (localPlayerAvatar.myGoldCount >= upgradedExplosionPrice)
        {
            localPlayer.hasUpgradedExplosion = true;
            localPlayerAvatar.myGoldCount -= upgradedExplosionPrice;
        }
    }
    public void OnClickExtraDynamite()
    {
        if (localPlayerAvatar.myGoldCount >= extraDynamitePrice)
        {
            localPlayer.hasExtraDynamite = true;
            localPlayerAvatar.myGoldCount -= extraDynamitePrice;

        }
    }
    public void OnClickSpeedBoots()
    {
        if (localPlayerAvatar.myGoldCount >= bootPrice)
        {
            localPlayer.hasBoots = true;
            localPlayerAvatar.myGoldCount -= bootPrice;
        }
    }
    public void OnClickHealthIncrease()
    {
        if (localPlayerAvatar.myGoldCount >= healthIncreasePrice)
        {
            localPlayer.hasHealthIncrease = true;
            localPlayerAvatar.myGoldCount -= healthIncreasePrice;
        }
    }


}
