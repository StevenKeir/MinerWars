using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMenu : MonoBehaviour
{





    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && Input.GetKey(KeyCode.F))
        {
            print("Shop opened by " + collision.gameObject.name);
        }
    }


}
