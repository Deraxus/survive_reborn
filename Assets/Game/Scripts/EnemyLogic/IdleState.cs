using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdle : IEnemyState
{
    public void Enter()
    {
        Debug.Log("Перешел в режим простоя");
    }

    public void StateUpdate()
    {

    }
    public void Exit()
    {
        Debug.Log("Покинул режим простоя");
    }

}
