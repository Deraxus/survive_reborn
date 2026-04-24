using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple_item : ItemLogic
{
    public int hpRegen = 5;
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
        InventoryUtils.AfterUsingConsumables(cellData);
        // предметов становится на 1 меньше
        // проигрывается звук яблока
        PlayerUtils.HealPlayer(hpRegen);
    }
}
