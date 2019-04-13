using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    PhotonView PV;
    public bool animationEnded;
    float hitBoxRemoveTimer;
    public float startHitBoxRemoveTimer;
    //BoxCollider2D collider;
    public bool hitBarricade;

    private void Awake()
    {
        GameSettings.GS.hitCheckTest = hitBarricade;
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


}
