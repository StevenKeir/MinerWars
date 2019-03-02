using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class Dynamite : MonoBehaviour
{
    PhotonView PV;
    bool starttimer;
    float floatTime;

    private void Start()
    {
       PV = GetComponent<PhotonView>();
        starttimer = true;
        floatTime = 3;

    }


    // Update is called once per frame
    void Update()
    {

        if(starttimer == true)
        {
            floatTime -= Time.deltaTime;
        }
        if(floatTime <= 0)
        {
            PV.RPC("RPC_ExplosionSpawn", RpcTarget.MasterClient);
            PhotonNetwork.Destroy(this.gameObject);
        }

    }


    [PunRPC]
    void RPC_ExplosionSpawn()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Explosion_Big"), transform.position, Quaternion.identity, 0);
    }

}
