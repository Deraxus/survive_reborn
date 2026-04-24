using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponFeature : ScriptableObject
{
    public virtual void OnEnemyDamage(GameObject enemy, float damage = 0) { }
    public virtual void OnEnemyDied(GameObject enemy){ }
    public virtual void OnShoot(GameObject bullet) { }
    public virtual void OnReload(GameObject weapon) { }

    // Когда достаем оружие, то есть берем оружие в активный слот
    public virtual void OnWeaponHandTaking()
    {
        EventManager.Instance.OnEnemyDamageEvent += OnEnemyDamage;
        EventManager.Instance.OnReloadEvent += OnReload;
        EventManager.Instance.OnShootEvent += OnShoot;
        EventManager.Instance.OnEnemyDiedEvent += OnEnemyDied;
    }

    // Когда убираем оружие, то есть меняем активный слот на другое оружие
    public virtual void OnWeaponHandLoosing()
    {
        EventManager.Instance.OnEnemyDamageEvent -= OnEnemyDamage;
        EventManager.Instance.OnReloadEvent -= OnReload;
        EventManager.Instance.OnShootEvent -= OnShoot;
        EventManager.Instance.OnEnemyDiedEvent -= OnEnemyDied;
    }

    public virtual void OnWeaponTaking()
    {
    }
    public virtual void OnWeaponLoosing()
    {
    }
}
