using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEffect : TemporaryEffect
{
    public int duration = 5;
    void Update()
    {
        
    }

    void PerSecEvent()
    {
        Debug.Log("Отравление");
        duration--;
        if (duration < 0) { 
        }
    }
}
