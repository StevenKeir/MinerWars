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
    }

    private void Start()
    {
        UpdateTimerUI();
    }
    private void Update()
    {
        if (startTimer)
        {

            gamelength -= Time.deltaTime;
            timerText.text = ((int)gamelength).ToString();
        }
    }

    private void UpdateTimerUI()
    {
        if (PhotonNetwork.IsMasterClient)
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
            upgradedExplosionTimesBought++;
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

        }
    }
    public void OnClickSpeedBoots()
    {
        if (localPlayerAvatar.myGoldCount >= bootPrice)
        {
            localPlayer.hasBoots = true;
            localPlayerAvatar.myGoldCount -= bootPrice;
            text.text = "Gold: " + localPlayerAvatar.myGoldCount;

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

    [PunRPC]
    void RPC_SendTimerUpdate()
    {
        this.startTimer = true;
    }
}
