using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private PhotonView PV;
    private CharacterController myCharacterController;
    public float movementSpeed;
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        myCharacterController = GetComponent<CharacterController>();
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

}
