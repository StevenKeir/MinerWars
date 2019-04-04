using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    public static ScoreCounter SC;

    public int[] scoreList = new int[2];
    public int localScore;
    public PhotonView PV;
    public TMP_Text playerOneText;
    public TMP_Text playerTwoText;
    public GameObject endGamePanel;
    public TMP_Text endGameScoreTally;
    public Image slot1;
    public Image slot2;


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
        endGamePanel.SetActive(false);
        scoreList[0] = 0;
        scoreList[1] = 0;
    }

    private void Update()
    {
        playerOneText.text = "Player 1: " + scoreList[0];
        playerTwoText.text = "Player 2: " + scoreList[1];

        if (PhotonNetwork.IsMasterClient)
        {
            scoreList[0] = localScore;
            PV.RPC("RPC_MasterClientSendScore", RpcTarget.Others, scoreList[0]);
            EndGameTallyUp();
        }
        else
        {
            scoreList[1] = localScore;
            PV.RPC("RPC_SendScore", RpcTarget.Others, scoreList[1]);
        }
    }

    void EndGameTallyUp()
    {
        if((scoreList[0] > scoreList[1]) && (GameSettings.GS.gameEnded == true))
        {
            endGameScoreTally.text = "Player 1 Wins!\nScore: " + scoreList[0];
            PV.RPC("RPC_EndGameSend", RpcTarget.AllBuffered, endGameScoreTally.text);
        }
        else if((scoreList[0] < scoreList[1]) && (GameSettings.GS.gameEnded == true))
        {
            endGameScoreTally.text = "Player 2 Wins!\nScore: " +  scoreList[1];
            PV.RPC("RPC_EndGameSend", RpcTarget.AllBuffered, endGameScoreTally.text);
        }
        else if((scoreList[0] == scoreList[1]) && (GameSettings.GS.gameEnded == true))
        {
            endGameScoreTally.text = "Draw";
            PV.RPC("RPC_EndGameSend", RpcTarget.AllBuffered, endGameScoreTally.text);
        }
    }

    [PunRPC]
    void RPC_SendScore(int playerScore)
    {
        scoreList[1] = playerScore;
    }

    [PunRPC]
    void RPC_MasterClientSendScore(int playerScore)
    {
        scoreList[0] = playerScore;
        //playerOneText.text = "Player 1: " + scoreList[0];
    }

    [PunRPC]
    void RPC_EndGameSend(string winner)
    {
        endGameScoreTally.text = winner;
    }



}
