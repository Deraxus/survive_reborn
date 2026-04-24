using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gift_item : ItemLogic
{

    public override void OnItemUse(SCellData cellData)
    {
        gameObject.GetComponent<GiftLogic>().LootDrop();
        InventoryUtils.AfterUsingConsumables(cellData);
    }
}
