using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings GS;
    
    public Transform[] spawnPoints;

    private void OnEnable() 
    {
        if (GameSettings.GS == null)
        {
            GameSettings.GS = this;
        }    
    }
}
