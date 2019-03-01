using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Dynamite : MonoBehaviour
{
    //PhotonView PV;
    bool starttimer;
    float floatTime;

    private void Start()
    {
       //PV = GetComponent<PhotonView>();
        starttimer = true;
        floatTime = 3;

    }


    // Update is called once per frame
    void Update()
    {

        if(starttimer == true)
        {
            floatTime -= Time.deltaTime;
        }
        if(floatTime <= 0)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }

    }
}
