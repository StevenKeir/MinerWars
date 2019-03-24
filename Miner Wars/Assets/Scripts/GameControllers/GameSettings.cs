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

    private void OnEnable() 
    {
        if (GameSettings.GS == null)
        {
            GameSettings.GS = this;
        }

       
    }



}
