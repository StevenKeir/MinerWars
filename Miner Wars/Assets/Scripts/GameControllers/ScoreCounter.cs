using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    public static ScoreCounter SC;

    public int[] scoreList = new int[2];
    public int localScore;
    public PhotonView PV;
    public TMP_Text playerOneText;
    public TMP_Text playerTwoText;


    private void OnEnable()
    {
        if (ScoreCounter.SC == null)
        {
            ScoreCounter.SC = this;
        }
    }

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Update()
    {

        playerOneText.text = "Player 1: " + scoreList[0];
        playerTwoText.text = "Player 2: " + scoreList[1];

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
        //playerTwoText.text = "Player 2: " + scoreList[1];
        //playerOneText.text = "Player 1: " + scoreList[0];
    }

    [PunRPC]
    void RPC_MasterClientSendScore(int playerScore)
    {
        scoreList[0] = playerScore;
        //
        playerOneText.text = "Player 1: " + scoreList[0];
        //playerTwoText.text = "Player 2: " + scoreList[1];
    }

}
