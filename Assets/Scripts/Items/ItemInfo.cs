using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    public SItemInfo data;
    public bool drivenByData = false;

    public int itemID = 0;
    public string itemName = "Неизвестный предмет";
    public string pickUpMessageText = "Вы подобрали какой то предмет!";
    public string pickUpMessageDescription = "Он достаточно сильный";

    public Sprite inventorySprite;
    public string inventoryDescription = "Тут что-то есть";

    public Utils.RareTypes rareType = Utils.RareTypes.common;
    public Utils.ItemTypes itemType = Utils.ItemTypes.weapon;
    public ItemLogic mainItemScript;

    public int shopPrice = 0;

    [Tooltip("Если включено - предмет может выпадть в дубликатах. Подходит для увеличителей статов")]
    public bool canBeRepeatAnyway = false;

    public int dropCountMin = 1;
    public int dropCountMax = 1;
    public List<AudioClip> takenSounds;


    private void Awake()
    {
        if (drivenByData && data != null)
        {
            itemID = data.itemID;
            itemName = data.itemName;
            pickUpMessageText = data.pickUpMessageText;
            pickUpMessageDescription = data.pickUpMessageDescription;

            inventorySprite = data.inventorySprite;
            inventoryDescription = data.inventoryDescription;

            rareType = data.rareType;
            itemType = data.itemType;
            shopPrice = data.shopPrice;

            canBeRepeatAnyway = data.canBeRepeatAnyway;

            dropCountMin = data.dropCountMin;
            dropCountMax = data.dropCountMax;
        }

        if (mainItemScript == null)
        {
            mainItemScript = GetComponent<ItemLogic>();
        }
    }
    void Start()
    {
        if (inventorySprite == null) { 
            inventorySprite = GetComponent<SpriteRenderer>().sprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
