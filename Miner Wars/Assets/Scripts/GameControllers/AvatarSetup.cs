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
    int updatedGoldCount;
    public int myNumber = 10;
    public int playerHealth;
    public int playerDamage;
    [SerializeField]
    private float immuneTimer;
    public float initialImmuneTimer;
    [SerializeField]
    private bool immuneTime;
    public bool isAlive;

    private void Awake()
    {
        if (myNumber < PhotonNetwork.CurrentRoom.PlayerCount)
        {
            myNumber = PhotonNetwork.CurrentRoom.PlayerCount - 1;
        }
    }

    private void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            PV.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, PlayerInfo.playerInfo.mySelectedCharacter);


            PV.RPC("RPC_SendInfo", RpcTarget.AllBuffered);

        }




        immuneTime = false;
        immuneTimer = initialImmuneTimer;

        //myNumber = RoomController.room.myNumberInRoom;

    }

    private void Update()
    {

        PlayerInformation();


        //Doesn't work as i intended keep for later evaluation 
        //UpdateGold();       

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

        if(collision.gameObject.tag == "Gold")
        {
            myGoldCount += 10;
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

    //void UpdateGold()
    //{
    //    if (PV.IsMine)
    //    {
    //        if (updatedGoldCount < myGoldCount)
    //        {
    //            updatedGoldCount = myGoldCount;
    //            PV.RPC("RPC_SendGold", RpcTarget.AllBuffered);
    //        }
    //    }

    //}

    [PunRPC]
    void RPC_AddCharacter(int whichCharacter)
    {
        characterValue = whichCharacter;
        myCharacter = Instantiate(PlayerInfo.playerInfo.allCharacters[whichCharacter], transform.position, transform.rotation, transform);
    }

    [PunRPC]
    void RPC_SendGold()
    {
        updatedGoldCount = GameSettings.GS.gold[myNumber];
        Debug.Log("Gold Changed");
    }
    
    [PunRPC]
    void RPC_SendInfo()
    {
        GameSettings.GS.players.Add(this.myNumber);
    }
}
