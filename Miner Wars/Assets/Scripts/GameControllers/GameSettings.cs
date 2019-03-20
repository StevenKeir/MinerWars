using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    public static GameSettings GS;

    public Transform[] spawnPoints;
    public Slider healthBar;
    public GameObject pauseWindow;

    public bool pauseCheck;

    private void OnEnable() 
    {
        if (GameSettings.GS == null)
        {
            GameSettings.GS = this;
        }

        pauseCheck = false;
    }

    //private void Update()
    //{
    //    PauseMenu();
    //}

    //void PauseMenu()
    //{
    //    if (Input.GetKey(KeyCode.Escape))
    //    {
    //        pauseCheck = true;
    //    }
    //    if (Input.GetKey(KeyCode.Escape) && pauseCheck == true)
    //    {
    //        pauseCheck = false;
    //    }
    //    if(pauseCheck == true)
    //    {
    //        pauseWindow.gameObject.SetActive(true);
    //    }
    //    else
    //    {
    //        pauseWindow.gameObject.SetActive(false);
    //    }
    //}

}
