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
    private PhotonView PV;
    public bool isGameRunning = false;
    public GameObject loadingScreen;

    public Slider healthBar;
    public TMP_Text text;
    public GameObject shopWindow;

    [Header("Local Players Reference")]
    public PlayerMovement localPlayer;
    public AvatarSetup localPlayerAvatar;

    [Header("Shop Prices")]
    public int bootPrice;
    public float bootSpeedIncrease;
    public int upgradedExplosionPrice;
    public int extraDynamitePrice;
    public int healthIncreasePrice;
    public int baricadePrice;

    [Header("Times purchased")]
    public int extraDynamiteTimesBought;
    public int upgradedExplosionTimesBought;

    [Header("Timer Settings")]
    public float startTimerTime;
    public float gamelength;
    public TMP_Text timerText;
    public bool startTimer = false;
    public bool gameEnded = false;

    [Header("Item bar settings")]
    public Image TNT1;
    public Image TNT2;
    public Image TNT3;
    public Image speedBoots;
    public Image upgradedExplosion;
    Image alphaChannel;
    public Image TNT2FillImage;
    public Image TNT3FillImage;


    private void OnEnable()
    {
        if (GameSettings.GS == null)
        {
            GameSettings.GS = this;
        }
    }

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        //UpdateTimerUI();
        startTimerTime = gamelength;
        TNT2.color = new Color(TNT2.color.r, TNT2.color.g, TNT2.color.b, 0f);
        TNT3.color = new Color(TNT3.color.r, TNT3.color.g, TNT3.color.b, 0f);
        speedBoots.color = new Color(speedBoots.color.r, speedBoots.color.g, speedBoots.color.b, 0f);
        upgradedExplosion.color = new Color(upgradedExplosion.color.r, upgradedExplosion.color.g, upgradedExplosion.color.b, 0f);


    }

    private void Start()
    {
        //UpdateTimerUI();
    }
    private void Update()
    {
        UpdateTimerUI();
        EndGame();
        if (startTimer)
        {
            gamelength -= Time.deltaTime;
            timerText.text = ((int)gamelength).ToString();
        }
    }

    private void UpdateTimerUI()
    {
        if (PhotonNetwork.IsMasterClient && GameSettings.GS.isGameRunning == true && GameSettings.GS.gameEnded == false)
        {
            PV.RPC("RPC_SendTimerUpdate", RpcTarget.AllBuffered);
        }
    }

    public void OnClickUpgradeDynamite()
    {
        if (localPlayerAvatar.myGoldCount >= upgradedExplosionPrice)
        {
            localPlayer.hasUpgradedExplosion = true;
            localPlayerAvatar.myGoldCount -= upgradedExplosionPrice;
            text.text = "Gold: " + localPlayerAvatar.myGoldCount;
            ShopUI.shopUI.upgradedDynamiteButton.interactable = false;
            ShopUI.shopUI.upgradedExplosionPrice.text = "Out of Stock";

            //upgradedExplosionTimesBought++;
            // ShopUI.shopUI.upgradedExplosionPrice.text = upgradedExplosionPrice.ToString() + "g  | " + upgradedExplosionTimesBought + "/2";
        }
    }
    public void OnClickExtraDynamite()
    {
        if (localPlayerAvatar.myGoldCount >= extraDynamitePrice)
        {
            localPlayer.hasExtraDynamite = true;
            localPlayerAvatar.myGoldCount -= extraDynamitePrice;
            text.text = "Gold: " + localPlayerAvatar.myGoldCount;
            extraDynamiteTimesBought++;
            ShopUI.shopUI.extraDynamitePrice.text = extraDynamitePrice.ToString() + "g  | " + extraDynamiteTimesBought + "/2";
        }
    }
    public void OnClickSpeedBoots()
    {
        if (localPlayerAvatar.myGoldCount >= bootPrice)
        {
            localPlayer.hasBoots = true;
            localPlayerAvatar.myGoldCount -= bootPrice;
            text.text = "Gold: " + localPlayerAvatar.myGoldCount;
            ShopUI.shopUI.bootsButton.interactable = false;
            ShopUI.shopUI.bootPrice.text = "Out of Stock";
        }
    }
    public void OnClickHealthIncrease()
    {
        if (localPlayerAvatar.myGoldCount >= healthIncreasePrice)
        {
            localPlayer.hasHealthIncrease = true;
            localPlayerAvatar.myGoldCount -= healthIncreasePrice;
            text.text = "Gold: " + localPlayerAvatar.myGoldCount;
        }
    }

    void EndGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if(gamelength <= 0f)
            {
                PV.RPC("RPC_GameEnded", RpcTarget.AllBuffered);
            }
        }
    }

    [PunRPC]
    void RPC_SendTimerUpdate()
    {
        this.startTimer = true;
        GameSettings.GS.isGameRunning = true;
        loadingScreen.SetActive(false);
    }

    [PunRPC]
    void RPC_GameEnded()
    {
        gameEnded = true;
        this.startTimer = false;
        ScoreCounter.SC.endGamePanel.SetActive(true);
    }
}
