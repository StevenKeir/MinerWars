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

    //Sets the references, should be done in awake. left in start as punishment to myself XD.
    private void Start()
    {
        PV = GetComponent<PhotonView>();
        starttimer = true;
        col = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        //Checks for timer to start a timer for which destroys the object and creates the explosion hitbox
        if(starttimer == true)
        {
            floatTime -= Time.deltaTime;
        }
        if(floatTime <= 0)
        {
            if (PV.IsMine)
            {
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Explosion"), transform.position, Quaternion.identity, 0);
                
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }

    //Stupid way of making the TNT colidable after we walk away from it, should been implemented in better. 
    private void OnTriggerExit2D(Collider2D collision)
    {
        col.isTrigger = false;
    }

    //RPC call that is no longer needed.
    [PunRPC]
    void RPC_ExplosionSpawn()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Explosion"), transform.position, Quaternion.identity, 0);
    }

}
