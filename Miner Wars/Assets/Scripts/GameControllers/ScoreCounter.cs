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
    public bool dontCount = false;


    private void OnEnable()
    {
        //Sets a singleton
        if (ScoreCounter.SC == null)
        {
            ScoreCounter.SC = this;
        }
        else
        {
            if (ScoreCounter.SC != null)
            {
                Destroy(ScoreCounter.SC.gameObject);
                ScoreCounter.SC = this;
            }
        }
    }

    //Setting the references 
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        endGamePanel.SetActive(false);
        scoreList[0] = 0;
        scoreList[1] = 0;
    }

    private void Update()
    {
        //Sets the text for each player to the right item in the array.
        playerOneText.text = "Player 1: " + scoreList[0];
        playerTwoText.text = "Player 2: " + scoreList[1];
        
        //Checks if we are the Masterclient/Host if so send our score to the other player only is the score has changed.
        if (PhotonNetwork.IsMasterClient && dontCount == false)
        {
            if (scoreList[0] != localScore)
            {
                scoreList[0] = localScore;
                PV.RPC("RPC_MasterClientSendScore", RpcTarget.Others, scoreList[0]);
            }

            EndGameTallyUp();
        }
        //Checks if we are not the Masterclient/Host if so send our score to the other player only is the score has changed.
        else if (!PhotonNetwork.IsMasterClient && dontCount == false)
        {
            if (scoreList[1] != localScore)
            {
                scoreList[1] = localScore;
                PV.RPC("RPC_SendScore", RpcTarget.Others, scoreList[1]);
            }
        }
    }

    //Function that sends the ending score to the master client to decide the winner, has some issues when the other player leaves but was made last minute.
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

    //RPC call for photon to send the players scores accross the network.
    [PunRPC]
    void RPC_SendScore(int playerScore)
    {
        scoreList[1] = playerScore;
    }
    //RPC call for photon to send the players scores accross the network.
    [PunRPC]
    void RPC_MasterClientSendScore(int playerScore)
    {
        scoreList[0] = playerScore;
    }
    //RPC call that no longer is needed.
    [PunRPC]
    void RPC_EndGameSend(string winner)
    {
        endGameScoreTally.text = winner;
    }



}
