using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : Item
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTaking()
    {
        
    }

    void DamageUp(float damageKf, float duration) {
        GameObject player = GameObject.Find("Player");
        player.GetComponent<Player>().damageKf += damageKf;
    }



    IEnumerator DamageDown(float damageKf, float duration) {
        yield return new WaitForSeconds(duration);
        GameObject player = GameObject.Find("Player");
        player.GetComponent<Player>().damageKf -= damageKf;
    }
}
