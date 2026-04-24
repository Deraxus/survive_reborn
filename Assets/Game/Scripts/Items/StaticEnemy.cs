using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticEnemy
{
    // Start is called before the first frame update
    public static GameObject DetectEnemy(GameObject obj, List<GameObject> enemyes) {
        List<float> dlina = null;
        List<Vector2> positions = null;
        foreach (GameObject enemy in enemyes)
        {
            positions.Add(enemy.transform.position);
        }

        foreach (Vector2 pos in positions)
        {
            dlina.Add(pos.x + pos.y);
        }

        foreach (float otrezok in dlina)
        {
            
        }
        return null;
    }
}
