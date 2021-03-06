﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Dynamite2 : MonoBehaviour
{
    PhotonView PV;
    bool starttimer;
    public float floatTime;

    //For some reason i made two of the same script, should been in the same one and used a if statement to spawn the right object.
    private void Start()
    {
        PV = GetComponent<PhotonView>();
        starttimer = true;
    }


    // Update is called once per frame
    void Update()
    {
        //Checks for timer to start a timer for which destroys the object and creates the explosion hitbox
        if (starttimer == true)
        {
            floatTime -= Time.deltaTime;
        }
        if (floatTime <= 0)
        {
            if (PV.IsMine)
            {
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Explosion2"), transform.position, Quaternion.identity, 0);
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }


    [PunRPC]
    void RPC_ExplosionSpawn()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Explosion"), transform.position, Quaternion.identity, 0);
    }
}
