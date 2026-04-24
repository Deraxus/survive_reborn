using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLoot : Loot
{
    [Tooltip("Шанс на то, что из моба выпадут мутагены/монеты (как решит скрипт Loot в своей логике)")]
    public float mutagenCoinChance = 0.3f;

    override public void LootDrop()
    {
        float randNumber = Random.Range(0f, 1f);
        if (randNumber <= itemChance)
        {
 
            StartCoroutine(LootDropCor(mode : 0));
        }
        else
        {
            randNumber = Random.Range(0f, 1f);
            if (randNumber <= mutagenCoinChance)
            {
                int randMutagenNumber = Random.Range(minCoinCount, maxCoinCount);
                StartCoroutine(LootDropCor(mode : 1, dropCount : randMutagenNumber));
            }
        }
    }
}
