using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SCellData : ScriptableObject
{
    public bool isFree = true;
    public Sprite itemSprite = null;
    public string itemName = "Empty";
    public string itemDescription = "��� ������ ����";
    public Utils.RareTypes itemRareType = Utils.RareTypes.common;
    public int itemCount = 1;
    public Utils.ItemTypes itemType;

    public GameObject mainInventoryCell = null;
    public GameObject prefabItem;
    public int cellIndex = 999;
}
