using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    void Awake()
    {
        Utils.InitializeCoroutine(this);
    }
    public void StartGlobalCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}
