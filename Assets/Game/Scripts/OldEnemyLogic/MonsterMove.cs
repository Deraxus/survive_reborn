using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject keks_main, keks_second;
    private Vector2 vector;
    public float speed = 1;
    public int mode = 0; // 0 - если следует за игроком, 1 - если ищет обход
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        switch (mode)
        {
            case 0:
                vector = keks_main.transform.position;
                transform.position = Vector2.MoveTowards(transform.position, vector, speed);
                break;
            case 1:
                vector = keks_second.transform.position;
                transform.position = Vector2.MoveTowards(transform.position, vector, speed);
                break;
        }
    }
}
