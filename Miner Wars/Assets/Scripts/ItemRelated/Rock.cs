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
    SpriteRenderer sprite;
    LayerMask layer;
    public float radius;
    public bool isDisabled;
    public float timer;
    public bool startTimer;
    public bool playerNearby;
    float startingTime;
    public float minTime;
    public float maxTime;

    //Initialize the references and setting the random value for which it uses to put out a random gold piece. also setting hit to false to it will set to true only is hit.
    //Setting the reference
    private void Awake() 
    {
        PV = GetComponent<PhotonView>();
        col = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    //Using the references, if needed.
    private void Start()
    {
        layer = (int)1 << LayerMask.NameToLayer("Player");
        hit = false;
        randomValue = Random.Range(0, 11);
        startingTime = Random.Range(minTime, maxTime);
        startTimer = false;
        timer = startingTime;
    }

    //Checks if the rock was hit if so sends to the masterclient to destroy the object, then since it has a PhotonView it deletes it on both clients.
    private void Update()
    {
        PlayerCheck();
        RespawnTimer();
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
            isDisabled = true;
            startTimer = true;
            hit = false;
        }
        DisableGraphics();
    }

    //Checks if the explosion has hit the rock if set the bool
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Explosion")
        {
            hit = true;
        }
    }

    //respawn the rock after some time.
    void RespawnTimer()
    {
        if (isDisabled)
        {
            //startTimer = true;
            if (startTimer)
            {
                timer -= Time.deltaTime;
            }
            if (timer <= 0.00f)
            {
                startTimer = false;
                startingTime = Random.Range(minTime, maxTime);
                timer = startingTime;
                RespawnRock();
            }
        }
    }

    //Respawns the rock only if there is no player nearby.
    void RespawnRock()
    {
        if (playerNearby)
        {
            startTimer = true;
        }
        else if (!playerNearby)
        {
            isDisabled = false;
        }
    }

    //Disable the sprite when the function 
    void DisableGraphics()
    {
        if (isDisabled && sprite.enabled == true)
        {
            PV.RPC("RPC_DisableSprites", RpcTarget.AllBuffered);
        }
        else
        {
            //PV.RPC("RPC_EnableSprites", RpcTarget.AllBuffered);
        }
    }

    //Checks if the player is nearby
    void PlayerCheck()
    {
        if(Physics2D.OverlapCircle(col.transform.position, radius, layer))
        {
            playerNearby = true;
        }
        else
        {
            playerNearby = false;
        }
    }

    //Used to debug the range of the overlap circle
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

    [PunRPC]
    void RPC_DisableSprites()
    {
        sprite.enabled = false;
        col.enabled = false;
    }

    [PunRPC]
    void RPC_EnableSprites()
    {
        sprite.enabled = true;
        col.enabled = true;
    }
}
