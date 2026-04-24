using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOnEnemyDamage : IBasicItem
{
    // Start is called before the first frame update
    void OnEnemyDamage(GameObject enemy, float damage = 0);
}
