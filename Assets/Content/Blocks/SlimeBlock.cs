using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBlock : Block
{
    private GameObject effectManager;
    private GameObject player;

    public int slowCount = 1;
    void Start()
    {
        effectManager = GameObject.Find("MainManager").GetComponent<MainManager>().effectManager;
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            collision.gameObject.GetComponent<EffectController>().OnPassiveEffectGetting(effectManager.GetComponent<EffectManager>().slowPassiveEffectScript);
        }
    }
}
