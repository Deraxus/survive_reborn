using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SCellCraftingData : ScriptableObject
{
    [SerializeField] public Sprite itemCraftingSprite = null;
    [SerializeField] public string itemName = "Empty";
    [SerializeField] public string itemDescription = "Тут ничего нету";
    [SerializeField] public int itemCount = 0;
    [SerializeField] public Utils.ItemTypes itemType;

    [SerializeField] public GameObject mainInventoryCell = null; // Для основной системы инвентаря: за что хвататься, если игрок выберет этот предмет
}