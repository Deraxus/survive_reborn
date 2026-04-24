using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseAttack : MonoBehaviour, IEnemyAttack
{
    public enum AttackType
    {
        Melee,
        Range,
        Special,
    }

    public AttackType attackType;
    public GameObject pos;
    
    public float damage;
    public float cooldown;

    public List<AudioClip> attackSounds;
    
    [HideInInspector] public AudioClip currentAttackSound;
    [HideInInspector] public AttackState attackState;
    public virtual void Awake()
    {
        attackState = GetComponent<EnemyLogic>().attackState;
    }
    public virtual void StartAttack()
    {
        if (attackSounds.Count != 0)
        {
            currentAttackSound = attackSounds[Random.Range(0, attackSounds.Count)];
        }
        attackState.isAttacking = true;
        //if (attackType == AttackType.Melee) {attackState.readyToMeleeAttack = false;}
        if ((attackType == AttackType.Range) || (attackType == AttackType.Special)) {attackState.readyToNotMeleeAttack = false;}
    }

    public virtual void StopAttack()
    {
        attackState.isAttacking = false;
    }
}
