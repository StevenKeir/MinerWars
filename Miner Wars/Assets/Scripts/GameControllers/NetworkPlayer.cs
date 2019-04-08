using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkPlayer : MonoBehaviourPun, IPunObservable
{
    public PhotonView PV;
    private Vector3 remotePlayerPos;


    private void Start()
    {
        if (!PV.IsMine)
        {

        }
    }

    private void Update()
    {
        if (PV.IsMine)
            return;

        var lagDistance = remotePlayerPos - transform.position;

        if(lagDistance.magnitude > 5f)
        {
            transform.position = remotePlayerPos;
            lagDistance = Vector3.zero;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            remotePlayerPos = (Vector3)stream.ReceiveNext();
        }
    }
}
