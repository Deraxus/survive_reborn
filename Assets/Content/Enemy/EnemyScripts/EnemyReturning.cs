using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReturning : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        GetComponent<AIDestinationSetter>().target = GetComponent<EnemyLogic>().homeObject.transform;
        GetComponent<AIPath>().canMove = true;
    }

    // Update is called once per frame
    void OnDisable()
    {
        GetComponent<AIDestinationSetter>().target = GameObject.Find("Player").transform;
        GetComponent<AIPath>().canMove = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject == GetComponent<EnemyLogic>().homeObject) && (this.enabled)) {
            Debug.Log("Должен остановиться");
            GetComponent<EnemyLogic>().ChangeState(GetComponent<EnemyLogic>().patrolState);
        }
    }
}
