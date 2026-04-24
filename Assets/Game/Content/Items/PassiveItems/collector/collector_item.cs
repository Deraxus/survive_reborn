using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class collector_item : ItemLogic, IOnEnemyDamage
{
    [Tooltip("�� ������ n ����� ����� ������ 1 ������, �� ������ ����������� (� ����������)")]
    public int damagePerCoin = 0;

    [Tooltip("���� ������ n-���� ����� �� � ����� ������� 1 ������ (� ����������)")]
    public float partHealth = 0.2f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnItemTaking()
    {

    }

    public void OnItemLoosing()
    {

    }

    public void OnEnemyDamage(GameObject enemy, float damage = 0)
    {
        int coinsCount = 0;
        int ostat = 0;
        int maxCoinsFromEnemy = (int)(enemy.GetComponentInChildren<EnemyLogic>().maxHP / damagePerCoin) + 1;
        if (damage >= damagePerCoin)
        {
            coinsCount = (int)(damage / damagePerCoin);
            ostat = (int)(damage % damagePerCoin);
            if (ostat != 0)
            {
                damage = ostat;
            }
        }
        float rand = Random.Range(0f, 1f);
        if (rand < ((damage /  damagePerCoin) * MainItemManager.Instance.GetModify(ItemManager.ModifyTypes.LuckKf)))
        {
            coinsCount++;
        }
        coinsCount = Mathf.Min(coinsCount, maxCoinsFromEnemy);
        if (coinsCount > 0)
        {
            Utils.StartGlobalCoroutine(GetComponent<Loot>().LootDropCor(mode: 1, dropCount: coinsCount, whereSpawn: enemy.transform.GetChild(0).position));
        }
        Debug.Log("260");
    }
}
