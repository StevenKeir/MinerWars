﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSetup : MonoBehaviour
{
    private PhotonView PV;
    public int characterValue;
    public GameObject myCharacter;

    public int playerHealth;
    public int playerDamage;
    [SerializeField]
    private float immuneTimer;
    public float initialImmuneTimer;
    [SerializeField]
    private bool immuneTime;
    public bool isAlive;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            PV.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, PlayerInfo.playerInfo.mySelectedCharacter);
        }
        immuneTime = false;
        immuneTimer = initialImmuneTimer;
    }

    private void Update()
    {
        if(immuneTime == true)
        {
            immuneTimer -= Time.deltaTime;
        }

        if(immuneTimer <= 0)
        {
            immuneTime = false;
            immuneTimer = initialImmuneTimer;
        }


        if(playerHealth <= 0)
        {
            isAlive = false;
        }
        else
        {
            isAlive = true;
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Explosion")
        {
            if(immuneTime == false)
            {
             playerHealth -= playerDamage;
             immuneTime = true;
            }

        }
    }

    [PunRPC]
    void RPC_AddCharacter(int whichCharacter)
    {
        characterValue = whichCharacter;
        myCharacter = Instantiate(PlayerInfo.playerInfo.allCharacters[whichCharacter], transform.position, transform.rotation, transform);
    }

}
