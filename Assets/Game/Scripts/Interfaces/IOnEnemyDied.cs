using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOnEnemyDied : IBasicItem
{
    // Start is called before the first frame update
    void OnEnemyDied(GameObject enemy);
}
