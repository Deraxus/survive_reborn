using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutagenPackLogic : Loot
{
    override public void LootDrop()
    {
        Awake();
        Start();
        int randCoinNumber = Random.Range(minCoinCount, maxCoinCount);
        Utils.StartGlobalCoroutine(LootDropCor(whereSpawn : GameObject.Find("MainManager").GetComponent<MainManager>().mainPlayer.transform.position, mode : 1, dropCount : randCoinNumber));
    }

    
}
