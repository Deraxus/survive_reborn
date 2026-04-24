using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : MonoBehaviour, IEnemyState
{
    public enum currentStates { idle, patrol };
    public currentStates currentState = currentStates.idle;
    public float localTimer = 0f;
    public float patrolTime = 3f;
    public float randomPatrolKf = 5f;
    public float idleTime = 2f;

    public float speed = 1f;
    public float randomSpeedKf = 5f;
    public float physKf = 1f;

    public int direction;
    int old_direction;
    int randStay;

    private Rigidbody2D rb;

    public Vector2 vector;

    public void Enter()
    {
        Debug.Log("������� � ����� �������");

        rb = gameObject.GetComponent<Rigidbody2D>();
        speed = Random.Range(speed - speed / randomSpeedKf, speed + speed / randomSpeedKf);
        patrolTime = Random.Range(patrolTime - patrolTime / randomPatrolKf, patrolTime + patrolTime / randomPatrolKf);
    }

    public void Exit()
    {
        Debug.Log("������� ����� �������");
    }



    public void StateUpdate()
    {
        if ((localTimer >= patrolTime) && (currentState == currentStates.patrol))
        {
            currentState = currentStates.idle;
            localTimer = 0f;
        }

        else if ((localTimer >= idleTime) && (currentState == currentStates.idle))
        {
            currentState = currentStates.patrol;
            localTimer = 0f;
        }

        if (currentState == currentStates.idle)
        {
            Idle();
        }

        else if (currentState == currentStates.patrol)
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (localTimer == 0f)
        {
            patrolTime = Random.Range(patrolTime - patrolTime / randomPatrolKf, patrolTime + patrolTime / randomPatrolKf);
            direction = Random.Range(0, 8);
            while (direction == old_direction || direction > old_direction - 2 && direction < old_direction + 2)
            {
                direction = Random.Range(0, 8);
            }
            switch (direction)
            {
                case 0:
                    vector = new Vector2(0, 1);
                    break;
                case 1:
                    vector = new Vector2(1, 1);
                    break;
                case 2:
                    vector = new Vector2(1, 0);
                    break;
                case 3:
                    vector = new Vector2(1, -1);
                    break;
                case 4:
                    vector = new Vector2(0, -1);
                    break;
                case 5:
                    vector = new Vector2(-1, -1);
                    break;
                case 6:
                    vector = new Vector2(-1, 0);
                    break;
                case 7:
                    vector = new Vector2(-1, 1);
                    break;
            }
        }
        rb.linearVelocity = vector * speed * physKf;
        localTimer += Time.fixedDeltaTime;
        old_direction = direction;
    }

    void Idle()
    {
        localTimer += Time.fixedDeltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.name == "StopEnemyBlock") && (currentState == currentStates.patrol))
        {
            currentState = currentStates.idle;
            localTimer = 0f;
        }
    }
}
