using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using System.IO;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private PhotonView PV;
    [Header("Movement options, used in all movement types")]
    public float movementSpeed;
    private Rigidbody2D RB;
    private AvatarSetup AV;

    [Header("Test movement options")]
    public float minSpeed;
    public float duration;
    private float startTime;

    [Header("Animation Settings")]
    public float dirX;
    public float dirY;
    public bool isWalking;
    public float moveX;
    public float moveY;

    [Header("Upgrade Checks")]
    public bool hasExtraDynamite = false;
    public bool hasBoots = false;
    public bool hasUpgradedExplosion = false;
    public bool hasHealthIncrease = false;
    public bool hasBaricade = false;

    [Header("Animator related")]
    bool facingRight;
    public Animator anim;

    public GameObject pauseWindow;
    public bool pauseCheck;

    [Header("New dynamite system")]
    public int dynamiteCount = 0;
    public int maxDynamiteCount = 5;
    public float startCooldownTimer;
    public float cooldownTimer;
    public bool startTimer = false;

    public bool barricadeStartTimer = false;
    float startBarricadeCooldownTimer = 5f;
    public float barricadeCooldownTimer;


    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        RB = GetComponent<Rigidbody2D>();
        AV = GetComponent<AvatarSetup>();
        startTime = Time.time;
        anim = GetComponentInChildren<Animator>();
        
        if (PV.IsMine)
        {
            GameSettings.GS.localPlayer = this;
        }
    }

    private void Update()
    {
        PauseMenu();
        //Checking if it's the local player and if the game is running and not ended.
        if (PV.IsMine && GameSettings.GS.isGameRunning == true && GameSettings.GS.gameEnded == false)
        {
            UpdateAnimator();
            UpdateDirection();
            DynamiteClient();
            ItemUIUpdate();
            PlaceBarricade();
            BarricadeTimer();
            ExtraDynamiteCheck(GameSettings.GS.extraDynamiteTimesBought);
            if (startTimer)
            {
                if(dynamiteCount == maxDynamiteCount)
                {
                    startTimer = false;
                    cooldownTimer = startCooldownTimer;
                }
                else
                {
                    DynamiteTimer();
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (PV.IsMine && GameSettings.GS.isGameRunning == true && GameSettings.GS.gameEnded == false)
        {
            OtherMovement();
            //For flipping character sprite
            float h = Input.GetAxis("Horizontal");
            if (h > 0 && !facingRight)
                Flip();
            else if (h < 0 && facingRight)
                Flip();
        }
        else
        {
            startTime = Time.time;
        }
    }

    //flip sprite for left / right movement 
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void OtherMovement() 
    //Simple movement, which i tried to improve but couldn't get a satisfiying movement. wished i worked harder on this has it doesn't feel nice at times and feels clunky.
    {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        Vector3 moveVector = new Vector3(moveX, moveY, 0.0f);
        if (hasBoots)
        {
            transform.Translate(moveVector * (movementSpeed + GameSettings.GS.bootSpeedIncrease) * Time.deltaTime);
        }
        else
        {
            transform.Translate(moveVector * movementSpeed * Time.deltaTime);
        }
    }

    //Updates the animator so it know which way to face the sprite
    void UpdateDirection()
    {
        if(moveY > 0)
        {
            dirY = 1;
            dirX = 0;
        }
        if(moveY < 0)
        {
            dirY = -1;
            dirX = 0;
        }
        if(moveX > 0)
        {
            dirY = 0;
            dirX = 1;
        }
        if(moveX < 0)
        {
            dirY = 0;
            dirX = -1;
        }
    }

    //Updates the animator so it know which way to face the sprite
    void UpdateAnimator()
    {
        if ((moveX >= 0.3f || moveY >= 0.3f) || (moveX <= -0.3f || moveY <= -0.3f))
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }

        if (dirX != 1 || dirY != 1)
        {
            anim.SetFloat("dirX", dirX);
            anim.SetFloat("dirY", dirY);
        }
        if(isWalking == true)
        {
            anim.SetBool("isWalking", true);
        }
        if (isWalking == false)
        {
            anim.SetBool("isWalking", false);
        }

    }

    //Pulls up a menu for which the player can choose to quit, or disconnect.
    void PauseMenu()
    {
        if (PV.IsMine)
        {
            if (pauseWindow == null)
            {
                pauseWindow = GameObject.FindGameObjectWithTag("PauseWindow");
            }
            if (Input.GetKeyDown(KeyCode.Escape) && pauseCheck == false)
            {
                pauseCheck = true;
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && pauseCheck == true)
            {
                pauseCheck = false;
            }

            if (pauseCheck == true)
            {
                pauseWindow.gameObject.SetActive(true);
            }
            else
            {
                pauseWindow.gameObject.SetActive(false);
            }
        }
    }

    void DynamiteTimer()
    {
        cooldownTimer -= Time.deltaTime;
        if(cooldownTimer <= 0)
        {
            dynamiteCount++;
            cooldownTimer = startCooldownTimer;
        }
    }

    void DynamiteClient()
    {
        if(dynamiteCount > maxDynamiteCount)
        {
            dynamiteCount = maxDynamiteCount;
        }
        if(dynamiteCount < 0)
        {
            dynamiteCount = 0;
        }
        if(GameSettings.GS.upgradedExplosionTimesBought > 2)
        {
            GameSettings.GS.upgradedExplosionTimesBought = 2;
        }
        if(GameSettings.GS.upgradedExplosionTimesBought < 0)
        {
            GameSettings.GS.upgradedExplosionTimesBought = 0;
        }
        if(dynamiteCount < maxDynamiteCount)
        {
            startTimer = true;
        }
        if (dynamiteCount > 0 && dynamiteCount <= maxDynamiteCount)
        {
            if (Input.GetKeyDown(KeyCode.Space) && hasUpgradedExplosion == false)
            {
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Dynamite"), transform.position, Quaternion.identity, 0);
                dynamiteCount--;
            }
            if (Input.GetKeyDown(KeyCode.Space) && hasUpgradedExplosion == true)
            {
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Dynamite2"), transform.position, Quaternion.identity, 0);
                dynamiteCount--;
            }
        }
    }

    void BarricadeTimer()
    {
        if (hasBaricade && barricadeStartTimer)
        {
            barricadeCooldownTimer -= Time.deltaTime;
            if (barricadeCooldownTimer <= 0)
            {
                barricadeCooldownTimer = startBarricadeCooldownTimer;
                barricadeStartTimer = false;
            }
        }
    }

    void PlaceBarricade()
    {
        if (Input.GetKeyUp(KeyCode.E) && barricadeStartTimer == false && hasBaricade)
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Barricade"), transform.position, Quaternion.identity, 0);
            barricadeStartTimer = true;
        }
    }

    
    void ItemUIUpdate()
    {
        if (maxDynamiteCount == 2)
        {
            //Setting the UI elements to visiable, to inidicate what items have been purchased.
            GameSettings.GS.TNT2.color = new Color(GameSettings.GS.TNT2.color.r, GameSettings.GS.TNT2.color.g, GameSettings.GS.TNT2.color.b, 255f);
            if(dynamiteCount == 1)
            {
                //This was meant to be a cooldown indicator, as the player as of now doesn't know how long till the next tnt is available. 
                //Add Fill code
                //GameSettings.GS.TNT2FillImage.color = new Color(GameSettings.GS.TNT2FillImage.color.r, GameSettings.GS.TNT2FillImage.color.g, GameSettings.GS.TNT2FillImage.color.g //Something to do with timer);
            }
        }
        if (maxDynamiteCount == 3)
        {
            //Setting the UI elements to visiable, to inidicate what items have been purchased.
            GameSettings.GS.TNT3.color = new Color(GameSettings.GS.TNT3.color.r, GameSettings.GS.TNT3.color.g, GameSettings.GS.TNT3.color.b, 255f);
            if (dynamiteCount == 2)
            {
                //This was meant to be a cooldown indicator, as the player as of now doesn't know how long till the next tnt is available. 
                //Add Fill code
                //GameSettings.GS.TNT3FillImage.color = new Color(GameSettings.GS.TNT3FillImage.color.r, GameSettings.GS.TNT3FillImage.color.g, GameSettings.GS.TNT3FillImage.color.g //Something to do with timer);
            }
        }
        //Setting the UI elements to visiable, to inidicate what items have been purchased.
        if (hasBoots)
        {
            GameSettings.GS.speedBoots.color = new Color(GameSettings.GS.speedBoots.color.r, GameSettings.GS.speedBoots.color.g, GameSettings.GS.speedBoots.color.b, 255f);
        }
        //Setting the UI elements to visiable, to inidicate what items have been purchased.
        if (hasUpgradedExplosion)
        {
            GameSettings.GS.upgradedExplosion.color = new Color(GameSettings.GS.upgradedExplosion.color.r, GameSettings.GS.upgradedExplosion.color.g, GameSettings.GS.upgradedExplosion.color.b, 255f);
        }
        //Setting the UI elements to visiable, to inidicate what items have been purchased.
        if (hasBaricade)
        {
            GameSettings.GS.barricade.color = new Color(GameSettings.GS.barricade.color.r, GameSettings.GS.barricade.color.g, GameSettings.GS.barricade.color.b, 255f);
        }
    }

    //A switch statement that takes in the amound times the player has purchased the upgrade.
    void ExtraDynamiteCheck(int timesBought)
    {
        switch (timesBought)
        {
            case 1:
                maxDynamiteCount = 2;
                break;
            case 2:
                maxDynamiteCount = 3;
                break;
        }
    }


    //RPC call for photon so it knows to instanstiate the corisponding item, No longer used as had issues with latency. 
    [PunRPC]
    void RPC_PlaceDynamite()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Dynamite"), transform.position, Quaternion.identity,0);
    }

}
