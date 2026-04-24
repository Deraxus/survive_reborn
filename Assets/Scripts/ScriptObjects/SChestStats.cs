using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu()]
public class SChestStats : ScriptableObject
{
    [SerializeField] public int rare = 0;

    public List<GameObject> LootCommon = new List<GameObject>();
    public List<GameObject> LootRare = new List<GameObject>();
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
