using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isVisionZone = false;

    public List<GameObject> enemyList = new List<GameObject>();

    public delegate void ChangeStateReturning(GameObject enemy);
    public delegate void ChangeStateAttack(GameObject enemy);

    public event ChangeStateReturning OnChangingReturning;
    public event ChangeStateAttack OnChangingAttack;

    public Vector3 startScale;

    void Start()
    {
        startScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "player") && (isVisionZone == false))
        {
            foreach (GameObject enemy in enemyList)
            {
                if (enemyList.Count > 0) {
                    try {
                        //enemy.gameObject.GetComponentInChildren<EnemyLogic>().OnChangingReturningVoid(enemy);
                        enemy.gameObject.GetComponentInChildren<EnemyLogic>().ChangeState(enemy.gameObject.GetComponentInChildren<EnemyLogic>().returningState);
                        //enemy.gameObject.GetComponentInChildren<EnemyLogic>().attackLogic.isInFight = false;
                    }
                    catch (NullReferenceException) {
                        continue;
                    }
                }
            }
        }

        else if ((collision.gameObject.tag == "player") && (enemyList.Count == 1) && (enemyList[0].GetComponentInChildren<EnemyLogic>().enemySpawnType == EnemyLogic.enemySpawnTypes.wild) && (isVisionZone))
        {
            //enemyList[0].gameObject.GetComponentInChildren<EnemyLogic>().attackLogic.isInFight = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "player"))
        {
            foreach (GameObject enemy in enemyList)
            {
                if (enemyList.Count > 0)
                {
                    try
                    {
                        //enemy.gameObject.GetComponentInChildren<EnemyLogic>().OnChangingAttackVoid(enemy);
                        enemy.gameObject.GetComponentInChildren<EnemyLogic>().ChangeState(enemy.gameObject.GetComponentInChildren<EnemyLogic>().attackState);
                        //enemy.gameObject.GetComponentInChildren<EnemyLogic>().attackLogic.isInFight = true;
                    }
                    catch (NullReferenceException){
                        continue;
                    }
                }
            }
        }

        else if ((collision.gameObject.tag == "player") && (enemyList.Count == 1) && (enemyList[0].GetComponentInChildren<EnemyLogic>().enemySpawnType == EnemyLogic.enemySpawnTypes.wild) && (isVisionZone)) {
            enemyList[0].gameObject.GetComponentInChildren<EnemyLogic>().ChangeState(enemyList[0].gameObject.GetComponentInChildren<EnemyLogic>().attackState);
            //enemyList[0].gameObject.GetComponentInChildren<EnemyLogic>().attackLogic.isInFight = true;
        }
    }
}
