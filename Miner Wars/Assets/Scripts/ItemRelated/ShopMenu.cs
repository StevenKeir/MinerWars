using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMenu : MonoBehaviour
{

    private PhotonView PV;
    public GameObject shopWindow;
    public bool someOneInShop = false;
    public bool shopOpen = false;

    private void Awake()
    {
        shopWindow = GameSettings.GS.shopWindow;
    }

    private void Start()
    {
        PV = GetComponent<PhotonView>();
        shopWindow.SetActive(false);
    }

    private void Update()
    { 
        //If the game is running and we are the local player then open the shop.
        if (PV.IsMine && GameSettings.GS.isGameRunning == true)
        {
            if (Input.GetKeyUp(KeyCode.F) && someOneInShop && !shopOpen)
            {
                shopWindow.SetActive(true);
                shopOpen = true;
                print("Shop opened");
                GameSettings.GS.enterShop.Play();
            }
            else if (Input.GetKeyUp(KeyCode.F) && someOneInShop && shopOpen)
            {
                shopWindow.SetActive(false);
                shopOpen = false;
            }
            else if (!someOneInShop)
            {
                shopWindow.SetActive(false);
                shopOpen = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Shop")
        {
            someOneInShop = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Shop")
        {
            someOneInShop = false;
        }
    }
}
