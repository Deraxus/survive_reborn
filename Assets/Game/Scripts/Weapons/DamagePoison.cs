using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePoison : Item
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            OnTaking();
        }
    }

    public void OnTaking()
    {
        DamageUp(1f, 10f);
    }

    void DamageUp(float damageKf, float duration)
    {
        GameObject player = GameObject.Find("Player");
        player.GetComponent<Player>().damageKf += damageKf;
        StartCoroutine(DamageDown(damageKf, duration));
    }


    IEnumerator DamageDown(float damageKf, float duration)
    {
        yield return new WaitForSeconds(duration);
        GameObject player = GameObject.Find("Player");
        player.GetComponent<Player>().damageKf -= damageKf;
        Destroy(gameObject);
    }
}
