using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using Pathfinding;
using UnityEngine;
using static EnemyLogic;

public class AttackState : MonoBehaviour, IEnemyState
{
    public float attackCooldownCast = 5f;
    public float specialAttackCooldown = 10f;

    public List<EnemyBaseAttack> enemyAttackList = new List<EnemyBaseAttack>();
    
    [HideInInspector] public float localTimer = 0;
    private EnemyLogic localEnemyLogic;

    public List<EnemyBaseAttack> enemyMeleeAttackList = new List<EnemyBaseAttack>();
    public List<EnemyBaseAttack> enemyRangeAttackList = new List<EnemyBaseAttack>();
    public List<EnemyBaseAttack> enemySpecialAttackList = new List<EnemyBaseAttack>();
    
    [HideInInspector] public bool readyToMeleeAttack = false;
    [HideInInspector] public bool readyToNotMeleeAttack = false;

    [HideInInspector] public bool isAttacking = false;

    [HideInInspector] private bool enemyHasMeleeAttacks = false;
    [HideInInspector] private bool enemyHasRangeAttacks = false;
    [HideInInspector] private bool enemyHasSpecialAttacks = false;
    

    public virtual void Awake()
    {
        localEnemyLogic = GetComponent<EnemyLogic>();

        if (attackCooldownCast == 0)
        {
            attackCooldownCast = 0.01f;
        }

        foreach (EnemyBaseAttack attack in enemyAttackList)
        {
            switch (attack.attackType)
            {
                case EnemyBaseAttack.AttackType.Melee:
                    enemyMeleeAttackList.Add(attack);
                    break;
                case EnemyBaseAttack.AttackType.Range:
                    enemyRangeAttackList.Add(attack);
                    break;
                case EnemyBaseAttack.AttackType.Special:
                    enemySpecialAttackList.Add(attack);
                    break;
            }
        }
        
        if (enemyMeleeAttackList.Count > 0 && enemyRangeAttackList.Count == 0 && enemySpecialAttackList.Count == 0)
        {
            readyToMeleeAttack = true;
        }

        if (enemyMeleeAttackList.Count > 0)
        {
            enemyHasMeleeAttacks = true;
        }
        if (enemyRangeAttackList.Count > 0)
        {
            enemyHasRangeAttacks = true;
        }
        if (enemySpecialAttackList.Count > 0)
        {
            enemyHasSpecialAttacks = true;
        }
    }
    public virtual void Enter()
    {

        if (localEnemyLogic.enemySpawnType == enemySpawnTypes.camping)
        {
            //detectObject.transform.localScale = new Vector2(detectObject.transform.localScale.x * 2, detectObject.transform.localScale.y * 2);
            localEnemyLogic.detectObject.transform.localScale = localEnemyLogic.detectObject.GetComponent<DetectPlayer>().startScale * 2;
        }

        GetComponent<AIPath>().canMove = true;
    }

    public virtual void StateUpdate()
    {
        if (localTimer >= attackCooldownCast)
        {
            if (enemyHasRangeAttacks || enemyHasSpecialAttacks)
            {
                List<EnemyBaseAttack> allAttackList = new List<EnemyBaseAttack>();
                if (enemyHasRangeAttacks) { allAttackList.AddRange(enemyRangeAttackList); }
                if (enemyHasSpecialAttacks) { allAttackList.AddRange(enemySpecialAttackList); }

                readyToNotMeleeAttack = true;
                localTimer = 0;
            }
            
        }
        else if ((!readyToNotMeleeAttack) && (!isAttacking))
        {
            localTimer += Time.deltaTime;
        }
    }
    public virtual void Exit()
    {
        Debug.Log("Выхожу из фазы атаки");
        GetComponent<AIPath>().canMove = false;

        if (localEnemyLogic.enemySpawnType == enemySpawnTypes.camping)
        {
            //detectObject.transform.localScale = new Vector2(detectObject.transform.localScale.x * 2, detectObject.transform.localScale.y * 2);
            localEnemyLogic.detectObject.transform.localScale = localEnemyLogic.detectObject.GetComponent<DetectPlayer>().startScale;
        }
    }

    private EnemyBaseAttack ChooseRandomAttack(EnemyBaseAttack.AttackType attackType)
    {
        switch (attackType)
        {
            case EnemyBaseAttack.AttackType.Melee:
                return enemyMeleeAttackList[UnityEngine.Random.Range(0, enemyMeleeAttackList.Count)];
            case EnemyBaseAttack.AttackType.Range:
                return enemyRangeAttackList[UnityEngine.Random.Range(0, enemyRangeAttackList.Count)];
            case EnemyBaseAttack.AttackType.Special:
                return enemySpecialAttackList[UnityEngine.Random.Range(0, enemySpecialAttackList.Count)];
        }
        return null;
    }

    public virtual void CastAttack(EnemyBaseAttack attack = null, List<EnemyBaseAttack> attackList = null)
    {
        Debug.Log("Кастую атаку");
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
        GetComponent<DynamicGridObstacle>().enabled = true;
    }
}
