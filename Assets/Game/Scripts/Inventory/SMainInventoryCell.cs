using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMainInventoryCell : ScriptableObject
{
    [SerializeField] public bool isFree = true;
    [SerializeField] public Sprite itemSprite = null;
    [SerializeField] public string itemName = "Empty";
    [SerializeField] public string itemDescription = "Тут ничего нету";
    [SerializeField] public int itemCount = 0;
    [SerializeField] public Utils.ItemTypes itemType;

    [SerializeField] public GameObject mainInventoryCell = null;

    [SerializeField] public GameObject prefabItem;
}
