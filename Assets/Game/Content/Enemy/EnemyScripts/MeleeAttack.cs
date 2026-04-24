using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MeleeAttack : EnemyBaseAttack
{
    public GameObject damageCloud;

    public float publicDelay1 = 0f;
    public float publicDelay2 = 0f;
    void Start()
    {
        attackState = GetComponent<EnemyLogic>().attackState;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void StartAttack()
    {
        base.StartAttack();
        StartCoroutine(techStartAttack(publicDelay1, publicDelay2));
    }

    public override void StopAttack()
    {
        base.StopAttack();
    }

    private IEnumerator techStartAttack(float delay1, float delay2) {
        GetComponent<EnemyLogic>().isAttack = true;
        GetComponent<AIPath>().canMove = false;
        yield return new WaitForSeconds(delay1);
        Quaternion smth = new Quaternion(0, 0, 0, 0);
        Vector3 position = pos.transform.position;
        //Vector2 position = gameObject.transform.position;
        Instantiate(damageCloud, position, smth).GetComponentInChildren<DamageCloudLogic>().damage = damage;
        yield return new WaitForSeconds(delay2);
        GetComponent<AIPath>().canMove = true;
        GetComponent<EnemyLogic>().isAttack = false;
        StopAttack();
    }
}
