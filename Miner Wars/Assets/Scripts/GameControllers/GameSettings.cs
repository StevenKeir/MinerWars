using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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
    //public float startTimerTime;
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
    public Image barricade;
    Image alphaChannel;
    public Image TNT2FillImage;
    public Image TNT3FillImage;

    public bool hitCheckTest = false;
    [Header("All Players in scene")]
    public List<AvatarSetup> players = new List<AvatarSetup>();


    private void OnEnable()
    {
        //Setting a singleton
        if (GameSettings.GS == null)
        {
            GameSettings.GS = this;
        }
    }

    private void Awake()
    {
        //Setting the instance of the PhotonView
        PV = GetComponent<PhotonView>();
        //Setting All the alphas of the UI images to 0 aka invisible..
        TNT2.color = new Color(TNT2.color.r, TNT2.color.g, TNT2.color.b, 0f);
        TNT3.color = new Color(TNT3.color.r, TNT3.color.g, TNT3.color.b, 0f);
        speedBoots.color = new Color(speedBoots.color.r, speedBoots.color.g, speedBoots.color.b, 0f);
        upgradedExplosion.color = new Color(upgradedExplosion.color.r, upgradedExplosion.color.g, upgradedExplosion.color.b, 0f);
        barricade.color = new Color(barricade.color.r, barricade.color.g, barricade.color.b, 0f);
    }

    private void Update()
    {
        //Endgame function plays once the game has ended, also we have a timer for the countdown.
        EndGame();
        if (startTimer)
        {
            gamelength -= Time.deltaTime;
            timerText.text = ((int)gamelength).ToString();
        }
    }

    private void UpdateTimerUI()
    {
        //Telling all other clients to start timer, This is more for appoximate time since there is a delay, will later implement a true synced timer by taking the PhotonNetwork.Time And calulating the different and making them the same between clients. but have other focuses at the moment
        if (PhotonNetwork.IsMasterClient && GameSettings.GS.isGameRunning == true && ScoreCounter.SC.dontCount == false)
        {
            PV.RPC("RPC_SendTimerUpdate", RpcTarget.AllBuffered);
        }
    }

    public void OnClickUpgradeDynamite()
    {
        //Playing the function when UI button is clicked and only if the player has the gold.
        if (localPlayerAvatar.myGoldCount >= upgradedExplosionPrice)
        {
            localPlayer.hasUpgradedExplosion = true;
            localPlayerAvatar.myGoldCount -= upgradedExplosionPrice;
            text.text = "Gold: " + localPlayerAvatar.myGoldCount;
            ShopUI.shopUI.upgradedDynamiteButton.interactable = false;
            ShopUI.shopUI.upgradedExplosionPrice.text = "Out of Stock";
        }
    }

    public void OnClickExtraDynamite()
    {
        //Playing the function when UI button is clicked and only if the player has the gold.
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
        //Playing the function when UI button is clicked and only if the player has the gold.
        if (localPlayerAvatar.myGoldCount >= bootPrice)
        {
            localPlayer.hasBoots = true;
            localPlayerAvatar.myGoldCount -= bootPrice;
            text.text = "Gold: " + localPlayerAvatar.myGoldCount;
            ShopUI.shopUI.bootsButton.interactable = false;
            ShopUI.shopUI.bootPrice.text = "Out of Stock";
        }
    } 

    public void OnClickBarricadeUpgrade()
    {
        //Playing the function when UI button is clicked and only if the player has the gold.
        if (localPlayerAvatar.myGoldCount >= baricadePrice)
        {
            localPlayer.hasBaricade = true;
            localPlayerAvatar.myGoldCount -= baricadePrice;
            text.text = "Gold: " + localPlayerAvatar.myGoldCount;
            ShopUI.shopUI.barricadeButton.interactable = false;
            ShopUI.shopUI.baricadePrice.text = "Out of Stock";
        }
    }

    public void DisconnectPlayer()
    {
        //Disconnects the player from the current game, deleting all old instances of DontDestroyOnLoad objects, this will be done by photon anyway but doing it myself just incase.
        StartCoroutine(DisconnectAndLoad());
    }

    IEnumerator DisconnectAndLoad()
    {
        //Leaves current room
        PhotonNetwork.LeaveRoom();
        //Only Does this if not in a room.
        while (!PhotonNetwork.InRoom)
        {
            //Deleting all old instances of DontDestroyOnLoad objects and loads main menu
            ScoreCounter.SC.dontCount = true;
            Destroy(MultiplayerSetting.multiplayerSetting.gameObject);
            yield return new WaitForSeconds(0.1f);
            Destroy(RoomController.room.gameObject);
            yield return new WaitForSeconds(0.1f);
            Destroy(PlayerInfo.playerInfo.gameObject);
            yield return new WaitForSeconds(0.1f);
            SceneManager.LoadScene(0);
            yield return null;
            
        }
    }
    void EndGame()
    {
        //Function to play when the game has concluded, this is done so can determine the winner.
        if (PhotonNetwork.IsMasterClient)
        {
            if(gamelength <= 0f)
            {
                PV.RPC("RPC_GameEnded", RpcTarget.AllBuffered);
            }
            else if(gameEnded == true)
            {
                PV.RPC("RPC_GameEnded", RpcTarget.AllBuffered);
            }
        }
    }



    [PunRPC]
    void RPC_GameEnded()
    {
        //Lets scoreCounter.cs know and other scripts that the game has ended continuing to result screen.
        gameEnded = true;
        startTimer = false;
        ScoreCounter.SC.endGamePanel.SetActive(true);
    }
}
