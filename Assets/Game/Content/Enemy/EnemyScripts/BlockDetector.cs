using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class BlockDetector : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject Enemy;
    DynamicGridObstacle grid;

    void Awake()
    {
        grid = Enemy.GetComponent<DynamicGridObstacle>();  
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "block")
        {
            Enemy.GetComponent<EnemyLogic>().IsBlock = true;
        }
        if (collision.gameObject.name == "ColliderEnemyGridFilled")
        {
            Enemy.layer = 3;
            grid.enabled = true;
            grid.DoUpdateGraphs();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "ColliderEnemyGridFilled")
        {
            Enemy.layer = 6;
            grid.enabled = false;
            grid.DoUpdateGraphs();
        }
    }
}
