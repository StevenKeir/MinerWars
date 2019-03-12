using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using System.IO;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private PhotonView PV;
    private CharacterController myCharacterController;
    [Header("Movement options, used in all movement types")]
    public float movementSpeed;
    public float rotationSpeed;

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


    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        myCharacterController = GetComponent<CharacterController>();
        offCooldown = true;
        startTime = Time.time;
    }


    private void Update()
    {
        if (PV.IsMine)
        {
            //PlaceDynamite();
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
        if (PV.IsMine && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W)))
        {
            //BasicMovement();
            //BasicRotation();
            OtherMovement();
            //TestMovement();
        }
        else
        {
            startTime = Time.time;
        }


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
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        Vector3 moveVector = new Vector3(moveX, moveY);
      

        transform.Translate(moveVector * movementSpeed * Time.fixedDeltaTime);


    }

    
    //void BasicMovement()
    //Couldn't use this due to issues with collisions on the character controller.
    //{
    //    if (Input.GetKey(KeyCode.W))
    //    {
    //        myCharacterController.Move(transform.up * Time.deltaTime * movementSpeed); 
    //    }
    //    if (Input.GetKey(KeyCode.A))
    //    {
    //        myCharacterController.Move(-transform.right * Time.deltaTime * movementSpeed); 
    //    }
    //    if (Input.GetKey(KeyCode.S))
    //    {
    //        myCharacterController.Move(-transform.up * Time.deltaTime * movementSpeed); 
    //    }
    //    if (Input.GetKey(KeyCode.D))
    //    {
    //        myCharacterController.Move(transform.right * Time.deltaTime * movementSpeed); 
    //    }
    //}


    void BasicRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * rotationSpeed;
        transform.Rotate(new Vector3(0,mouseX,0));
    }

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
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Explosion")
        {
            Debug.Log("Hit");
        }
    }




    [PunRPC]
    void RPC_PlaceDynamite()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Dynamite"), transform.position, Quaternion.identity,0);
    }


}
