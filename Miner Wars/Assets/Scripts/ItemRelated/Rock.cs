using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PV.IsMine)
        {
            if (collision.gameObject.tag == "Explosion")
            {
                PV.RPC("RPC_DestroyMe", RpcTarget.MasterClient);
            }
        }

    }


    [PunRPC]
    void RPC_DestroyMe()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }



}
