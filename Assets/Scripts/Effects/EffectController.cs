using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    [Tooltip("Список, в котором находятся временные эффекты персонажа")]
    public List<TemporaryEffect> temporaryEffects = new List<TemporaryEffect>();

    public List<PassiveEffect> passiveEffects = new List<PassiveEffect>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPassiveEffectGetting(PassiveEffect passiveEffect, float effectCount = 0)
    {
        if (passiveEffect == null)
        {
            if (passiveEffect is SlowPassiveEffect)
            {
            }
            passiveEffect.OnEffectGetting(gameObject);
        }
    }
}
