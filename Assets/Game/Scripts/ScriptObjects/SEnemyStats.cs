using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SEnemyStats : ScriptableObject
{
    [SerializeField] public float HP = 100;
    [SerializeField] public float Damage;
    [SerializeField] public float speed = 0.3f;
    [SerializeField] public int steps_limit = 1000;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
