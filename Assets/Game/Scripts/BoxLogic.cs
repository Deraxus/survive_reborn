using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxLogic : MonoBehaviour
{
    public float HP = 50;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0)
        {
            BoxDeath();
        }
    }

    void BoxDeath() {
        Transform pos = transform;
        int randInt = Random.Range(GetComponent<Loot>().minCoinCount, GetComponent<Loot>().maxCoinCount);
        StartCoroutine(GetComponent<Loot>().LootDropCor(mode: 1, whereSpawn: pos.position, dropCount: randInt));
        DisableFull(gameObject);
        StartCoroutine(FullBoxDeath(5.2f));
    }

    void DisableFull(GameObject obj)
    {
        obj.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        obj.GetComponent<BoxLogic>().enabled = false;
        obj.GetComponent<BoxCollider2D>().enabled = false;
        //obj.GetComponent<Rigidbody2D>().simulated = false;
    }

    IEnumerator FullBoxDeath(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
