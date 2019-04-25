using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Lobby : MonoBehaviourPunCallbacks
{
    private PhotonView PV;
    public static Lobby lobby;
    public AllRoomInfo roomInfo;

    public GameObject joinButton;
    public GameObject cancelButton;
    public GameObject offlineButton;
    public GameObject currentRoomButton;
    public GameObject leaveRoomButton;
    public GameObject menuStuff;
    public GameObject waitingText;
    public Text roomText;

    private void Awake()
    {
        lobby = this;
        PV = GetComponent<PhotonView>();
    }

    //Checks if we are not connected if so trys to connect to the photon server, this was meant to be in awake but as i found it doesn't connect when you disconnect from a game inprogress.
    public void LateUpdate()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("player has connected");
        joinButton.SetActive(true);
        PhotonNetwork.AutomaticallySyncScene = true;
    }



    public void OnJoinButtonClick()
    {
        joinButton.SetActive(false);
        cancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Join button was clicked");
        waitingText.SetActive(true);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Player failed to join random room");
        CreateRoom();
    }

    //Creates a room for the player to join with the set settings.
    void CreateRoom()
    {
        Debug.Log("Trying to create room");
        int randRoomName = Random.Range(0, 1000);
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)MultiplayerSetting.multiplayerSetting.maxPlayers };
        PhotonNetwork.CreateRoom("Room " + randRoomName, roomOptions);
   
        //Sets the RoomInfo
        roomInfo.numberMaxPlayers = MultiplayerSetting.multiplayerSetting.maxPlayers;
        roomInfo.roomName = ("Room " + randRoomName).ToString();
    }


    
    public override void OnJoinedRoom()
    {
        Debug.Log("Room joined");
    }

    //If we fail to create a room, retry to create the room
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create a new room but failed, must be one of that name existing");
        CreateRoom();
    }

    public void OnCancelButtonClicked()
    {
        Debug.Log("Cancel button clicked");
        joinButton.SetActive(true);
        cancelButton.SetActive(false);
        PhotonNetwork.LeaveRoom();
        waitingText.SetActive(false);
    }

    public void OnLeaveRoomClick()
    {
        PhotonNetwork.LeaveRoom();
        currentRoomButton.SetActive(false);
        leaveRoomButton.SetActive(false);
        joinButton.SetActive(true);
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Player left room");

    }

}
