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
    public float rotationSpeed;
    private Rigidbody2D RB;

    [Header("Cooldown for TNT")]
    [SerializeField]
    private float cooldown = 3f;
    [SerializeField]
    private float startCooldown = 3f;
    [SerializeField]
    private bool startTimer = false;
    [SerializeField]
    private bool offCooldown = true;


    [Header("Test movement options")]
    public float minSpeed;
    public float duration;
    private float startTime;

    public float dirX;
    public float dirY;
    public bool isWalking;
    public float moveX;
    public float moveY;

    bool facingRight;
    public Animator anim;

    //public GameObject pauseWindow;

    //public bool pauseCheck;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        RB = GetComponent<Rigidbody2D>();
        offCooldown = true;
        startTime = Time.time;

        anim = GetComponentInChildren<Animator>();



        //StartCoroutine(LateStart(0.001f));
    }


    private void Update()
    {

        UpdateAnimator();
        UpdateDirection();
        if (PV.IsMine)
        {

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
        }





    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (PV.IsMine /*&& (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W))*/)
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

    //void TestMovement()
    //Testing Mathf.smoothStep trying to get a smoother movement but ended up worse
    //{
    //    float t = (Time.time - startTime) / duration;

    //    //float moveX = Input.GetAxis("Horizontal");
    //    //float moveY = Input.GetAxis("Vertical");
    //    float moveX = Mathf.SmoothStep(minSpeed, (Input.GetAxis("Horizontal")), t);
    //    float moveY = Mathf.SmoothStep(minSpeed, (Input.GetAxis("Vertical")), t);
    //    Vector3 moveVector = new Vector3(moveX, moveY);
    //    transform.Translate(moveVector * movementSpeed * Time.deltaTime);

    //}



    void OtherMovement() 
    //Simple movement for now
    {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        Vector3 moveVector = new Vector3(moveX, moveY, 0.0f);
        if(moveX != 0.00f || moveY != 0.00f)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }

        transform.Translate(moveVector * movementSpeed * Time.deltaTime);
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
        if(dirX != 1 || dirY != 1)
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


    //void PauseMenu()
    //{
    //    if (Input.GetKey(KeyCode.Escape))
    //    {
    //        pauseCheck = true;
    //    }
    //    if (Input.GetKey(KeyCode.Escape) && pauseCheck == true)
    //    {
    //        pauseCheck = false;
    //    }
    //    if (pauseCheck == true)
    //    {
    //        pauseWindow.gameObject.SetActive(true);
    //    }
    //    else
    //    {
    //        pauseWindow.gameObject.SetActive(false);
    //    }
    //}

    //void PlaceDynamite()
    //{
    //    if (Input.GetKeyDown(KeyCode.E) && offCooldown == true)
    //    {
    //        PV.RPC("RPC_PlaceDynamite", RpcTarget.MasterClient);
    //        offCooldown = false;
    //        startTimer = true;
    //    }
    //}

    void PlaceDynamiteClient()
    {
        if (Input.GetKeyDown(KeyCode.E) && offCooldown == true)
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Dynamite"), transform.position, Quaternion.identity, 0);
            offCooldown = false;
            startTimer = true;
        }
        if (Input.GetKeyDown(KeyCode.R) && offCooldown == true)
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Dynamite2"), transform.position, Quaternion.identity, 0);
            offCooldown = false;
            startTimer = true;
        }
    }

    


    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        
    }

    [PunRPC]
    void RPC_PlaceDynamite()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Dynamite"), transform.position, Quaternion.identity,0);
    }


}
