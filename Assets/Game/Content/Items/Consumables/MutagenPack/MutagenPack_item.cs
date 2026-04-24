using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutagenItem_item : ItemLogic
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnItemUse(SCellData cellData)
    {
        gameObject.GetComponent<MutagenPackLogic>().LootDrop();
        InventoryUtils.AfterUsingConsumables(cellData);
        // предметов становится на 1 меньше
        // проигрывается звук яблока
        //PlayerUtils.HealPlayer(hpRegen);
    }
}
