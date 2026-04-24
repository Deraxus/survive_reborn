using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanSpawnCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals("EnemyZone"))
        {
            collision.gameObject.GetComponentInParent<RoomInfo>().canSpawn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name.Equals("EnemyZone"))
        {
            collision.gameObject.GetComponentInParent<RoomInfo>().canSpawn = false;
        }
    }
}
