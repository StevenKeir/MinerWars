﻿using System.Collections;
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
    public int explosionUpgradeValue = 0;
    public float startCooldownTimer;
    public float cooldownTimer;
    public bool startTimer = false;

    private void Awake()
    {
        pauseWindow = GameObject.FindGameObjectWithTag("PauseWindow");
    }

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        RB = GetComponent<Rigidbody2D>();
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
        if (PV.IsMine && GameSettings.GS.isGameRunning == true)
        {
            UpdateAnimator();
            UpdateDirection();
            DynamiteClient();
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
        if (PV.IsMine && GameSettings.GS.isGameRunning == true)
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
        if (Input.GetKeyDown(KeyCode.Escape) && pauseCheck == false)
        {
            pauseCheck = true;
        }else if (Input.GetKeyDown(KeyCode.Escape) && pauseCheck == true)
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
        if(explosionUpgradeValue > 2)
        {
            explosionUpgradeValue = 2;
        }
        if(explosionUpgradeValue < 0)
        {
            explosionUpgradeValue = 0;
        }
        if(dynamiteCount < maxDynamiteCount)
        {
            startTimer = true;
        }
        if (dynamiteCount > 0 && dynamiteCount <= maxDynamiteCount)
        {
            if (Input.GetKeyDown(KeyCode.Space) && explosionUpgradeValue == 0)
            {
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Dynamite"), transform.position, Quaternion.identity, 0);
                dynamiteCount--;
            }
            if (Input.GetKeyDown(KeyCode.Space) && explosionUpgradeValue == 1)
            {
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Dynamite2"), transform.position, Quaternion.identity, 0);
                dynamiteCount--;
            }
            if (Input.GetKeyDown(KeyCode.Space) && explosionUpgradeValue == 2)
            {
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Dynamite2"), transform.position, Quaternion.identity, 0);
                dynamiteCount--;
            }
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


}
