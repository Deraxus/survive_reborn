using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Testttt : MonoBehaviour
{
    public Tilemap tilemap;
    public Grid grid;

    public GameObject whatSpawn;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B)) {
            Instantiate(whatSpawn, transform.position + new Vector3(0, 0, 0), new Quaternion(0,0,0,0));
            Debug.Log($"Спавн {transform.position}");
        }   
    }
}
