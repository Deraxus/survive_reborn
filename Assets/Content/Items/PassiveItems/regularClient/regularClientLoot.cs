using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class regularClientLoot : Loot
{
    override public void LootDrop()
    {
        Awake();
        Start();
        Utils.StartGlobalCoroutine(LootDropCor(whereSpawn: GameObject.Find("MainManager").GetComponent<MainManager>().mainPlayer.transform.position, mode: 1, dropCount: minCoinCount));
    }
}
