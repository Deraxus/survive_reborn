using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemMagnet : MonoBehaviour
{
    public SUIData data;
    bool isMagnet = false;
    GameObject player;
    public GameObject magnetCollider;
    private Rigidbody2D rb;
    private float timer = 0.8f;
    public float speed = 10;
    public bool reverse = true;
    void Start()
    {
        player = GameObject.Find("MainManager").GetComponent<MainManager>().mainPlayer;
        magnetCollider = player.transform.Find("MagnetCollider").gameObject;
        rb = GetComponent<Rigidbody2D>();
        if (data == null)
        {
            data = SUIData.CreateInstance<SUIData>();
        }
        speed = data.magnetSpeed;
        this.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isMagnet)
        {
            timer += Time.fixedDeltaTime;
            rb.linearVelocity = ((player.transform.position - transform.position).normalized * Mathf.Pow(timer, 10) * speed);

        }
    }

    void OnTriggerEnter2D(Collider2D collison)
    {
        if (collison.gameObject == magnetCollider)
        {
            isMagnet = true;
        }
    }
}
