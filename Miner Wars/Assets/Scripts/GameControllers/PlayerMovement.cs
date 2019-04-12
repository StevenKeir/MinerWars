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
    //public float rotationSpeed;
    private Rigidbody2D RB;
    private AvatarSetup AV;
    /*
    [Header("Cooldown for TNT")]
    [SerializeField]
    private float cooldown = 3f;
    [SerializeField]
    private float startCooldown = 3f;
    [SerializeField]
    private bool startTimer = false;
    [SerializeField]
    private bool offCooldown = true;
    [SerializeField]
    private float secondCooldown = 1.5f;
    [SerializeField]
    private float secondStartCooldown = 1.5f;
    [SerializeField]
    private bool secondStartTimer = false;
    [SerializeField]
    private bool secondOffCooldown = true;
    [SerializeField]
    private bool nextDynamite = false;
    [SerializeField]
    private float startNextTimer = 0.5f;
    [SerializeField]
    private float nextTimer = 0.5f;
    [SerializeField]
    bool nextTimerStart;
    */

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
    //public int explosionUpgradeValue = 0;
    public float startCooldownTimer;
    public float cooldownTimer;
    public bool startTimer = false;

    public bool barricadeStartTimer = false;
    float startBarricadeCooldownTimer = 5f;
    public float barricadeCooldownTimer;


    private void Awake()
    {
        //pauseWindow = GameObject.FindGameObjectWithTag("PauseWindow");
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

    // Start is called before the first frame update
    void Start()
    {

    }


    private void Update()
    {
        PauseMenu();
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
            /*
            PlaceDynamiteClient(); 
            if (startTimer == true)
            {
                cooldown -= Time.deltaTime;
            }
            if (cooldown <= 0)
            {
                startTimer = false;
                offCooldown = true;
                cooldown = startCooldown;
            }

            if (secondStartTimer == true)
            {
                secondCooldown -= Time.deltaTime;
            }
            if (secondCooldown <= 0)
            {
                secondStartTimer = false;
                secondOffCooldown = true;
                secondCooldown = secondStartCooldown;
                if (hasExtraDynamite)
                {
                    nextDynamite = false;
                }
            }

            if (hasExtraDynamite)
            {
                if (nextTimerStart == true)
                {
                    nextTimer -= Time.deltaTime;
                }
                if (nextTimer <= 0)
                {
                    nextDynamite = true;
                    nextTimerStart = false;
                    nextTimer = startNextTimer;
                }
            }
            */
        }
    }

    void FixedUpdate()
    {
        if (PV.IsMine && GameSettings.GS.isGameRunning == true && GameSettings.GS.gameEnded == false /*&& AV.immuneTime == false*/)
        {
            OtherMovement();
            // for flipping character
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

    //flip sprite for left movement
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void OtherMovement() 
    //Simple movement for now
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

    /*
    void PlaceDynamiteClient()
    {
        if (Input.GetKeyDown(KeyCode.Space) && offCooldown == true && hasUpgradedExplosion == false)
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Dynamite"), transform.position, Quaternion.identity, 0);
            offCooldown = false;
            startTimer = true;
            nextTimerStart = true;
        }
        if (Input.GetKeyDown(KeyCode.Space) && offCooldown == true && hasUpgradedExplosion == true)
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Dynamite2"), transform.position, Quaternion.identity, 0);
            offCooldown = false;
            startTimer = true;
            nextTimerStart = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && offCooldown == false && nextDynamite == true && hasExtraDynamite == true && hasUpgradedExplosion == false)
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Dynamite"), transform.position, Quaternion.identity, 0);
            secondOffCooldown = false;
            secondStartTimer = true;
            nextDynamite = false;
        }
        if (Input.GetKeyDown(KeyCode.Space) && offCooldown == false && nextDynamite == true && hasExtraDynamite == true && hasUpgradedExplosion == true)
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Dynamite2"), transform.position, Quaternion.identity, 0);
            secondOffCooldown = false;
            secondStartTimer = true;
            nextDynamite = false;
        }
    }
    */

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
            /*
            if (Input.GetKeyDown(KeyCode.Space) && GameSettings.GS.upgradedExplosionTimesBought == 2)
            {
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Dynamite2"), transform.position, Quaternion.identity, 0);
                dynamiteCount--;
            }
            */
        }
    }

    void BarricadeTimer()
    {
        barricadeCooldownTimer -= Time.deltaTime;
        if (barricadeCooldownTimer <= 0)
        {
            barricadeCooldownTimer = startBarricadeCooldownTimer;
            barricadeStartTimer = false;
        }
    }

    void PlaceBarricade()
    {
        if (Input.GetKey(KeyCode.E) && barricadeStartTimer == false && hasBaricade)
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Barricade"), transform.position, Quaternion.identity, 0);
            barricadeStartTimer = true;
        }
    }
    void ItemUIUpdate()
    {
        if (maxDynamiteCount == 2)
        {
            GameSettings.GS.TNT2.color = new Color(GameSettings.GS.TNT2.color.r, GameSettings.GS.TNT2.color.g, GameSettings.GS.TNT2.color.b, 255f);
            if(dynamiteCount == 1)
            {
                //Add Fill code
                //GameSettings.GS.TNT2FillImage.color = new Color(GameSettings.GS.TNT2FillImage.color.r, GameSettings.GS.TNT2FillImage.color.g, GameSettings.GS.TNT2FillImage.color.g //Something to do with timer);
            }
        }
        if (maxDynamiteCount == 3)
        {
            GameSettings.GS.TNT3.color = new Color(GameSettings.GS.TNT3.color.r, GameSettings.GS.TNT3.color.g, GameSettings.GS.TNT3.color.b, 255f);
            if (dynamiteCount == 2)
            {
               //Add Fill code
               // GameSettings.GS.TNT3FillImage.color = new Color(GameSettings.GS.TNT3FillImage.color.r, GameSettings.GS.TNT3FillImage.color.g, GameSettings.GS.TNT3FillImage.color.g //Something to do with timer);
            }
        }

        if (hasBoots)
        {
            GameSettings.GS.speedBoots.color = new Color(GameSettings.GS.speedBoots.color.r, GameSettings.GS.speedBoots.color.g, GameSettings.GS.speedBoots.color.b, 255f);
        }

        if (hasUpgradedExplosion)
        {
            GameSettings.GS.upgradedExplosion.color = new Color(GameSettings.GS.upgradedExplosion.color.r, GameSettings.GS.upgradedExplosion.color.g, GameSettings.GS.upgradedExplosion.color.b, 255f);
        }

        if (hasBaricade)
        {
            GameSettings.GS.barricade.color = new Color(GameSettings.GS.barricade.color.r, GameSettings.GS.barricade.color.g, GameSettings.GS.barricade.color.b, 255f);
        }
    }

 

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



    [PunRPC]
    void RPC_PlaceDynamite()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Dynamite"), transform.position, Quaternion.identity,0);
    }

    [PunRPC]
    void RPC_PlaceBarricade()
    {
        
    }

}
