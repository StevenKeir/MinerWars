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
            myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"), GameSettings.GS.spawnPoints[spawnPicker].position, GameSettings.GS.spawnPoints[spawnPicker].rotation, 0);
        }
    }

}
