using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftLogic : Loot
{
    override public void LootDrop()
    {
        Awake();
        Start();
        Utils.StartGlobalCoroutine(LootDropCor(whereSpawn : MainManager.Instance.mainPlayer.transform.position));
    }

    
}
