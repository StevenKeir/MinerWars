using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

    public Text text;
    public int textInt;

    public void OnClickCharacterPick(int whichCharacter)
    {
        if(PlayerInfo.playerInfo != null)
        {
            PlayerInfo.playerInfo.mySelectedCharacter = whichCharacter;
            PlayerPrefs.SetInt("MyCharacter",whichCharacter);
        }



    }

    private void Update()
    {
        TextChange();
    }

    public void TextChange()
    {
        if(PlayerPrefs.GetInt("MyCharacter") == 0)
        {
            text.text = "Selected Character: Rob";
        }
        if (PlayerPrefs.GetInt("MyCharacter") == 1)
        {
            text.text = "Selected Character: Ted";
        }
        if (PlayerPrefs.GetInt("MyCharacter") == 2)
        {
            text.text = "Selected Character: Beth";
        }
        if (PlayerPrefs.GetInt("MyCharacter") == 3)
        {
            text.text = "Selected Character: Tina";
        }
    }



}
