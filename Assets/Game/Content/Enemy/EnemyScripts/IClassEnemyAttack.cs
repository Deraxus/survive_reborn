using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IClassEnemyAttack : MonoBehaviour
{
    [HideInInspector] public GameObject pos = null;
    [HideInInspector] public GameObject damageCloud = null;
    [HideInInspector] public float damage = 0f;

    public virtual bool StartAttack() {
        return true;
    }

    public IEnumerator AttackDelay(float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        GetComponent<AIPath>().canMove = true;
    }
}
