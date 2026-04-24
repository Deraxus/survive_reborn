using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class EyeAttackState : AttackState
{
    [Tooltip("������, ����������� ��� ����� �������")]
    public float rangeForAttack = 10f;

    public LayerMask layerMask;

    private bool targetInVision = false;
    
    public override void StateUpdate()
    {
        Vector2 direction = (MainManager.Instance.mainPlayer.transform.position - transform.position).normalized;

        base.StateUpdate();

        float distance = Vector2.Distance(transform.position, MainManager.Instance.mainPlayer.transform.position);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, layerMask);
        Debug.DrawRay(transform.position, direction * distance, Color.red);

        if (distance <= rangeForAttack)
        {
            targetInVision = true;
        }
        else
        {
            targetInVision = false;
        }

        if ((targetInVision) && (readyToNotMeleeAttack) && (hit.collider != null) && (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player")))
        {
            // Выбираем лазер - он всегда первый в списке (потом можно пофиксить и сделать более гибко)
            EnemyBaseAttack currentAttack = enemyRangeAttackList[0];
            CastAttack(currentAttack);
        }
    }
}
