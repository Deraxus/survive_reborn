using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class SlowPassiveEffect : PassiveEffect
{
    public float slowCount = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEffectGetting(GameObject character)
    {
        if ((character.name) == "Player")
        {
            character.GetComponent<Player>().speed -= slowCount;
        }
    }

    void OnEffectEnding(GameObject character)
    {
        if ((character.name) == "Player")
        {
            character.GetComponent<Player>().speed += slowCount;
        }
    }
}
