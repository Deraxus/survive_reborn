using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class ExplosionLogic : MonoBehaviour
{
    public float damage;
    private float seconds;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        seconds += Time.deltaTime;
        if (seconds >= 0.3f)
        {
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "Enemy"))
        {
            Utils.DamageEnemy(collision.gameObject, damage);
        }
    }
}
