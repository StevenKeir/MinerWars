using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    public static GameSettings GS;

    public Transform[] spawnPoints;
    public List<int> players = new List<int>();
    public int[] scoreCount;

    public int[] gold;
    


    public Slider healthBar;


    private void OnEnable() 
    {
        if (GameSettings.GS == null)
        {
            GameSettings.GS = this;
        }    
    }

    private void Start()
    {
        int players = MultiplayerSetting.multiplayerSetting.maxPlayers;
        gold = new int[players];
    }

    [PunRPC]
    void RPC_UpdateGold()
    {

    }
}
