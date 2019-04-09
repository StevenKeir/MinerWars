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

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
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

    [PunRPC]
    void RPC_DestroyThis()
    {
        Destroy(this.gameObject);
    }


}
