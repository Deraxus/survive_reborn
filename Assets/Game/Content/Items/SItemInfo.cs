using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/ItemInfo")]
public class SItemInfo : ScriptableObject
{
    public int itemID = 0;
    public string itemName = "Неизвестный предмет";
    public string pickUpMessageText = "Вы подобрали какой то предмет!";
    public string pickUpMessageDescription = "Он достаточно сильный";

    public Sprite inventorySprite;
    public string inventoryDescription = "Тут что-то есть";

    public Utils.RareTypes rareType = Utils.RareTypes.common;
    public Utils.ItemTypes itemType = Utils.ItemTypes.weapon;

    public int shopPrice = 0;

    [Tooltip("Если включено - предмет может выпадть в дубликатах. Подходит для увеличителей статов")]
    public bool canBeRepeatAnyway = false;

    public int dropCountMin = 1;
    public int dropCountMax = 1;
    public List<AudioClip> takenSounds;
}
