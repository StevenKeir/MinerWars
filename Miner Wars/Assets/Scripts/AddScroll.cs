using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddScroll : MonoBehaviour
{

    public GameObject add1;
    public GameObject add2;
    public GameObject add3;
    public int addNo;
    public float displayTimer = 0;
    public float switchTime;

    // Start is called before the first frame update
    void Start()
    {
        add1.SetActive(true);
        add2.SetActive(false);
        add3.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        displayTimer += Time.deltaTime;

        if (displayTimer >= switchTime)
        {
            if (addNo != 2)
                {
                addNo++;
            }
            else
            {
                addNo = 0;
            }
            displayTimer = 0;
        }

        if (addNo == 0)
        {
            add1.SetActive(true);
            add2.SetActive(false);
            add3.SetActive(false);
        }
        if (addNo == 1)
        {
            add1.SetActive(false);
            add2.SetActive(true);
            add3.SetActive(false);
        }
        if (addNo == 2)
        {
            add1.SetActive(false);
            add2.SetActive(false);
            add3.SetActive(true);
        }

    }
}
