using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SpawnRocks : MonoBehaviour
{

    public Transform[] destructablePoints;
    public Transform[] nonDestructablePoints;
    PhotonView PV;
    public GameObject[] destructableGameobjects = new GameObject[57];

    private void Awake()
    {
        PV = GetComponent<PhotonView>();    
    }

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(Spawn());
            
        }
    }

    IEnumerator Spawn()
    {
        Debug.Log("I'm running");
        for (int i = 0; i < destructablePoints.Length; i++)
        {
            yield return new WaitForSeconds(0f);
            PhotonNetwork.InstantiateSceneObject(Path.Combine("PhotonPrefabs", "DestructibleRock"), destructablePoints[i].transform.position, Quaternion.identity, 0);
        }
        for (int i = 0; i < nonDestructablePoints.Length; i++)
        {
            yield return new WaitForSeconds(0f);
            PhotonNetwork.InstantiateSceneObject(Path.Combine("PhotonPrefabs", "NonDestructibleRock"), nonDestructablePoints[i].transform.position, Quaternion.identity, 0);
        }
        //GameSettings.GS.isGameRunning = true;
        if (PhotonNetwork.IsMasterClient)
        {
            PV.RPC("RPC_SendTimerUpdate", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    void RPC_SendTimerUpdate()
    {
        //Starts the timer for the game and lets other players know the game has started, also disables the loading screen.
        GameSettings.GS.startTimer = true;
        GameSettings.GS.isGameRunning = true;
        GameSettings.GS.loadingScreen.SetActive(false);
    }
}
