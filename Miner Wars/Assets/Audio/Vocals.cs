using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vocals : MonoBehaviour {

    public AudioSource[] footsteps;
    public int howManySteps;
    public int stepNumber;
    public float timer;
    public float stepTime;
    public float minTime = 1;
    public float maxTime = 3;

    // Use this for initialization
    void Start()
    {
        stepTime = Random.Range(minTime, maxTime);
        stepNumber = Random.Range(0, howManySteps);
    }

    // Update is called once per frame
    void Update()
    {

        
        timer += Time.deltaTime;

        if (timer >= stepTime)
        {
            footsteps[stepNumber].Play();
            stepNumber = Random.Range(0, howManySteps);
            stepTime = Random.Range(minTime, maxTime);
            timer = 0;
        }
    }

}
