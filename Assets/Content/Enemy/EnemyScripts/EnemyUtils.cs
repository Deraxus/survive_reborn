using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyUtils
{
    public static IEnumerator FullEnemyDeath(GameObject target, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject.Destroy(target);
        foreach (MonoBehaviour monoBehaviour in target.GetComponents<MonoBehaviour>())
        {
            monoBehaviour.StopAllCoroutines();
        }
        GameObject.Destroy(target.transform.parent.gameObject);
    }

    public static IEnumerator DamageEntity(GameObject enemy, GameObject damageCloud, float damage, GameObject pos, float animationDelay = 0f, float exitAnimationDelay = 0f)
    {
        enemy.GetComponent<Animator>().SetTrigger("IsAttacking");
        enemy.GetComponent<EnemyLogic>().isMoving = false;
        enemy.GetComponent<EnemyLogic>().isAttack = true;
        enemy.GetComponent<AIPath>().canMove = false;
        yield return new WaitForSeconds(animationDelay);
        Quaternion smth = new Quaternion(0, 0, 0, 0);
        Vector3 position = pos.transform.position;
        GameObject.Instantiate(damageCloud, position, smth).GetComponentInChildren<DamageCloudLogic>().damage = damage;
        yield return new WaitForSeconds(exitAnimationDelay);
        enemy.GetComponent<EnemyLogic>().isMoving = true;
        enemy.GetComponent<EnemyLogic>().isAttack = false;
        enemy.GetComponent<AIPath>().canMove = true;
    }

    public static void DisableFull(GameObject obj)
    {
        try
        {
            GameObject.Destroy(obj.transform.Find("Pos0").gameObject);
            GameObject.Destroy(obj.transform.Find("Pos1").gameObject);
            GameObject.Destroy(obj.transform.Find("Pos2").gameObject);
            GameObject.Destroy(obj.transform.Find("Pos3").gameObject);
        }
        catch { 
        }
        obj.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        obj.GetComponent<EnemyLogic>().enabled = false;
        obj.GetComponent<BoxCollider2D>().enabled = false;
        obj.GetComponent<Rigidbody2D>().simulated = false;
        foreach (EnemyBaseAttack script in obj.GetComponent<EnemyLogic>().attackState.enemyMeleeAttackList)
        { 
            script.enabled = false;
        }
        foreach (EnemyBaseAttack script in obj.GetComponent<EnemyLogic>().attackState.enemySpecialAttackList)
        { 
            script.enabled = false;
        }
        //obj.GetComponent<EnemyLogic>().attackLogic.enabled = false;
        //obj.GetComponent<EnemyLogic>().patrolLogic.enabled = false;
        if (obj.GetComponentInChildren<EnemyLogic>().enemySpawnType == EnemyLogic.enemySpawnTypes.wild) {
            WaveManager.Instance.spawnedEnemyes.Remove(obj.transform.parent.gameObject);
        }
        EventManager.Instance.OnEnemyDied(obj);
    }
}
