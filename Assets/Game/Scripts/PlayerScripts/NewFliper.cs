using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewFliper : MonoBehaviour
{
    private GameObject player;

    public GameObject center;

    private bool togl;
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.position.x <= center.transform.position.x) && (togl == false))
        {
            player.transform.localScale = new Vector2(-1, player.transform.localScale.y);
            togl = true;
        }
        else if ((transform.position.x >= center.transform.position.x) && (togl == true)) {
            player.transform.localScale = new Vector2(1, player.transform.localScale.y);
            togl = false;
        }
    }
}
