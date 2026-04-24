using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MeleeSuperAttack : IClassEnemyAttack
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool StartAttack()
    {
        GetComponent<AIPath>().canMove = false;
        StartCoroutine(AttackDelay(2));
        Debug.Log("Супер милишная атака");
        return true;
    }
}
