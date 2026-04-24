using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenApple_item : Apple_item
{
    public override void OnItemUse(SCellData cellData)
    {
        hpRegen = (int) MainManager.Instance.mainPlayer.GetComponent<Player>().MaxHP;
        base.OnItemUse(cellData);
    }
}
