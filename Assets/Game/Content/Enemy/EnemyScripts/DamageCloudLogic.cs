using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCloudLogic : MonoBehaviour
{
    public float damage;
    public GameObject parentEnemy;

    private float seconds;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        seconds += Time.deltaTime;
        if (seconds >= 0.2f) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            EventManager.Instance.OnPlayerDamaged(gameObject, damage);
            Destroy(gameObject);
            Debug.Log(1);
        }
    }
}
