using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SCellShopData : ScriptableObject
{
    [SerializeField] public Sprite itemSprite = null;
    [SerializeField] public string itemName = "Empty";
    [SerializeField] public string itemDescription = "Тут ничего нету";
    [SerializeField] public int itemCount = 0;
    [SerializeField] public int itemPrice = 0;
    [SerializeField] public Utils.ItemTypes itemType;

    [SerializeField] public GameObject linkedShopCenter;

    [SerializeField] public GameObject mainItemPrefab = null; // Для основной системы инвентаря: за что хвататься, если игрок выберет этот предмет
}