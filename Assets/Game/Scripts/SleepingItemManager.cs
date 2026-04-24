using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepingItemManager : ItemManager
{
    public static SleepingItemManager Instance;
    public override void Awake()
    {
        base.Awake();
        Instance = this;
    }
}
