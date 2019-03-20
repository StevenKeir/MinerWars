using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class Dynamite : MonoBehaviour
{
    PhotonView PV;
    bool starttimer;
    public float floatTime;

    BoxCollider2D col;

    private void Start()
    {
       PV = GetComponent<PhotonView>();
        starttimer = true;
        //floatTime = 3;
        col = GetComponent<BoxCollider2D>();
    }


    // Update is called once per frame
    void Update()
    {
        //Checks for timer to start a timer for which destroys the object and creates the explosion hitbox
        if(starttimer == true)
        {
            floatTime -= Time.deltaTime;
        }
        if(floatTime <= 0)
        {
            //PV.RPC("RPC_ExplosionSpawn", RpcTarget.MasterClient);
            if (PV.IsMine)
            {
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Explosion"), transform.position, Quaternion.identity, 0);
                PhotonNetwork.Destroy(this.gameObject);
            }

        }





    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        col.isTrigger = false;
    }


    [PunRPC]
    void RPC_ExplosionSpawn()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Explosion"), transform.position, Quaternion.identity, 0);
    }

}
