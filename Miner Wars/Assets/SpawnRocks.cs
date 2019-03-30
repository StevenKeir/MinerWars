using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SpawnRocks : MonoBehaviour
{

    public Transform[] destructablePoints;
    public Transform[] nonDestructablePoints;




    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(Spawn());
        }
    }

    IEnumerator Spawn()
    {
        for (int i = 0; i < destructablePoints.Length; i++)
        {
            yield return new WaitForSeconds(0f);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "DestructibleRock"), destructablePoints[i].transform.position, Quaternion.identity, 0);
            print(i);
        }
        for (int i = 0; i < nonDestructablePoints.Length; i++)
        {
            yield return new WaitForSeconds(0f);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "NonDestructibleRock"), nonDestructablePoints[i].transform.position, Quaternion.identity, 0);
        }
    }
}
