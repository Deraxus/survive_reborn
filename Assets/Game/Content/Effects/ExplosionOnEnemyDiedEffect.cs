using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionOnEnemyDiedEffect : WeaponFeature, IOnEnemyDied
{
    public GameObject publicExplosion;

    private MainItemManager mainItemManager;
    
    public float startExplosionDamage = 10f;
    
    public float explosionChance = 1;
    
    public void OnItemTaking()
    {
        //GameObject.Find("EventManager").GetComponent<EventManager>().OnEnemyDiedEvent += OnEnemyDied;
        mainItemManager = MainItemManager.Instance;
    }

    public void OnItemLoosing()
    {
        //GameObject.Find("EventManager").GetComponent<EventManager>().OnEnemyDiedEvent -= OnEnemyDied;
    }

    public void OnEnemyDied(GameObject enemy)
    {
        float randInt = Random.Range(0f, 1f);
        if (randInt <= explosionChance * MainItemManager.Instance.GetModify(MainItemManager.ModifyTypes.LuckKf))
        {
            MakeExplosion(publicExplosion, enemy.transform.position);
        }
    }
    
    void MakeExplosion(GameObject explosion, Vector2 position) {
        Quaternion quaternion = new Quaternion(0, 0, 0, 0);
        Instantiate(explosion, position, quaternion).GetComponentInChildren<ExplosionLogic>().damage = startExplosionDamage * MainItemManager.Instance.GetModify(ItemManager.ModifyTypes.DamageKf);
    }
}
