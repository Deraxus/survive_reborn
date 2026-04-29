using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class LaserAttack : EnemyBaseAttack
{
    public GameObject laserObj;

    private LineRenderer lineRenderer;

    // ����� ������ ����� ������� ����� �������� ���������� ����� - ������ �������� �������
    public GameObject eyeTrigger;

    public Vector2 playerPos;
    public float laserPreparingDelay = 0.5f;
    public float laserAfterDelay = 1f;
    public float afkAfterAttack = 0.5f;

    public GameObject startLaser;
    public GameObject midLaser;
    public GameObject endLaser;

    public float laserDistance;

    public override void StartAttack()
    {
        lineRenderer = laserObj.GetComponent<LineRenderer>();
        Debug.Log("������� �����");
        base.StartAttack();
        GetComponent<Animator>().SetBool("preparingForAttack", true);
        StartCoroutine(techStartAttack(laserPreparingDelay, laserAfterDelay, afkAfterAttack));
    }

    public override void StopAttack()
    {
        base.StopAttack();
    }

    private IEnumerator techStartAttack(float delay1, float delay2, float delay3)
    {
        GetComponent<AIPath>().canMove = false;
        Debug.Log("������� �����");
        playerPos = MainManager.Instance.mainPlayer.transform.position;
        Vector2 techVector = (playerPos - (Vector2)transform.position).normalized;
        yield return new WaitForSeconds(delay1);

        RaycastHit2D ray = Physics2D.Raycast(transform.position, techVector, 1000, LayerMask.GetMask("Blocks"));

        lineRenderer.SetPosition(1, techVector * ray.distance);

        // ������ ������

        // ������� ��� ����� �� �������
        //Transform correctLaser = laserObj.transform.GetChild(0);
        //correctLaser.GetComponentInChildren<fullLaserLogic>().PrepareLaser(eyeTrigger.transform.position);
        //correctLaser.localScale = new Vector2(ray.distance * -1, correctLaser.localScale.y);


        RaycastHit2D damageRay = Physics2D.Raycast(transform.position, techVector, 1000, LayerMask.GetMask("Blocks", "Player"));
        Debug.Log($"��������� - {damageRay.transform.name}");

        if (damageRay.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            EventManager.Instance.OnPlayerDamaged(gameObject, damage);
            Debug.Log("���������");
        }

        laserObj.GetComponent<LineRenderer>().enabled = true;
        AudioSource.PlayClipAtPoint(currentAttackSound, transform.position);
        Debug.Log("������� �������");
        Quaternion smth = new Quaternion(0, 0, 0, 0);
        //Vector3 position = pos.transform.position;
        //Vector2 position = gameObject.transform.position;
        yield return new WaitForSeconds(delay2);
        laserObj.GetComponent<LineRenderer>().enabled = false;
        yield return new WaitForSeconds(delay3);
        GetComponent<Animator>().SetBool("preparingForAttack", false);
        GetComponent<AIPath>().canMove = true;
        StopAttack();
    }
}
