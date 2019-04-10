using Photon.Pun;
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

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            GameSettings.GS.localPlayerAvatar = this;
        }
    }

    private void Start()
    {
        immuneTime = false;
        immuneTimer = initialImmuneTimer;
        GameSettings.GS.players.Add(this);
    }

    private void Update()
    {
        PlayerInformation();
        if (PV.IsMine)
        {
            EndGameSlotUpdater();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Explosion")
        {
            if (immuneTime == false)
            {
                //playerHealth -= playerDamage;
                ScoreCounter.SC.localScore -= playerDamage / 2;
                immuneTime = true;
                if (PV.IsMine)
                {
                    //GameSettings.GS.healthBar.value = playerHealth;
                }

            }

        }
        if (collision.gameObject.tag == "Gold")
        {
            if (PV.IsMine)
            {
                myGoldCount += collision.gameObject.GetComponent<Gold>().goldWorth;
                ScoreCounter.SC.localScore += collision.gameObject.GetComponent<Gold>().goldWorth * 2;
                GameSettings.GS.text.text = "Gold: " + myGoldCount;
            }
            Destroy(collision.gameObject);
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
