using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterVision : MonoBehaviour
{
    public bool agro;
    private bool vtrigere;
    // Start is called before the first frame update
    void Start()
    {
        agro = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (vtrigere == true)
        {
            agro = true;
        }
        else
        {
            agro = false;
        }*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "player")
        {
            agro = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "player")
        {
            agro = false;
        }
    }
}
