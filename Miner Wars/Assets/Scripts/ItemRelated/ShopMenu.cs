using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMenu : MonoBehaviour
{

    private PhotonView PV;
    public bool someOneInShop = false;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (PV.IsMine)
        {
            if (someOneInShop)
            {
                if (Input.GetKey(KeyCode.F))
                {

                    GameSettings.GS.shopWindow.SetActive(true);
                    print("Shop opened");
                }
            }
            else
            {
                GameSettings.GS.shopWindow.SetActive(false);
            }
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            print("Someone Entered me ;)");
            someOneInShop = true;
        }


    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            print("Someone Left me :(");
            someOneInShop = false;
        }
    }


}
