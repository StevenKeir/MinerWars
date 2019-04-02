using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

    public Text text;
    public Text text2;
    public int textInt;
    public GameObject characterSelect;
    public GameObject mainButtons;
    public GameObject controlsDisplay;
    public GameObject waitingText;

    private void Start()
    {
        characterSelect.SetActive(false);
        controlsDisplay.SetActive(false);
        waitingText.SetActive(false);
    }

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
            text2.text = "Selected Character: Rob";
        }
        if (PlayerPrefs.GetInt("MyCharacter") == 1)
        {
            text.text = "Selected Character: Ted";
            text2.text = "Selected Character: Ted";
        }
        if (PlayerPrefs.GetInt("MyCharacter") == 2)
        {
            text.text = "Selected Character: Beth";
            text2.text = "Selected Character: Beth";
        }
        if (PlayerPrefs.GetInt("MyCharacter") == 3)
        {
            text.text = "Selected Character: Tina";
            text2.text = "Selected Character: Tina";
        }
    }

    public void CharacterSelectionButton ()
    {
        mainButtons.SetActive(false);
        characterSelect.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void ControlsButton()
    {
        mainButtons.SetActive(false);
        controlsDisplay.SetActive(true);
    }

    public void BackButton()
    {
        mainButtons.SetActive(true);
        characterSelect.SetActive(false);
        controlsDisplay.SetActive(false);
    }

}
