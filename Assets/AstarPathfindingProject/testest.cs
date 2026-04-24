using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class testest : MonoBehaviour
{
    public DynamicGridObstacle obstacle;
    void Start()
    {
        obstacle = GetComponent<DynamicGridObstacle>();
    }

    // Update is called once per frame
    void Update()
    {
        if (obstacle != null) {
            AstarPath.active.UpdateGraphs(obstacle.bounds);  // Пересчитываем граф
        }
    }
}
