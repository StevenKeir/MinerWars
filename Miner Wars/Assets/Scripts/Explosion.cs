using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    ///public Animation animation;
    //Animator ani;
    //AnimationClip clip;
    public bool startPlaying;
    public bool animationEnded;

    private void Awake()
    {
        //animation = this.gameObject.GetComponent<Animation>();
        //startPlaying = true;
        animationEnded = false;
    }

    private void LateUpdate()
    {

        if (animationEnded == true)
        {
            //startPlaying = false;
            Destroy(this.gameObject);
        }
    }


    
}
