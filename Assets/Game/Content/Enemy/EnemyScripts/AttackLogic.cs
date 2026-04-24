using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class AttackLogic : MonoBehaviour
{
    [HideInInspector] public List<EnemyBaseAttack> avaliableMeleeAttacks;
    [HideInInspector] public List<EnemyBaseAttack> avaliableOtherAttacks;

    protected float otherAttackSeconds = 0f;

    public float randomOtherAttackPeriod = 20f;
    protected float startRandomOtherAttackPeriod;
    public bool isInFight = false;
    protected void CastAttack(EnemyBaseAttack attack, List<EnemyBaseAttack> attackList = null) {
        EnemyBaseAttack currentAttack;
        if ((attack == null) && (attackList != null))
        {
            currentAttack = attackList[UnityEngine.Random.Range(0, attackList.Count)];
        }
        else
        {
            currentAttack = attack;
        }
        currentAttack.StartAttack();
        Debug.Log($"�������� ���� {currentAttack.name}");
        GetComponent<DynamicGridObstacle>().enabled = true;
    }

    protected float GetRandomOtherAttackPeriod(float differenceKf = 3f)
    {
        float newValue = Random.Range(startRandomOtherAttackPeriod - (startRandomOtherAttackPeriod / differenceKf), startRandomOtherAttackPeriod + (startRandomOtherAttackPeriod / differenceKf));
        return newValue;
    }
}
