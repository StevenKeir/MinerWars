using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Rock : MonoBehaviour
{
    PhotonView PV;
    public int randomValue;
    bool hit;
    BoxCollider2D col;
    //public 
    //Reference to the collider and sprite
    LayerMask layer;
    public float radius;


    //Initialize the references and setting the random value for which it uses to put out a random gold piece. also setting hit to false to it will set to true only is hit.
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        col = GetComponent<BoxCollider2D>();
        randomValue = Random.Range(0, 11);
        hit = false;
    }
    private void Start()
    {
        layer = (int)1 << LayerMask.NameToLayer("Player");
    }

    //Checks if the rock was hit if so sends to the masterclient to destroy the object, then since it has a PhotonView it deletes it on both clients.
    private void Update()
    {
        //RespawnAfterTime();


        //Was in a if(PhotonNetwork.isMasterClient) but had issues with errors saying it couldn't destory the object, even tho it did it anyway.
        if (hit)
        {

            if (randomValue >= 9)
            {
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "GoldBig"), transform.position, Quaternion.identity, 0);
            }
            else if (randomValue >= 7 && randomValue <= 8)
            {
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "GoldSmall"), transform.position, Quaternion.identity, 0);
            }
            else if (randomValue >= 3 && randomValue <= 6)
            {
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "GoldMedium"), transform.position, Quaternion.identity, 0);
            }
            else
            {
                //print("No gold found");
            }
            PV.RPC("RPC_DestroyMe", RpcTarget.MasterClient);
            hit = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Explosion")
        {
            hit = true;
        }
    }

    void RespawnAfterTime()
    {
        //When hit check if player in the the area with physics.SphereOverlap2D if

        Debug.Log(Physics2D.OverlapCircle(col.transform.position, radius, layer));
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(col.transform.position, radius);
    }
    [PunRPC]
    void RPC_DestroyMe()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }



}
