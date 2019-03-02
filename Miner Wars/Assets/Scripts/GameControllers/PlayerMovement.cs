using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using System.IO;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private PhotonView PV;
    private CharacterController myCharacterController;
    public float movementSpeed;
    public float rotationSpeed;

    public float cooldown = 3f;
    public float startCooldown = 3f;
    public bool startTimer = false;
    public bool offCooldown = true;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        myCharacterController = GetComponent<CharacterController>();
        offCooldown = true;
    }


    private void Update()
    {


        if (PV.IsMine)
        {
            PlaceDynamite();
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
        if (PV.IsMine)
        {
            BasicMovement();
            BasicRotation();

        }


    }


    void BasicMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            myCharacterController.Move(transform.up * Time.deltaTime * movementSpeed); 
        }
        if (Input.GetKey(KeyCode.A))
        {
            myCharacterController.Move(-transform.right * Time.deltaTime * movementSpeed); 
        }
        if (Input.GetKey(KeyCode.S))
        {
            myCharacterController.Move(-transform.up * Time.deltaTime * movementSpeed); 
        }
        if (Input.GetKey(KeyCode.D))
        {
            myCharacterController.Move(transform.right * Time.deltaTime * movementSpeed); 
        }
    }


    void BasicRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * rotationSpeed;
        transform.Rotate(new Vector3(0,mouseX,0));
    }

    void PlaceDynamite()
    {
        if (Input.GetKeyDown(KeyCode.E) && offCooldown == true)
        {
            PV.RPC("RPC_PlaceDynamite", RpcTarget.MasterClient);
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
