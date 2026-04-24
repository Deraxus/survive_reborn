using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class ReturningState : MonoBehaviour, IEnemyState
{
    public void Enter()
    {
        Debug.Log("Перешел в режим возвращения");

        GetComponent<AIDestinationSetter>().target = GetComponent<EnemyLogic>().homeObject.transform;
        GetComponent<AIPath>().canMove = true;
    }

    public void Exit()
    {
        Debug.Log("Покинул режим возвращения");

        GetComponent<AIDestinationSetter>().target = MainManager.Instance.mainPlayer.transform;
        GetComponent<AIPath>().canMove = false;
    }

    public void StateUpdate()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject == GetComponent<EnemyLogic>().homeObject) && (GetComponent<EnemyLogic>().currentEnemyState == (IEnemyState)this))
        {
            Debug.Log("Должен остановиться");
            GetComponent<EnemyLogic>().ChangeState(GetComponent<EnemyLogic>().patrolState);
        }
    }

}
