using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOnItemBuying
{
    // Start is called before the first frame update
    void OnItemBuying(GameObject item, ItemManager itemManager);
}
