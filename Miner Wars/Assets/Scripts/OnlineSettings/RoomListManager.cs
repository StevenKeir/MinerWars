using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomListManager : MonoBehaviourPun
{
    public static RoomListManager RLM;
    public List<AllRoomInfo> roomInfo = new List<AllRoomInfo>();


    // Start is called before the first frame update
    void Start()
    {
        if (RoomListManager.RLM == null)
        {
            RoomListManager.RLM = this;
        }
        else
        {
            if (RoomListManager.RLM != this)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

}
