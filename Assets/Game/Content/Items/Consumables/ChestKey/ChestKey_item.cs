using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestKey_item : ItemLogic
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnItemUse(SCellData cellData)
    {
        InventoryUtils.AfterUsingConsumables(cellData);
        // предметов становится на 1 меньше
        // проигрывается звук яблока
    }
}
