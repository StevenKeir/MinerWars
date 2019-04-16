using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VocalsRandom : MonoBehaviour {

    public AudioSource[] footsteps;
    public int howManySteps;
    public int stepNumber;
    public float timer;
    public float stepTime;
    public float minTime = 1;
    public float maxTime = 3;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
            stepNumber = Random.Range(0, howManySteps);
        stepTime = Random.Range(minTime, maxTime);
            timer += Time.deltaTime;
            if (timer >= stepTime)
            {
                footsteps[stepNumber].Play();
                timer = 0;
            }
        }

}
