using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class EnemyClassicAttack : AttackLogic
{

    private void Start()
    {
        startRandomOtherAttackPeriod = randomOtherAttackPeriod;
        //avaliableMeleeAttacks = GetComponentInParent<EnemyLogic>().meleeEnemyAttacks;
        //avaliableOtherAttacks = GetComponentInParent<EnemyLogic>().otherEmenyAttacks;
    }
    // Start is called before the first frame update
    void OnEnable()
    {
        GetComponent<AIPath>().canMove = true;
    }

    // Update is called once per frame
    void OnDisable()
    {
        GetComponent<AIPath>().canMove = false;
    }

    private void FixedUpdate()
    {
        if ((gameObject.GetComponent<AIPath>().canMove == true) && (isInFight)) {
            otherAttackSeconds += Time.deltaTime;
        }
        if ((otherAttackSeconds >= randomOtherAttackPeriod) && (avaliableOtherAttacks.Count != 0)) {
            CastAttack(null, avaliableOtherAttacks);
            otherAttackSeconds = 0f;
            randomOtherAttackPeriod = GetRandomOtherAttackPeriod();
        }
    }
}
