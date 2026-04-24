using System;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AttackSpiderSpawn : MonoBehaviour, IEnemyAttack
{
    public GameObject microEnemySpawn;

    private EnemyLogic enemyLogic;
    private AttackState attackState;

    private void Awake()
    {
        enemyLogic = GetComponent<EnemyLogic>();
        attackState = enemyLogic.attackState;
    }

    public void StartAttack()
    {
        StartCoroutine(TechSpawn(1.5f));
    }

    public void StopAttack()
    {
        attackState.isAttacking = false;
    }

    private IEnumerator TechSpawn(float spawnPeriod) {
        GetComponent<AIPath>().canMove = false;
        int randInt = Random.Range(0, 4); // ������� ����� ������
        Vector3 targetCell = Utils.GetNearbyCell(gameObject);
        for (int i = 0; i <= randInt; i++)
        {
            switch (randInt) {
                case 3:
                    if (transform.localScale.x > 0)
                    {
                        switch (i)
                        {
                            case 0:
                                targetCell.y = targetCell.y + 2f;
                                break;
                            case 1:
                                targetCell.y = targetCell.y - 1f;
                                targetCell.x = targetCell.x + 2f;
                                break;
                            case 2:
                                targetCell.y = targetCell.y - 1f;
                                break;
                            case 3:
                                targetCell.y = targetCell.y - 2f;
                                targetCell.x = targetCell.x - 2f;
                                break;

                        }
                    }
                    else
                    {
                        switch (i)
                        {
                            case 0:
                                targetCell.y = targetCell.y + 2f;
                                break;
                            case 1:
                                targetCell.y = targetCell.y - 1f;
                                targetCell.x = targetCell.x - 2f;
                                break;
                            case 2:
                                targetCell.y = targetCell.y - 1f;
                                break;
                            case 3:
                                targetCell.y = targetCell.y - 2f;
                                targetCell.x = targetCell.x + 2f;
                                break;

                        }
                    }
                    break;
                case 2:
                    if (transform.localScale.x > 0)
                    {
                        switch (i)
                        {
                            case 0:
                                targetCell.y = targetCell.y + 2f;
                                break;
                            case 1:
                                targetCell.y = targetCell.y - 2f;
                                targetCell.x = targetCell.x + 3f;
                                break;
                            case 2:
                                targetCell.y = targetCell.y - 2f;
                                targetCell.x = targetCell.x - 3f;
                                break;
                        }
                    }
                    else
                    {
                        switch (i)
                        {
                            case 0:
                                targetCell.y = targetCell.y + 2f;
                                break;
                            case 1:
                                targetCell.y = targetCell.y - 2f;
                                targetCell.x = targetCell.x - 3f;
                                break;
                            case 2:
                                targetCell.y = targetCell.y - 2f;
                                targetCell.x = targetCell.x + 3f;
                                break;
                        }
                    }
                    break;
                case 1:
                    if (transform.localScale.x > 0)
                    {
                        switch (i)
                        {
                            case 0:
                                targetCell.y = targetCell.y + 2f;
                                break;
                            case 1:
                                targetCell.y = targetCell.y - 4f;
                                break;

                        }
                    }
                    else {
                        switch (i)
                        {
                            case 0:
                                targetCell.y = targetCell.y + 2f;
                                break;
                            case 1:
                                targetCell.y = targetCell.y - 4f;
                                break;

                        }
                    }
                    break;
                case 0:
                    if (transform.localScale.x > 0)
                    {
                        targetCell.x = targetCell.x + 2f;
                    }
                    else {
                        targetCell.x = targetCell.x - 2f;
                    }
                    break;
            }
           
            // ��������� �������� �������
            yield return new WaitForSeconds(spawnPeriod);
            Utils.SpawnTarget(microEnemySpawn, targetCell);

        }
        yield return new WaitForSeconds(1f);
        StopAttack();
        GetComponent<AIPath>().canMove = true;
    }


}
