using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TakeItem : MonoBehaviour
{
    public SUIData uidata;
    int freeCell;

    private GameObject mainUIObject;
    public GameObject ItemManagerTrue;
    public GameObject ItemManagerSleep;
    void Start()
    {
        mainUIObject = UIManager.Instance.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "coin")
        {
            TakeCoin(collision.gameObject);
        }
        else if (collision.gameObject.tag == "mutagen")
        {
            TakeMutagen(collision.gameObject);
        }
        else if (collision.gameObject.tag == "weapon") {
            if (InventoryUtils.GetFreeCell() == null)
            {
                Utils.StartNewMessage("<color=white>В инвентаре недостаточно места!");
                return;
            }
            InventoryUtils.GiveItem(ItemTakingUtils.GetFullItem(collision.gameObject), mainUIObject.GetComponent<InventorySystem>().allInventoryCells);
            switch (collision.gameObject.GetComponent<ItemInfo>().rareType)
                {
                    case Utils.RareTypes.common:
                        mainUIObject.GetComponent<UIManager>().StartNewMessage($"{uidata.commonColor}Вы подобрали \"{collision.gameObject.GetComponent<ItemInfo>().itemName}\"", $"{uidata.commonColor}{collision.gameObject.GetComponent<ItemInfo>().pickUpMessageDescription}");
                        break;
                    case Utils.RareTypes.rare:
                        mainUIObject.GetComponent<UIManager>().StartNewMessage($"{uidata.commonColor}Вы подобрали \"{uidata.rareColor}{collision.gameObject.GetComponent<ItemInfo>().itemName}\"", $"{uidata.commonColor}{collision.gameObject.GetComponent<ItemInfo>().pickUpMessageDescription}");
                        break;
                    case Utils.RareTypes.mythic:
                        mainUIObject.GetComponent<UIManager>().StartNewMessage($"{uidata.commonColor}Вы подобрали \"{uidata.mythicColor}{collision.gameObject.GetComponent<ItemInfo>().itemName}", $"{uidata.commonColor}{collision.gameObject.GetComponent<ItemInfo>().pickUpMessageDescription}");
                        break;
                    case Utils.RareTypes.legendary:
                        mainUIObject.GetComponent<UIManager>().StartNewMessage($"{uidata.commonColor}Вы подобрали \"{uidata.legendaryColor}{collision.gameObject.GetComponent<ItemInfo>().itemName}", $"{uidata.commonColor}{collision.gameObject.GetComponent<ItemInfo>().pickUpMessageDescription}");
                        break;
                }
            //GameObject.Find("UIObject").GetComponent<InventorySystem>().OnItemTaking(collision.gameObject); �������
            //InventoryUtils.OnItemTaking2(collision.gameObject, GameObject.Find("UIObject").GetComponent<InventorySystem>().allInventoryCells);
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "consumables123")
        {
            if (InventoryUtils.GetFreeCell() == null)
            {
                Utils.StartNewMessage("<color=white>В инвентаре недостаточно места!");
                return;
            }
            //mainUIObject.GetComponent<UI>().StartNewMessage(collision.gameObject.GetComponent<ItemInfo>().pickUpMessageText, collision.gameObject.GetComponent<ItemInfo>().pickUpMessageDescription);
            //InventoryUtils.OnItemTaking2(collision.gameObject, GameObject.Find("UIObject").GetComponent<InventorySystem>().allInventoryCells);
            //collision.gameObject.SetActive(false);
            // доработать - 02.12
        }
        if (collision.gameObject.tag == "poison")
        {
            TakePoison(collision.gameObject);
        }
        if ((collision.gameObject.tag == "pItem") || (collision.gameObject.tag == "consumables"))
        {
            if (InventoryUtils.GetFreeCell() == null)
            {
                Utils.StartNewMessage("<color=white>В инвентаре недостаточно места!");
                return;
            }
            //ItemTakingUtils.OnPassiveItemTaking(collision.gameObject, ItemManagerTrue, ItemManagerSleep); ������ ������
            InventoryUtils.GiveItem(ItemTakingUtils.GetFullItem(collision.gameObject), mainUIObject.GetComponent<InventorySystem>().allInventoryCells);
            Destroy(collision.gameObject);
            if (collision.gameObject.tag != "consumables")
            {
                switch (collision.gameObject.GetComponent<ItemInfo>().rareType)
                {
                    case Utils.RareTypes.common:
                        mainUIObject.GetComponent<UIManager>().StartNewMessage($"{uidata.commonColor}Вы подобрали \"{collision.gameObject.GetComponent<ItemInfo>().itemName}\"", $"{uidata.commonColor}{collision.gameObject.GetComponent<ItemInfo>().pickUpMessageDescription}");
                        break;
                    case Utils.RareTypes.rare:
                        mainUIObject.GetComponent<UIManager>().StartNewMessage($"{uidata.commonColor}Вы подобрали \"{uidata.rareColor}{collision.gameObject.GetComponent<ItemInfo>().itemName}\"", $"{uidata.commonColor}{collision.gameObject.GetComponent<ItemInfo>().pickUpMessageDescription}");
                        break;
                    case Utils.RareTypes.mythic:
                        mainUIObject.GetComponent<UIManager>().StartNewMessage($"{uidata.commonColor}Вы подобрали \"{uidata.mythicColor}{collision.gameObject.GetComponent<ItemInfo>().itemName}\"", $"{uidata.commonColor}{collision.gameObject.GetComponent<ItemInfo>().pickUpMessageDescription}");
                        break;
                    case Utils.RareTypes.legendary:
                        mainUIObject.GetComponent<UIManager>().StartNewMessage($"{uidata.commonColor}Вы подобрали \"{uidata.legendaryColor}{collision.gameObject.GetComponent<ItemInfo>().itemName}\"", $"{uidata.commonColor}{collision.gameObject.GetComponent<ItemInfo>().pickUpMessageDescription}");
                        break;
                }
            }
        }
        /*if (collision.gameObject.tag == "consumables") {
            ItemTakingUtils.OnPassiveItemTaking(collision.gameObject, ItemManagerTrue, ItemManagerSleep);
        }*/
        
    }


    void TakeCoin(GameObject coin) {
        GetComponent<Player>().coins++;
        Destroy(coin);
    }

    void TakeMutagen(GameObject mutagen) {
        mutagen.GetComponent<Mutagen>().Player = gameObject;
        switch (mutagen.GetComponent<Mutagen>().mutagenType)
        {
            case 0:
                mutagen.GetComponent<Mutagen>().MutagenHP();
                break;
            case 1:
                mutagen.GetComponent<Mutagen>().MutagenDamage();
                break;
            case 2:
                mutagen.GetComponent<Mutagen>().MutagenRate();
                break;
            /*case 3:
                mutagen.GetComponent<Mutagen>().MutagenSpeed();
                break;*/
        }
        Destroy(mutagen);
    }
    void TakePoison(GameObject poison)
    {
        if (poison.name == "damagePoison_sprite") {
            FakeDestroying(poison);
            poison.GetComponent<DamagePoison>().OnTaking();
        }
    }

    void FakeDestroying(GameObject obj)
    {
        obj.GetComponent<BoxCollider2D>().enabled = false;
        obj.GetComponent<SpriteRenderer>().enabled = false;
        foreach (var component in obj.GetComponents<MonoBehaviour>())
        {
            component.enabled = false;
        }
    }

    /*void TakeItemF(GameObject item) {
        GameObject.Find("UIObject").GetComponent<InventorySystem>().OnItemTaking(item);
        Debug.Log("������� �����");
        Debug.Log("������");
        freeCell = gameObject.GetComponent<MainInventory>().GetFreeCell();
        //gameObject.GetComponent<MainInventory>().inventory[freeCell] = item.gameObject.GetComponentInChildren<LinkLootSprites>().linkSprite.gameObject;
        MainItemManager ItemManager = GameObject.Find("ItemManager").GetComponent<MainItemManager>();
        string weaponName = item.name.Replace("_sprite", "") + "_weapon";
        foreach (GameObject obj in ItemManager.weaponsItems)
        {
            if (weaponName.Equals(obj.name))
            {
                gameObject.GetComponent<MainInventory>().inventory[freeCell] = obj;
            }
        }
        GetComponent<MainInventory>().allItems.Add(item);
        mainUIObject.GetComponent<UI>().StartNewMessage(item.GetComponent<ItemInfo>().pickUpMessageText, item.gameObject.GetComponent<ItemInfo>().pickUpMessageDescription);
        item.SetActive(false);
    }*/

}
