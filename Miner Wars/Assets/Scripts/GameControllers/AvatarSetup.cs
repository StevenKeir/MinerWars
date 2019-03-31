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
    private bool immuneTime;

    Animator anim;
    SpriteRenderer sprite;


    private void Awake()
    {
        //sprite = GetComponent<SpriteRenderer>();
        //anim = GetComponent<Animator>();
        //if (PV.IsMine)
        //{
        //    PV.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, PlayerInfo.playerInfo.mySelectedCharacter);

        //}
        PV = GetComponent<PhotonView>();



    }

    private void Start()
    {
        immuneTime = false;
        immuneTimer = initialImmuneTimer;
    }

    private void Update()
    {
        PlayerInformation();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Explosion")
        {
            if (immuneTime == false)
            {
                playerHealth -= playerDamage;
                immuneTime = true;
                if (PV.IsMine)
                {
                    GameSettings.GS.healthBar.value = playerHealth;
                }

            }

        }
        if (collision.gameObject.tag == "Gold")
        {
            if (PV.IsMine)
            {
                myGoldCount += collision.gameObject.GetComponent<Gold>().goldWorth;
                ScoreCounter.SC.localScore += collision.gameObject.GetComponent<Gold>().goldWorth / 2;
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


    [PunRPC]
    void RPC_AddCharacter(int whichCharacter)
    {
        //characterValue = whichCharacter;
        //myCharacter = Instantiate(PlayerInfo.playerInfo.allCharacters[whichCharacter], transform.position, transform.rotation, transform);
        //anim.runtimeAnimatorController = PlayerInfo.playerInfo.animControllers[whichCharacter];
        //sprite.sprite = PlayerInfo.playerInfo.allSprites[whichCharacter];

    }


}
