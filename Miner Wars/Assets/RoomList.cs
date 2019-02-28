using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class RoomList : MonoBehaviour
{
    //public PhotonView pv;
    public static RoomList roomList;


    private void OnEnable() 
    {
        roomList = this;
    }

    private void Update() {
        print(PhotonNetwork.CountOfRooms);
    }
   
   


}
