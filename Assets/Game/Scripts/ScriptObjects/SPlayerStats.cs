using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu()]
public class SPlayerStats : ScriptableObject
{
    [SerializeField] public float HP = 100;
    [SerializeField] public float speed = 3f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

