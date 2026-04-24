using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public static class ItemTakingUtils
{
    // Update is called once per frame


    public static string OnPassiveItemTaking(GameObject item)
    {
        switch (item.GetComponent<ItemInfo>().itemType) {
            case Utils.ItemTypes.passive:
                GameObject ItemManager = MainItemManager.Instance.gameObject;
                GameObject ItemManagerSleeping = SleepingItemManager.Instance.gameObject;
                string scriptName = item.name.Replace("_sprite", "") + "Item";
                Debug.Log("Буду искать скрипт: " + scriptName);

                foreach (MonoBehaviour component in ItemManagerSleeping.GetComponents<MonoBehaviour>())
                {
                    if (component.GetType().Name == scriptName)
                    {
                        Component newComponent = ItemManager.AddComponent(component.GetType());

                        FieldInfo[] fieldNew = component.GetType().GetFields();
                        FieldInfo[] fieldOld = newComponent.GetType().GetFields();

                        PropertyInfo[] propertyNew = component.GetType().GetProperties();
                        PropertyInfo[] propertyOld = newComponent.GetType().GetProperties();
                        for (int i = 0; i < newComponent.GetType().GetFields().Length; i++)
                        {
                            fieldNew[i].SetValue(newComponent, fieldOld[i].GetValue(component));
                        }

                        /*for (int i = 0; i < newComponent.GetType().GetProperties().Length; i++)
                        {
                            if (propertyNew[i].CanWrite)
                            {
                                propertyNew[i].SetValue(newComponent, propertyOld[i].GetValue(component));
                            }
                        }*/
                        // СУДЯ ПО ВСЕМУ НЕ НУЖНО, МОЖНО УБРАТЬ

                        Debug.Log($"Добавил {component.GetType()}");

                        DestroyGameObject(item);
                        InventoryUtils.GiveItem(item, InventorySystem.Instance.allInventoryCells);
                        return component.GetType().Name;
                    }
                }
                break;
            case Utils.ItemTypes.consumables:
                DestroyGameObject(item);
                InventoryUtils.GiveItem(item, InventorySystem.Instance.allInventoryCells);
                break;
        }

        return null;
    }
    public static void DestroyGameObject(GameObject obj)
    {
        GameObject.Destroy(obj);
    }

    // Метод для поиска основного объекта предмета
    // К примеру, нужно найти основной объект пистолета - на вход поступает пистолет который падает в игре,
    // а метод возвращает основной объект из менеджера
    public static GameObject GetFullItem(GameObject spriteItem, bool shouldFindTrueWeapon = false)
    {
        Debug.Log("Начинаю поиск");
        Debug.Log("Взятие");
        Debug.Log(spriteItem.name);
        //gameObject.GetComponent<MainInventory>().inventory[freeCell] = item.gameObject.GetComponentInChildren<LinkLootSprites>().linkSprite.gameObject;
        MainItemManager mainItemManager = MainItemManager.Instance;
        Debug.Log($"{mainItemManager.weaponsItems} буга");
        string weaponName = spriteItem.name.Replace("_sprite", "") + "_weapon";
        string itemName = spriteItem.name;
        if (itemName.Contains("(Clone)"))
        {
            while (itemName.Contains("(Clone)"))
            {
                itemName = itemName.Replace("(Clone)", "");
            }
        }
        if (!shouldFindTrueWeapon)
        {
            foreach (GameObject obj in mainItemManager.weaponsSprites)
            {
                if (itemName.Equals(obj.name))
                {
                    return obj;
                }
            }
        }
        else
        {
            // Пытаемся найти нужное оружие из всего списка оружий
            foreach (GameObject obj in mainItemManager.weaponsItems)
            {
                if (weaponName.Equals(obj.name))
                {
                    return obj;
                }
            }
        }
        
        // Пытаемся найти копию объекта в пассивных предметах
        foreach (GameObject obj in mainItemManager.passiveItems)
        {
            if (itemName.Equals(obj.name))
            {
                return obj;
            }
        }

        // Пытаемся найти нужный расходник
        foreach (GameObject obj in mainItemManager.consumablesItems)
        {
            if (itemName.Equals(obj.name))
            {
                return obj;
            }
        }
        return null;
    }
}
