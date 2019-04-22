using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    PhotonView PV;
    public bool animationEnded;
    float hitBoxRemoveTimer;
    public float startHitBoxRemoveTimer;
    //BoxCollider2D collider;
    public bool hitBarricade;
    public bool spawnCrator;
    int cratorInt = 0;

    private void Awake()
    {
        GameSettings.GS.hitCheckTest = hitBarricade;
        PV = this.gameObject.GetComponent<PhotonView>();
        animationEnded = false;
        spawnCrator = false;
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
        if (spawnCrator == true && cratorInt < 1)
        {
            cratorInt++;
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Crator"), transform.position, Quaternion.identity, 0);
        }
    }


}
