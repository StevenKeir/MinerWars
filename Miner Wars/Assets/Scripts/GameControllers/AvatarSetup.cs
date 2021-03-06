﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSetup : MonoBehaviour
{
    private PhotonView PV;
    public int characterValue;
    public GameObject myCharacter;

    public int myGoldCount;
    public int playerHealth;
    public bool isAlive;
    public int playerDamage;
    [SerializeField]
    private float immuneTimer;
    public float initialImmuneTimer;
    [SerializeField]
    public bool immuneTime;

    Animator anim;
    SpriteRenderer sprite;

    [Header("End game references")]
    public Sprite winningStance;
    public Sprite losingStance;

    [Header("Score related")]
    public int scoreModifier;

    [Header("Sound")]
    int soundInt; 

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        sprite = GetComponent<SpriteRenderer>();
        if (PV.IsMine)
        {
            GameSettings.GS.localPlayerAvatar = this;
        }
        if (PV.IsMine)
        {
            //EndGameSlotUpdater();
        }
    }

    private void Start()
    {
        //Setting the instances, Not really required.
        immuneTime = false;
        immuneTimer = initialImmuneTimer;
    }

    private void Update()
    {
        //Immunity check function
        PlayerInformation();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Checks if the player collides with an exoplosion.
        if (collision.gameObject.tag == "Explosion")
        {
            //Checks if you're not immune if no then take the damage, then start the immunity function.
            if (immuneTime == false)
            {
                immuneTime = true;
                StartCoroutine(HitFlash());
                if (PV.IsMine)
                {
                    soundInt = (int)Random.Range(0, 3);
                    ScoreCounter.SC.localScore -= playerDamage;
                    HurtSound(soundInt);
                    
                }
                
            }
        }
        //Checks if the player collides with gold.
        if (collision.gameObject.tag == "Gold")
        {
            //Checking if it's the local player colliding if so add the score to the local score and update the UI element. 
            if (PV.IsMine)
            {
                myGoldCount += collision.gameObject.GetComponent<Gold>().goldWorth;
                ScoreCounter.SC.localScore += collision.gameObject.GetComponent<Gold>().goldWorth * scoreModifier;
                GameSettings.GS.text.text = "Gold: " + myGoldCount;
                GameSettings.GS.goldPickup.Play();
            }
            Destroy(collision.gameObject);
        }
    }

    IEnumerator HitFlash()
    {
        sprite.color = new Color(255, 0, 0);
        yield return new WaitForSeconds(0.5f);
        sprite.color = new Color(255, 255, 255);
        yield return new WaitForSeconds(0.5f);
        sprite.color = new Color(255, 0, 0);
        yield return new WaitForSeconds(0.5f);
        sprite.color = new Color(255, 255, 255);
    }

    void HurtSound(int hurtSound)
    {
        switch (hurtSound)
        {
            case 0:
                GameSettings.GS.hurtSound.Play();
                break;
            case 1:
                GameSettings.GS.hurtSound1.Play();
                break;
            case 2:
                GameSettings.GS.hurtSound2.Play();
                break;
            case 3:
                GameSettings.GS.hurtSound3.Play();
                break;
        }
    }

    void PlayerInformation()
    {
        if (immuneTime == true)
        {
            immuneTimer -= Time.deltaTime;
        }
        if (immuneTimer <= 0)
        {
            immuneTime = false;
            immuneTimer = initialImmuneTimer;
        }
        if (playerHealth <= 0)
        {
            isAlive = false;
        }
        else
        {
            isAlive = true;
        }
    }

    void EndGameSlotUpdater()
    {
        if ((ScoreCounter.SC.scoreList[0] > ScoreCounter.SC.scoreList[1]) && (GameSettings.GS.gameEnded == true))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                //winningStance = ScoreCounter.SC.slot1.sprite;
                PV.RPC("RPC_SendSpriteWinner", RpcTarget.AllBuffered);
            }
            else if (!PhotonNetwork.IsMasterClient)
            {
                //losingStance = ScoreCounter.SC.slot2.sprite;
                PV.RPC("RPC_SendSpriteLoser", RpcTarget.AllBuffered);
            }
        }
        else if ((ScoreCounter.SC.scoreList[0] < ScoreCounter.SC.scoreList[1]) && (GameSettings.GS.gameEnded == true))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                //losingStance = ScoreCounter.SC.slot1.sprite;
                PV.RPC("RPC_SendSpriteLoser", RpcTarget.AllBuffered);
            }
            else if (!PhotonNetwork.IsMasterClient)
            {
                //winningStance = ScoreCounter.SC.slot2.sprite;
                PV.RPC("RPC_SendSpriteWinner", RpcTarget.AllBuffered);
            }
        }
        else if ((ScoreCounter.SC.scoreList[0] == ScoreCounter.SC.scoreList[1]) && (GameSettings.GS.gameEnded == true))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                //losingStance = ScoreCounter.SC.slot1.sprite;
                PV.RPC("RPC_SendSpriteDrawMaster", RpcTarget.AllBuffered);
            }
            else if (!PhotonNetwork.IsMasterClient)
            {
                //losingStance = ScoreCounter.SC.slot2.sprite;
                PV.RPC("RPC_SendSpriteDrawNotMaster", RpcTarget.AllBuffered);
            }
        }
    }

  

    [PunRPC]
    void RPC_AddCharacter(int whichCharacter)
    {
        //characterValue = whichCharacter;
        //myCharacter = Instantiate(PlayerInfo.playerInfo.allCharacters[whichCharacter], transform.position, transform.rotation, transform);
        //anim.runtimeAnimatorController = PlayerInfo.playerInfo.animControllers[whichCharacter];
        //sprite.sprite = PlayerInfo.playerInfo.allSprites[whichCharacter];
    }

    [PunRPC]
    void RPC_SendSpriteWinner()
    {
        ScoreCounter.SC.slot1.sprite = winningStance;
    }

    [PunRPC]
    void RPC_SendSpriteLoser()
    {
        ScoreCounter.SC.slot2.sprite = losingStance;
    }

    [PunRPC]
    void RPC_SendSpriteDrawMaster()
    {
        ScoreCounter.SC.slot1.sprite = losingStance;
    }
    void RPC_SendSpriteDrawNotMaster()
    {
        ScoreCounter.SC.slot2.sprite = losingStance;
    }
}
