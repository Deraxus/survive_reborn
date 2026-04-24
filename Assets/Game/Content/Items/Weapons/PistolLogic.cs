using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolLogic : Weapon
{
    public override void loadData()
    {
        foreach (GameObject obj in GameObject.Find("MainManager").GetComponent<MainManager>().mainPlayer.GetComponent<MainInventory>().allHands)
        {
            if (obj.name == "hand1_pistol")
            {
                hand1 = obj;
            }
        }
    }
}
