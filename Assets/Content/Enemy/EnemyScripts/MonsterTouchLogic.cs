using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTouchLogic : MonoBehaviour
{
    public GameObject mainEnemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision collision)
    {
        if (collision.gameObject.name == gameObject.name)
        {
            mainEnemy.GetComponent<AIPath>().canMove = false;
        }
    }

    private void OnCollisionExit2D(Collision collision)
    {
        if (collision.gameObject.name == gameObject.name)
        {
            mainEnemy.GetComponent<AIPath>().canMove = true;
        }
    }
}
