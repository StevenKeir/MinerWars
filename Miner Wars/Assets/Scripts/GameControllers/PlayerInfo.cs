using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{

    public static PlayerInfo playerInfo;
    public int mySelectedCharacter;

    public GameObject[] allCharacters;

    public Sprite[] allSprites;
    public RuntimeAnimatorController[] animControllers;

    private void OnEnable()
    {
        //Creates the singleton
        if(PlayerInfo.playerInfo == null)
        {
            PlayerInfo.playerInfo = this;
        }
        else
        {
            if(PlayerInfo.playerInfo != this)
            {
                Destroy(PlayerInfo.playerInfo.gameObject);
                PlayerInfo.playerInfo = this;
            }
        }
        //Makes sure the object won't destroy on load, as it hold all the info we need on the next scene.
        DontDestroyOnLoad(this.gameObject);
    }

    //Holds the players info
    private void Start()
    {
        if (PlayerPrefs.HasKey("MyCharacter"))
        {
            mySelectedCharacter = PlayerPrefs.GetInt("MyCharacter");
        }
        else
        {
            mySelectedCharacter = 0;
            PlayerPrefs.GetInt("MyCharacter",mySelectedCharacter);
        }
    }



}
