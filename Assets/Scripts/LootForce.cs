using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootForce : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject obj;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("k")) {
            obj.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 10f);
        }
    }
}
