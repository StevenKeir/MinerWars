using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAudio : MonoBehaviour
{
    public AudioSource mouseOverNoise;
    public AudioSource selectNoise;
    public AudioSource skinHover;
    public AudioSource skinSelectwork;
    public AudioSource skinSelectFail;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseOver()
    {
        mouseOverNoise.Play();
    }

    public void OnMouseDown()
    {
        selectNoise.Play();
    }

    public void SkinOnMouseOver()
    {
        skinHover.Play();
    }

    public void SkinSelectOnMouseDown()
    {
        skinSelectwork.Play();
    }

    public void SkinFailOnMouseDown()
    {
        skinSelectFail.Play();
    }
}
