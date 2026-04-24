using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPosLogic : MonoBehaviour
{
    private EnemyLogic enemyLogic;

    private void Awake()
    {
        enemyLogic = GetComponentInParent<EnemyLogic>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.gameObject == MainManager.Instance.mainPlayer) && (enemyLogic.currentEnemyState == (IEnemyState)enemyLogic.attackState)) {
            if (enemyLogic.attackState.enemyMeleeAttackList.Count != 0)
            {
                enemyLogic.attackState.enemyMeleeAttackList[0].pos = gameObject;
                if (enemyLogic.attackState.readyToMeleeAttack && enemyLogic.isAttack == false)
                {
                    enemyLogic.attackState.CastAttack(attackList : enemyLogic.attackState.enemyAttackList);
                }
            }
        }
    }
}
