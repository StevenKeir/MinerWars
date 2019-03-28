using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{

    public int[] scoreList = new int[2];
    public int localScore;
    public PhotonView PV;


    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            scoreList[0] = localScore;
            PV.RPC("RPC_MasterClientSendScore", RpcTarget.Others, scoreList[0]);
        }
        else
        {
            scoreList[1] = localScore;
            PV.RPC("RPC_SendScore", RpcTarget.Others, scoreList[1]);
        }
    }





    [PunRPC]
    void RPC_SendScore(int playerScore)
    {
        scoreList[1] = playerScore;
    }

    void RPC_MasterClientSendScore(int playerScore)
    {
        scoreList[0] = playerScore;
    }

}
