using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationSpawn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DisableFull(GameObject obj)
    {
        obj.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        obj.GetComponent<BoxLogic>().enabled = false;
        obj.GetComponent<BoxCollider2D>().enabled = false;
        //obj.GetComponent<Rigidbody2D>().simulated = false;
    }
}
