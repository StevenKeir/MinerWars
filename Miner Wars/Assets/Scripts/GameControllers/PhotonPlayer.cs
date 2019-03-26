using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;

public class PhotonPlayer : MonoBehaviour
{
    private PhotonView pv;
    public GameObject myAvatar;

    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        int spawnPicker = Random.Range(0, GameSettings.GS.spawnPoints.Length);

        if (pv.IsMine)
        {
            if (PlayerPrefs.GetInt("MyCharacter") == 0)
            {
                myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "P1"), GameSettings.GS.spawnPoints[spawnPicker].position, GameSettings.GS.spawnPoints[spawnPicker].rotation, 0);
            }
            if (PlayerPrefs.GetInt("MyCharacter") == 1)
            {
                myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "P2"), GameSettings.GS.spawnPoints[spawnPicker].position, GameSettings.GS.spawnPoints[spawnPicker].rotation, 0);
            }
            if (PlayerPrefs.GetInt("MyCharacter") == 2)
            {
                myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "P3"), GameSettings.GS.spawnPoints[spawnPicker].position, GameSettings.GS.spawnPoints[spawnPicker].rotation, 0);
            }
            if (PlayerPrefs.GetInt("MyCharacter") == 3)
            {
                myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "P4"), GameSettings.GS.spawnPoints[spawnPicker].position, GameSettings.GS.spawnPoints[spawnPicker].rotation, 0);
            }
            
        }
    }

}
