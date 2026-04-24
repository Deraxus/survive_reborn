using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectBlockLogic : MonoBehaviour
{
    public bool isTouched = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "block") {
            isTouched = true;
        }
    }
}
