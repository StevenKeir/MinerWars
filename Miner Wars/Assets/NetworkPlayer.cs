using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class NetworkPlayer : MonoBehaviourPun, IPunObservable
{
    public PhotonView PV;

    public PlayerMovement playerMovement;
    public Vector3 remotePlayerPosition;


    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        



    }

    private void Update()
    {
        if (!PV.IsMine)
        {
            transform.position = Vector3.MoveTowards(transform.position, remotePlayerPosition, Time.fixedDeltaTime);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(this.transform.position);
        }
        else
        {
            remotePlayerPosition = (Vector3)stream.ReceiveNext();


            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.timestamp));


            Vector3 oldPos = playerMovement.moveVector;

            Vector3 movement = transform.position - oldPos;
            remotePlayerPosition += (movement * lag);
        }
    }
}
