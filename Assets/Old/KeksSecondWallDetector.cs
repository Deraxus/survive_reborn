using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeksSecondWallDetector : MonoBehaviour
{
    public int rotation = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "block" && rotation == 0)
        {
            rotation = 2;
            Debug.Log("Стою в блоке");
        }
        else if (collision.transform.tag == "block" && rotation == 2)
        {
            rotation = 0;
            Debug.Log("Стою в блоке");
        }
    }
}
