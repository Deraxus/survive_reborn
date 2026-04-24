using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainItemManager : ItemManager
{
    public bool itemsCanRepeat = true;

    public static MainItemManager Instance;
    public override void Awake()
    {
        base.Awake();
        Instance = this;
    }
}
