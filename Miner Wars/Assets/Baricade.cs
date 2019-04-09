using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Baricade : MonoBehaviour
{
    PhotonView PV;
    int hits;
    public SpriteRenderer mainSprite;
    public Sprite defaultSprite;
    public Sprite secondHit;
    public Sprite thirdHit;
    Collider2D myCol;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        myCol = GetComponent<BoxCollider2D>();
    }
    private void Start()
    {
        myCol.isTrigger = true;
    }

    private void Update()
    {
        SpriteChange();
    }

    void SpriteChange()
    {
        switch (hits)
        {
            case 0:
                mainSprite.sprite = defaultSprite;
                break;
            case 1:
                mainSprite.sprite = secondHit;
                break;
            case 2:
                mainSprite.sprite = thirdHit;
                break;
            case 3:
                PV.RPC("RPC_DestroyThis", RpcTarget.AllBuffered);
                break;
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Explosion")
        {
            hits++;
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


}
