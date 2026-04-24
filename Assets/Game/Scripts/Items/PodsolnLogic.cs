using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodsolnLogic : MonoBehaviour
{
    private float seconds;

    public float sunGivePeriod = 10;
    void Start()
    {
        StartCoroutine(SunGive());
    }

    // Update is called once per frame
    void Update()
    {
        seconds += Time.deltaTime;

    }

    void SpawnSun() {
        StartCoroutine(GetComponent<Loot>().LootDropCor());
    }

    IEnumerator SunGive() {
        yield return new WaitForSeconds(sunGivePeriod);
        SpawnSun();
        StartCoroutine(SunGive());

    }
}
