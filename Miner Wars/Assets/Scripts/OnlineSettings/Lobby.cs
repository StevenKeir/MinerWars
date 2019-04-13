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


    void CreateRoom()
    {

        
        Debug.Log("Trying to create room");
        int randRoomName = Random.Range(0, 1000);
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)MultiplayerSetting.multiplayerSetting.maxPlayers };
        PhotonNetwork.CreateRoom("Room " + randRoomName, roomOptions);

        //Sets the RoomInfo
        roomInfo.numberMaxPlayers = MultiplayerSetting.multiplayerSetting.maxPlayers;
        roomInfo.roomName = ("Room " + randRoomName).ToString();
        //roomInfo.roomNumber = randRoomName;
        

        

    }



    public override void OnJoinedRoom()
    {
        Debug.Log("Room joined");
        //base.OnJoinedRoom();
        //cancelButton.SetActive(false);
        //offlineButton.SetActive(false);
        //currentRoomButton.SetActive(true);
        //leaveRoomButton.SetActive(true);
        //roomText.text = PhotonNetwork.CurrentRoom.Name.ToString();
    }

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
