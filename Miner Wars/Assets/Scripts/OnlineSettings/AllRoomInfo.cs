using UnityEngine;
using Photon.Pun;

public class AllRoomInfo : MonoBehaviourPunCallbacks
{
    PhotonView PV;

    public string roomName;
    //public int roomNumber;
    public int numberMaxPlayers;
    public int currentPlayers;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        PV = GetComponent<PhotonView>();
    }
    private void Update()
    {
        CurrentPlayerUpdate();
    }

    public override void OnJoinedRoom()
    {
        
        if (!PhotonNetwork.IsMasterClient)
        {
            numberMaxPlayers = MultiplayerSetting.multiplayerSetting.maxPlayers;
            roomName = PhotonNetwork.CurrentRoom.Name;
        }
    }

    void CurrentPlayerUpdate()
    {
        if(currentPlayers < RoomController.room.playersInRoom)
        {
            currentPlayers = RoomController.room.playersInRoom;
        }
    }




}
