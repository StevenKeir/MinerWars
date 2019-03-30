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

    private void OnEnable()
    {
        
         shopWindow = GameObject.FindGameObjectWithTag("ShopWindow");
    }

    private void Awake()
    {
       
    }

    private void Start()
    {
        //GameSettings.GS.shopWindow.SetActive(false);
        shopWindow.SetActive(false);
    }


    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.F) && someOneInShop && !shopOpen)
        {
            //GameSettings.GS.shopWindow.SetActive(true);
            shopWindow.SetActive(true);
            shopOpen = true;
            print("Shop opened");
        }
        else if (Input.GetKeyUp(KeyCode.F) && someOneInShop && shopOpen)
        {
            //GameSettings.GS.shopWindow.SetActive(false);
            shopWindow.SetActive(false);
            shopOpen = false;
        }
        else if (!someOneInShop)
        {
            shopWindow.SetActive(false);
            shopOpen = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Shop")
        {
            print("Someone Entered me ;)");
            someOneInShop = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Shop")
        {
            print("Someone Left me :(");
            someOneInShop = false;
        }
    }
}
