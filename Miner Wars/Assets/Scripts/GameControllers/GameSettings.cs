using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    public static GameSettings GS;
    
    public Transform[] spawnPoints;
    public int[] scoreCount;
    public PlayerInfo[] players;
    public Slider healthBar;


    private void OnEnable() 
    {
        if (GameSettings.GS == null)
        {
            GameSettings.GS = this;
        }    
    }



}
