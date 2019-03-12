using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    PhotonView PV;
    public bool animationEnded;

    private void Awake()
    {
        PV = this.gameObject.GetComponent<PhotonView>();
        animationEnded = false;
    }

    private void LateUpdate()
    {
        AnimationEnded();
    }

    void AnimationEnded()
    {
        if (animationEnded == true)
        {
            Destroy(this.gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        PV.RPC("RPC_SendDamage", RpcTarget.Others);


    }

    [PunRPC]
    void RPC_SendDamage()
    {
        Debug.Log("Hit");
    }

}
