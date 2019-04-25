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
    public bool hitBarricade;
    public bool spawnCrator;
    int cratorInt = 0;
    public bool playSound;

    //Setting the references
    private void Awake()
    {
        GameSettings.GS.hitCheckTest = hitBarricade;
        PV = this.gameObject.GetComponent<PhotonView>();
        animationEnded = false;
        spawnCrator = false;
    }

    //Plays the animation and sounds, no idea why it's in lateUpdate....
    private void LateUpdate()
    {
        AnimationEnded();
        PlaySound();
    }

    //Spawns the crator once the animation has ended which is set by using the animator component.
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

    //Plays the sound once the animator has ticked the bool
    void PlaySound()
    {
        if (playSound)
        {
            GameSettings.GS.explosionSound.Play();
        }
    }

}
