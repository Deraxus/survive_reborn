using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Fliper_mini : MonoBehaviour
{
    public bool togl_mini = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Wall_WeaponTrue")
        {
            togl_mini = true;
        }
        else if (collision.gameObject.name == "Wall_WeaponFalse")
        {
            togl_mini = false;
        }
    }

}
