using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public List<GameObject> PublicEnemy = new List<GameObject>();
    public List<GameObject> CurrentSpawners;
    public int SpawnTimer = 5;
    public bool isOnCollider = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "ColliderEnemySpawn")
        {
            isOnCollider = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "ColliderEnemySpawn")
        {
            isOnCollider = false;
        }
    }

}
