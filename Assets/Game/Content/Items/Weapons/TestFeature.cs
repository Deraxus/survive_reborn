using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class TestFeature : WeaponFeature
{
    public override void OnReload(GameObject bullet)
    {
        base.OnReload(bullet);

        Debug.Log("Это моя фишка!");
    }

    public override void OnEnemyDied(GameObject enemy)
    {
        base.OnEnemyDied(enemy);
        
        Debug.Log($"{enemy.name} ПОМЕР");
    }

    public override void OnShoot(GameObject bullet)
    {
        base.OnShoot(bullet);
        
        bullet.GetComponentInChildren<SpriteRenderer>().color = Color.green;
    }
}
