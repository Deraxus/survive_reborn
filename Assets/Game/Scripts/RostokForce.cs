using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RostokForce : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.MovePosition(new Vector2(transform.position.x + 0.01f, transform.position.y + 0.01f));
    }
}
