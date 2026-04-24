using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCollision : MonoBehaviour
{
    public int tog = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals("FullRoom")) {
            Debug.Log("2 комнаты столкнулись");
            tog = 0;
        }
    }
}
