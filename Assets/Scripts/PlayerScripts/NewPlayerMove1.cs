using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerMove1 : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 5.0f;
    Vector2 vector;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        vector.x = Input.GetAxisRaw("Horizontal");
        vector.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = vector + rb.position * speed * Time.fixedDeltaTime;
    }
}
