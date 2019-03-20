using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Rock : MonoBehaviour
{
    PhotonView PV;
    public int randomValue;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        randomValue = Random.Range(0, 11);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PV.IsMine)
        {
            if (collision.gameObject.tag == "Explosion")
            {
                if(randomValue >= 9)
                {
                    PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "GoldBig"), transform.position, Quaternion.identity, 0);
                }else if(randomValue >= 7 && randomValue <= 8)
                {
                    PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "GoldSmall"), transform.position, Quaternion.identity, 0);
                }else if(randomValue >= 3 && randomValue <= 6)
                {
                    PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "GoldMedium"), transform.position, Quaternion.identity, 0);
                }
                else
                {
                    print("No gold found");
                }
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
