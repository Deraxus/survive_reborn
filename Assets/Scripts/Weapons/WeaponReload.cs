using System.Collections;
using UnityEngine;

public class WeaponReload : MonoBehaviour
{
    public int patrons = 10;
    // Start is called before the first frame update
    void Start()
    {
        //patrons = GameObject.Find("Stats").GetComponent<WeaponStats>().patrons;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (patrons <= 0) {
            Invoke("Reload", 3);
        }
        else if (Input.GetKey("r")) {
            Invoke("Reload", 3);
        }*/
    }

    void Reload() { 
        
    }
}
