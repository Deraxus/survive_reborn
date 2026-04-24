using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_move : MonoBehaviour
{
    public float speed = 5;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w"))
        {
            transform.Translate(new Vector3(0, 1, 0) * Time.deltaTime * speed);
        }
        if (Input.GetKey("s"))
        {
            transform.Translate(new Vector3(0, -1, 0) * Time.deltaTime * speed);
        }
        if (Input.GetKey("a"))
        {
            transform.Translate(new Vector3(-1, 0, 0) * Time.deltaTime * speed);
        }
        if (Input.GetKey("d"))
        {
            transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime * speed);
        }
    }
}
