using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Baricade : MonoBehaviour
{
    PhotonView PV;
    public int hits;
    public SpriteRenderer mainSprite;
    public Sprite defaultSprite;
    public Sprite secondHit;
    public Sprite thirdHit;
    Collider2D myCol;
    public bool hit = false;
    public bool takeDmg = false;
    float startTimer;
    public float timer;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        myCol = GetComponent<BoxCollider2D>();
        startTimer = 0.8f;
    }
    private void Start()
    {
        myCol.isTrigger = true;
        timer = startTimer;
    }

    private void FixedUpdate()
    {
        //HitCheck();
        HitTimer();
        SpriteChange();
    }
    void SpriteChange()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            switch (hits)
            {
                case 0:
                    mainSprite.sprite = defaultSprite;
                    //PV.RPC("RPC_SendSpriteChangeHit", RpcTarget.AllBuffered, 0);
                    break;
                case 1:
                    mainSprite.sprite = secondHit;
                    //PV.RPC("RPC_SendSpriteChangeHit", RpcTarget.AllBuffered, 1);
                    break;
                case 2:
                    mainSprite.sprite = thirdHit;
                    //PV.RPC("RPC_SendSpriteChangeHit", RpcTarget.AllBuffered, 2);
                    break;
                case 3:
                    //PV.RPC("RPC_DestroyThis", RpcTarget.AllBuffered);
                    break;
            }
        }
        
        if (PhotonNetwork.IsMasterClient)
        {
            switch (hits)
            {
                case 0:
                    mainSprite.sprite = defaultSprite;
                    PV.RPC("RPC_SendSpriteChangeHit", RpcTarget.OthersBuffered, 0);
                    break;
                case 1:
                    mainSprite.sprite = secondHit;
                    PV.RPC("RPC_SendSpriteChangeHit", RpcTarget.OthersBuffered, 1);
                    break;
                case 2:
                    mainSprite.sprite = thirdHit;
                    PV.RPC("RPC_SendSpriteChangeHit", RpcTarget.OthersBuffered, 2);
                    break;
                case 3:
                    PV.RPC("RPC_DestroyThis", RpcTarget.AllBuffered);
                    break;
            }
        }
    }

    void HitTimer()
    {
        if (hit)
        {
            takeDmg = false;
            timer -= Time.deltaTime;
        }
        else
        {
            timer = startTimer;
        }
        if(timer <=- 0.00f)
        {
            hit = false;
            timer = startTimer;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (collision.gameObject.tag == "Explosion" && hit == false && GameSettings.GS.hitCheckTest == false)
            {
                hit = true;
                hits++;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            myCol.isTrigger = false;
        }
    }

    [PunRPC]
    void RPC_DestroyThis()
    {
        Destroy(this.gameObject);
    }

    [PunRPC]
    void RPC_SendSpriteChangeHit(int hitCount)
    {
        hits = hitCount;
    }

    [PunRPC]
    void RPC_SendSpriteChangeHitNotMaster()
    {
        hits++;
    }

    
}
