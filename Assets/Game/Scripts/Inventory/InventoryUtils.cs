using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public static class InventoryUtils
{
    // Методы для работы с ячейками

    // Получить свободную ячейку в инвентаре. Принимает на вход все ячейки инвентаря, а также необязательный второй параметр
    // Если reverse == true, то вернет ячейку с конца, полезно если нужно положить предмет в конец инвентаря
    // Возвращает ячейку инвентаря, которую нашел, если не удалось найти ячейку возвращает null
    public static GameObject GetFreeCell(List<GameObject> inventoryCells = null, bool reverse = false) {
        if (inventoryCells == null)
        {
            inventoryCells = GameObject.Find("UIObject").GetComponent<InventorySystem>().allInventoryCells;
        }
        if (reverse)
        {
            foreach (GameObject cell in inventoryCells.AsEnumerable().Reverse())
            {
                if (cell.GetComponent<CellData>().data.isFree)
                {
                    return cell;
                }
            }
        }
        else 
        {
            foreach (GameObject cell in inventoryCells)
            {
                CellData cellData = cell.GetComponent<CellData>();
                if (cellData != null && cellData.data != null && cell.GetComponent<CellData>().data.isFree)
                {
                    return cell;
                }
            }
        }
        return null;
    }

    // Найти ячейку в инвентаре, в которой лежит предмет с именем itemName
    // Принимает на вход имя предмета или его полноценный префаб (item_sprite)
    // Возвращает ячейку в которой лежит предмет с таким именем, если такой ячейки нету возвращает null
    public static GameObject FindItemCell(string itemName = null, GameObject prefabItem = null)
    {
        List<GameObject> callesList = new List<GameObject>();
        callesList = InventorySystem.Instance.allInventoryCells;
        foreach (GameObject targetCell in callesList)
        {
            SCellData cellData = targetCell.GetComponent<CellData>().data;
            if (itemName != null)
            {
                if (targetCell.GetComponent<CellData>().data.itemName.Equals(itemName))
                {
                    return targetCell;
                }
                if (targetCell.GetComponent<CellData>().data.prefabItem != null && targetCell.GetComponent<CellData>().data.prefabItem.name.Equals(itemName))
                {
                    return targetCell;
                }
                if (targetCell.GetComponent<CellData>().data.prefabItem != null && targetCell.GetComponent<CellData>().data.prefabItem.name == itemName)
                {
                    return targetCell;
                }
            }
            if (prefabItem != null)
            {
                if (cellData.prefabItem != null && prefabItem == cellData.prefabItem)
                {
                    return targetCell;
                }
                if (cellData.prefabItem != null && prefabItem.name == cellData.prefabItem.name)
                {
                    return targetCell;
                }

                // Пытаемся достать оружия
                if (prefabItem.GetComponentInChildren<ItemInfo>().itemType == Utils.ItemTypes.weapon)
                {
                    if (cellData != null && ItemTakingUtils.GetFullItem(prefabItem) == cellData.prefabItem)
                    {
                        return targetCell;
                    }

                    string weaponName = prefabItem.name.Replace("sprite", "weapon");
                    if (cellData.prefabItem != null && weaponName == cellData.prefabItem.name)
                    {
                        return targetCell;
                    }
                }

            }
        }
        return null;
    }

    // Перерисовать ячейку в инвентаре: заполнить ее нужной картинкой и нужным предметом
    // Получает на вход item - предмет, который будет в ячейке. Также принимает на вход список всех ячеек и по желанию одну единственную ячейку
    // Если указана конкретная ячейка - перерисует ее, если же не указана - найдет нужную ячейку и перерисует ее
    public static void RedrawItemCell(GameObject item, List<GameObject> cellList = null, GameObject cell = null)
    {
        if (cell == null)
        {
            cell = InventoryUtils.GetFreeCell(cellList);
        }
        cell.GetComponent<CellData>().data.itemName = item.GetComponent<ItemInfo>().itemName;
        cell.GetComponent<CellData>().data.itemDescription = item.GetComponent<ItemInfo>().inventoryDescription;
        cell.GetComponent<CellData>().data.itemSprite = item.GetComponent<ItemInfo>().inventorySprite;
        cell.GetComponent<CellData>().data.itemType = item.GetComponent<ItemInfo>().itemType;
        cell.GetComponent<CellData>().data.itemRareType = item.GetComponent<ItemInfo>().rareType;
        cell.GetComponent<CellData>().spriteObject.GetComponent<UnityEngine.UI.Image>().sprite = item.GetComponent<ItemInfo>().inventorySprite;
        cell.GetComponent<CellData>().data.isFree = false;
        if (cell.GetComponent<CellData>().data.itemType != Utils.ItemTypes.consumables)
        {
            cell.GetComponent<CellData>().countText.text = "";
        }
        else
        {
            cell.GetComponent<CellData>().countText.text = cell.GetComponent<CellData>().data.itemCount.ToString();
        }

        /*switch (item.GetComponent<ItemInfo>().itemType) {
            case Utils.ItemTypes.passive:
                ItemTakingUtils.OnPassiveItemTaking(item, ItemManagerTrue, ItemManagerSleep);
                break;
        }*/
    }

    

    // Перерисовать ячейку описания предметов в инвентаре
    // На вход принимает дату ячейки инвентаря, в которой хранится вся информация чем именно заполнить описание предмета.
    // namePanel - панель с именем предмета
    // typePanel - панель с типом предмета
    // descriptionPanel - панель с описанием предмета
    public static bool RedrawInfoPanels(SCellData cellInfo, UnityEngine.UI.Image namePanel, UnityEngine.UI.Image typePanel, UnityEngine.UI.Image descriptionPanel, SUIData uidata = null)
    {
        if (uidata == null)
        {
            uidata = SUIData.CreateInstance<SUIData>();
        }
        switch(cellInfo.itemRareType)
        {
            case Utils.RareTypes.common:
                namePanel.GetComponentInChildren<TextMeshProUGUI>().text = cellInfo.itemName;
                break;
            case Utils.RareTypes.rare:
                namePanel.GetComponentInChildren<TextMeshProUGUI>().text = $"{uidata.rareColor}{cellInfo.itemName}";
                break;
            case Utils.RareTypes.mythic:
                namePanel.GetComponentInChildren<TextMeshProUGUI>().text = $"{uidata.mythicColor}{cellInfo.itemName}";
                break;
            case Utils.RareTypes.legendary:
                namePanel.GetComponentInChildren<TextMeshProUGUI>().text = $"{uidata.legendaryColor}{cellInfo.itemName}";
                break;
        }
        descriptionPanel.GetComponentInChildren<TextMeshProUGUI>().text = cellInfo.itemDescription;
        switch (cellInfo.itemType)
        {
            case Utils.ItemTypes.passive:
                typePanel.GetComponentInChildren<TextMeshProUGUI>().text = $"Тип: пассивный";
                break;
            case Utils.ItemTypes.weapon:
                typePanel.GetComponentInChildren<TextMeshProUGUI>().text = $"Тип: оружие";
                break;
            case Utils.ItemTypes.consumables:
                typePanel.GetComponentInChildren<TextMeshProUGUI>().text = $"Тип: расходники";
                break;
        }
        return true;
    }

    // Перерисовать ячейку описания предметов в магазине
    // На вход принимает дату ячейки магазина, в которой хранится вся информация чем именно заполнить описание предмета
    // namePanel - панель с именем предмета
    // typePanel - панель с типом предмета
    // descriptionPanel - панель с описанием предмета
    public static bool RedrawShopInfoPanels(SCellShopData shopCellInfo, UnityEngine.UI.Image namePanel, UnityEngine.UI.Image typePanel, UnityEngine.UI.Image descriptionPanel)
    {
        namePanel.GetComponentInChildren<TextMeshProUGUI>().text = shopCellInfo.itemName;
        descriptionPanel.GetComponentInChildren<TextMeshProUGUI>().text = shopCellInfo.itemDescription;
        switch (shopCellInfo.itemType)
        {
            case Utils.ItemTypes.passive:
                typePanel.GetComponentInChildren<TextMeshProUGUI>().text = $"Тип: пассивный";
                break;
            case Utils.ItemTypes.weapon:
                typePanel.GetComponentInChildren<TextMeshProUGUI>().text = $"Тип: оружие";
                break;
            case Utils.ItemTypes.consumables:
                typePanel.GetComponentInChildren<TextMeshProUGUI>().text = $"Тип: расходники";
                break;
        }
        return true;
    }

    // Выдать предмет - комплексный вариант
    // Принимает на вход предмет, который хотим выдать; все ячейки инвентаря ; ключевой UI объект (если не указать - метод попытается его найти)
    // Метод универсальный, может выдавать на данный момент: пассивные предметы, оружия, расходники
    // У пассивного предмета должен быть скрипт, который отвечает за логику этого предмета - он должен находится в sleepingItemManager, метод перетянет его в основной менеджер предметов
    // 
    public static bool GiveItem(GameObject item,List<GameObject> allInventoryCells) {
        GameObject targetCell = null;
        GameObject localItem = null;
        Debug.Log($"{item.name} лог");
        if (item.GetComponent<ItemInfo>() != null) {// Пытаемся понять, перед нами префаб или нет
            localItem = item;
            Debug.Log("оставил");
        }
        else { // Если не смогли найти нужный компонент, то это префаб: пытаемся вытащить нужный нам объект с данными
            localItem = item.transform.GetChild(0).gameObject;
            Debug.Log("Заменааа");
        }
        //localUIObject.GetComponent<InventorySystem>().OnItemTaking(localItem);
        Debug.Log("Начинаю работу с инвентарем");
        switch (localItem.GetComponent<ItemInfo>().itemType)
        {
            case Utils.ItemTypes.passive:
                MainItemManager mainItemManager = MainItemManager.Instance;
                SleepingItemManager sleepingItemManager = SleepingItemManager.Instance;
                /*string scriptName = localItem.name.Replace("_sprite", "") + "_item";
                Debug.Log("Буду искать скрипт: " + scriptName);

                foreach (MonoBehaviour component in localItem.GetComponents<MonoBehaviour>())
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
                        Debug.Log($"Добавил {component.GetType()}");
                    }
                } СТАРАЯ ДОБРАЯ ВЕРСИЯ */
                MainItemManager.Instance.RegisterPassiveItem(localItem);
                break;
            case Utils.ItemTypes.consumables:
                break;
        }

        GameObject publicTargetCell = null;
        if (publicTargetCell != null)
        {
            targetCell = publicTargetCell;
        }
        else if ((InventoryUtils.FindItemCell(localItem.GetComponent<ItemInfo>().itemName) == null) || (localItem.GetComponent<ItemInfo>().itemType != Utils.ItemTypes.consumables))
        {
            if (localItem.GetComponent<ItemInfo>().itemType == Utils.ItemTypes.passive || localItem.GetComponent<ItemInfo>().itemType == Utils.ItemTypes.consumables)
            {
                targetCell = InventoryUtils.GetFreeCell(allInventoryCells, true);
            }
            else
            {
                targetCell = InventoryUtils.GetFreeCell(allInventoryCells);
            }
            if (targetCell == null)
            {
                return false;
            }
            targetCell.GetComponent<CellData>().data.itemCount = 1;
        }
        else // для расходников
        {
            targetCell = InventoryUtils.FindItemCell(localItem.GetComponent<ItemInfo>().itemName);
            int targetCellIndex = InventorySystem.Instance.allInventoryCells.IndexOf(targetCell);
            GameObject.Find("Player").GetComponent<MainInventory>().mainInventory[targetCellIndex].itemCount += 1;
            RedrawMainInventoryCell(targetCellIndex, null, GameObject.Find("Player").GetComponent<MainInventory>().mainInventory[targetCellIndex]);
        }

        // Отрисовка предмета в UI инвентаре
        //InventoryUtils.RedrawItemCell(localItem, allInventoryCells, targetCell);
        if ((targetCell.GetComponent<CellData>().data.cellIndex != -1) && (localItem.GetComponent<ItemInfo>().itemType == Utils.ItemTypes.weapon))
        {
            RedrawMainInventoryCell(targetCell.GetComponent<CellData>().data.cellIndex, localItem);
        }

        // Выдаем предмет в основной инвентарь игрока
        int a = InventorySystem.Instance.allInventoryCells.IndexOf(targetCell);
        MainManager.Instance.mainPlayer.GetComponent<MainInventory>().mainInventory[a].prefabItem = ItemTakingUtils.GetFullItem(localItem);
        RedrawMainInventoryCell(a, ItemTakingUtils.GetFullItem(localItem));
        Debug.Log($"Перерисовал ячейку {targetCell.name}");
        if (localItem.GetComponent<ItemInfo>().takenSounds.Count > 0)
        {
            int randSoundIndex = UnityEngine.Random.Range(0, localItem.GetComponent<ItemInfo>().takenSounds.Count);
            AudioSource.PlayClipAtPoint(localItem.GetComponent<ItemInfo>().takenSounds[randSoundIndex], MainManager.Instance.mainPlayer.transform.position);
        }
        return true;
    }
    public static void RedrawMainInventoryCell(int mainInventoryCellIndex, GameObject item = null, SCellData cellData = null)
    {
        //GameObject localItem = null;
        //localItem = ItemTakingUtils.GetFullItem(item);
        //GameObject.Find("Player").GetComponent<MainInventory>().inventory[mainInventoryCellIndex] = localItem;
        GameObject player = GameObject.Find("Player");
        GameObject cell = GameObject.Find("UIObject").GetComponent<InventorySystem>().allInventoryCells[mainInventoryCellIndex];
        if (cellData == null)
        {

        }
        if (item != null)
        {
            cell.GetComponent<CellData>().spriteObject.SetActive(true);
        }
        if (item != null)
        {
            if (item.GetComponent<ItemInfo>() != null)
            {// Пытаемся понять, перед нами префаб или нет
                //
            }
            else { // Если не смогли найти нужный компонент, то это префаб: пытаемся вытащить нужный нам объект с данными
                item = item.transform.GetChild(0).gameObject;
            }
        }

        if (item != null)
        {
            player.GetComponent<MainInventory>().mainInventory[mainInventoryCellIndex].itemName = item.GetComponent<ItemInfo>().itemName;
            player.GetComponent<MainInventory>().mainInventory[mainInventoryCellIndex].itemDescription = item.GetComponent<ItemInfo>().inventoryDescription;
            player.GetComponent<MainInventory>().mainInventory[mainInventoryCellIndex].itemSprite = item.GetComponent<ItemInfo>().inventorySprite;
            player.GetComponent<MainInventory>().mainInventory[mainInventoryCellIndex].itemType = item.GetComponent<ItemInfo>().itemType;
            player.GetComponent<MainInventory>().mainInventory[mainInventoryCellIndex].itemSprite = item.GetComponent<ItemInfo>().inventorySprite;
            player.GetComponent<MainInventory>().mainInventory[mainInventoryCellIndex].itemRareType = item.GetComponent<ItemInfo>().rareType;
            player.GetComponent<MainInventory>().mainInventory[mainInventoryCellIndex].isFree = false;

            if (item.GetComponent<ItemInfo>().itemType == Utils.ItemTypes.weapon)
            {
                player.GetComponent<MainInventory>().mainInventory[mainInventoryCellIndex].prefabItem = ItemTakingUtils.GetFullItem(item, true);
            }
            else
            {
                player.GetComponent<MainInventory>().mainInventory[mainInventoryCellIndex].prefabItem = item;
            }

            cell.GetComponent<CellData>().spriteObject.GetComponent<UnityEngine.UI.Image>().sprite = item.GetComponent<ItemInfo>().inventorySprite;

            switch (item.GetComponent<ItemInfo>().rareType)
            {
                case Utils.RareTypes.common:
                    cell.GetComponent<CellData>().GetComponent<UnityEngine.UI.Image>().sprite = GameObject.Find("MainManager").GetComponent<MainManager>().mainUiData.commonCell;
                    break;
                case Utils.RareTypes.rare:
                    cell.GetComponent<CellData>().GetComponent<UnityEngine.UI.Image>().sprite = GameObject.Find("MainManager").GetComponent<MainManager>().mainUiData.rareCell;
                    break;
                case Utils.RareTypes.mythic:
                    cell.GetComponent<CellData>().GetComponent<UnityEngine.UI.Image>().sprite = GameObject.Find("MainManager").GetComponent<MainManager>().mainUiData.mythicCell;
                    break;
                case Utils.RareTypes.legendary:
                    cell.GetComponent<CellData>().GetComponent<UnityEngine.UI.Image>().sprite = GameObject.Find("MainManager").GetComponent<MainManager>().mainUiData.legendaryCell;
                    break;
            }

        }
        else if (cellData != null && cellData.itemName == "Empty" && item == null) // Детект полной очистки ячейки
        {
            Debug.Log("фул очистка");
            //player.GetComponent<MainInventory>().mainInventory[mainInventoryCellIndex] = cellData;
            cell.GetComponent<CellData>().GetComponent<UnityEngine.UI.Image>().sprite = GameObject.Find("MainManager").GetComponent<MainManager>().mainUiData.commonCell;
            cell.GetComponent<CellData>().spriteObject.GetComponent<UnityEngine.UI.Image>().sprite = cellData.itemSprite;

            player.GetComponent<MainInventory>().mainInventory[mainInventoryCellIndex].itemName = cellData.itemName;
            player.GetComponent<MainInventory>().mainInventory[mainInventoryCellIndex].itemDescription = cellData.itemDescription;
            //player.GetComponent<MainInventory>().mainInventory[mainInventoryCellIndex].itemType = Utils.ItemTypes.passive; // Сомнительно, лучше сделать что-то с count чтобы не было нуля
            player.GetComponent<MainInventory>().mainInventory[mainInventoryCellIndex].itemRareType = cellData.itemRareType;
            player.GetComponent<MainInventory>().mainInventory[mainInventoryCellIndex].isFree = true;
            player.GetComponent<MainInventory>().mainInventory[mainInventoryCellIndex].prefabItem = null;
            player.GetComponent<MainInventory>().mainInventory[mainInventoryCellIndex].mainInventoryCell.GetComponent<CellData>().countText.text = "";
            return;
        }
        else if (cellData != null)
        {
            Debug.Log("999");
            switch (cellData.prefabItem.GetComponentInChildren<ItemInfo>().rareType)
            {
                case Utils.RareTypes.common:
                    cell.GetComponent<CellData>().GetComponent<UnityEngine.UI.Image>().sprite = GameObject.Find("MainManager").GetComponent<MainManager>().mainUiData.commonCell;
                    break;
                case Utils.RareTypes.rare:
                    cell.GetComponent<CellData>().GetComponent<UnityEngine.UI.Image>().sprite = GameObject.Find("MainManager").GetComponent<MainManager>().mainUiData.rareCell;
                    break;
                case Utils.RareTypes.mythic:
                    cell.GetComponent<CellData>().GetComponent<UnityEngine.UI.Image>().sprite = GameObject.Find("MainManager").GetComponent<MainManager>().mainUiData.mythicCell;
                    break;
                case Utils.RareTypes.legendary:
                    cell.GetComponent<CellData>().GetComponent<UnityEngine.UI.Image>().sprite = GameObject.Find("MainManager").GetComponent<MainManager>().mainUiData.legendaryCell;
                    break;
                default:
                    cell.GetComponent<CellData>().GetComponent<UnityEngine.UI.Image>().sprite = GameObject.Find("MainManager").GetComponent<MainManager>().mainUiData.commonCell;
                    break;
            }
            
            //cell.GetComponent<CellData>().GetComponent<UnityEngine.UI.Image>().sprite = GameObject.Find("MainManager").GetComponent<MainManager>().mainUiData.commonCell;
            cell.GetComponent<CellData>().spriteObject.GetComponent<UnityEngine.UI.Image>().sprite = cellData.itemSprite;
        }
        cell.GetComponent<CellData>().data = player.GetComponent<MainInventory>().mainInventory[mainInventoryCellIndex];

        if (cell.GetComponent<CellData>().data.itemType != Utils.ItemTypes.consumables)
        {
            cell.GetComponent<CellData>().countText.text = "";
        }
        else
        {
            if (cellData != null && cellData.itemCount == 1)
            {
                cell.GetComponent<CellData>().countText.text = "";
            }
            else if (cellData != null && cellData.itemCount > 1)
            {
                cell.GetComponent<CellData>().countText.text = cell.GetComponent<CellData>().data.itemCount.ToString();
            }
        }
    }

    public static bool OnItemTaking2(GameObject item, List<GameObject> allInventoryCells, GameObject publicTargetCell = null)
    {
        Debug.Log("Начинаю работу с инвентарем");
        GameObject targetCell;
        if (publicTargetCell != null)
        {
            targetCell = publicTargetCell;
        }
        else if ((InventoryUtils.FindItemCell(item.GetComponent<ItemInfo>().itemName) == null) || (item.GetComponent<ItemInfo>().itemType != Utils.ItemTypes.consumables))
        {
            if (item.GetComponent<ItemInfo>().itemType == Utils.ItemTypes.passive)
            {
                targetCell = InventoryUtils.GetFreeCell(allInventoryCells, true);
            }
            else
            {
                targetCell = InventoryUtils.GetFreeCell(allInventoryCells);
            }
            if (targetCell == null)
            {
                return false;
            }
            targetCell.GetComponent<CellData>().data.itemCount = 1;
        }
        else
        {
            targetCell = InventoryUtils.FindItemCell(item.GetComponent<ItemInfo>().itemName);
            targetCell.GetComponent<CellData>().data.itemCount += 1;
        }
        InventoryUtils.RedrawItemCell(item, allInventoryCells, targetCell);
        if ((targetCell.GetComponent<CellData>().data.cellIndex != -1) && (item.GetComponent<ItemInfo>().itemType == Utils.ItemTypes.weapon))
        {
            RedrawMainInventoryCell(targetCell.GetComponent<CellData>().data.cellIndex, item);
        }
        Debug.Log($"Перерисовал ячейку {targetCell.name}");
        return true;
    }


    public static bool RedrawShopCell(SItemShopInfo itemShopData, GameObject cell = null, GameObject shopCenter = null) {
        if ((itemShopData.itemPrefab == null) || (itemShopData.itemPrefab.name == "EmptyPrefab")) {
            cell.GetComponent<CellShopData>().spriteObject.GetComponent<UnityEngine.UI.Image>().sprite = null;
            cell.GetComponent<CellShopData>().priceText.text = string.Empty;
            cell.GetComponent<CellShopData>().shopItem = null;
            cell.gameObject.SetActive(false);
            return true;
        }
        cell.GetComponent<CellShopData>().spriteObject.GetComponent<UnityEngine.UI.Image>().sprite = itemShopData.itemPrefab.GetComponentInChildren<ItemInfo>().inventorySprite;
        cell.GetComponent<CellShopData>().priceText.text = itemShopData.itemPrice.ToString();
        cell.GetComponent<CellShopData>().shopItem = itemShopData.itemPrefab;
        cell.GetComponent<CellShopData>().data.linkedShopCenter = shopCenter;
        cell.GetComponent<CellShopData>().data.itemName = itemShopData.itemPrefab.GetComponentInChildren<ItemInfo>().itemName;
        cell.GetComponent<CellShopData>().data.itemType = itemShopData.itemPrefab.GetComponentInChildren<ItemInfo>().itemType;
        cell.GetComponent<CellShopData>().data.itemDescription = itemShopData.itemPrefab.GetComponentInChildren<ItemInfo>().inventoryDescription;
        cell.GetComponent<CellShopData>().data.mainItemPrefab = itemShopData.itemPrefab;
        cell.GetComponent<CellShopData>().data.itemPrice = itemShopData.itemPrice;
        cell.GetComponent<CellShopData>().data.itemCount = itemShopData.itemCount;

        if (itemShopData.itemPrefab != null)
        {
            switch (itemShopData.itemPrefab.GetComponentInChildren<ItemInfo>().rareType)
            {
                case Utils.RareTypes.rare:
                    cell.GetComponent<Image>().sprite = MainManager.Instance.mainUiData.rareShopCell;
                    break;
                case Utils.RareTypes.mythic:
                    cell.GetComponent<Image>().sprite = MainManager.Instance.mainUiData.mythicShopCell;
                    break;
                case Utils.RareTypes.legendary:
                    cell.GetComponent<Image>().sprite = MainManager.Instance.mainUiData.legendaryShopCell;
                    break;
                default:
                    cell.GetComponent<Image>().sprite = MainManager.Instance.mainUiData.commonShopCell;
                    break;
            }
        }
        cell.gameObject.SetActive(true);
        return true;
    }

    public static bool RemoveItemFromShopCenter(GameObject item, GameObject shopCenter) {
        foreach (SItemShopInfo localData in shopCenter.GetComponent<ShopCenterLogic>().sellItemList)
        {
            if (localData.itemPrefab != null && localData.itemPrefab.name == item.name)
            {
                localData.itemPrefab = null;
                break;
            }
        }
        // shopCenter.GetComponent<ShopCenterLogic>().sellItemList[shopCenter.GetComponent<ShopCenterLogic>().sellItemList.IndexOf(item)] = null;
        return true;
    }

    public static bool HideAllPanelsExcept(GameObject panelNotHiding, GameObject UIObject = null) {
        GameObject mainUIObject = UIObject;
        if (UIObject == null) {
            mainUIObject = GameObject.Find("UIObject");
        }
        // Панель инвентаря
        if (panelNotHiding != mainUIObject.GetComponent<InventorySystem>().inventoryPanel) {
            mainUIObject.GetComponent<InventorySystem>().inventoryPanel.gameObject.SetActive(false);
        }
        // Панель магазина
        if (panelNotHiding != mainUIObject.GetComponent<InventorySystem>().shopPanel)
        {
            mainUIObject.GetComponent<InventorySystem>().shopPanel.gameObject.SetActive(false);
        }
        // Панель крафта
        if (panelNotHiding != mainUIObject.GetComponent<InventorySystem>().craftingPanel)
        {
            mainUIObject.GetComponent<InventorySystem>().craftingPanel.gameObject.SetActive(false);
        }
        // Панель консоли
        if (panelNotHiding != mainUIObject.GetComponent<InventorySystem>().consolePanel)
        {
            mainUIObject.GetComponent<InventorySystem>().consolePanel.gameObject.SetActive(false);
        }

        return true;
    }

    // Метод, который нужно вызывать после каждого применения расходника
    // Он берет на себя все: уменьшает количество предметов, отрисовывает ячейку и изменяет ее
    // Если количество после использования стало равно нулю, метод очистит ячейку в инвентаре
    public static void AfterUsingConsumables(SCellData cellData)
    {
        cellData.itemCount -= 1;
        if (cellData.itemCount <= 0)    
        {
            //SCellData localCellData = SCellData.CreateInstance<SCellData>();
            //localCellData.cellIndex = cellData.cellIndex;
            //RedrawMainInventoryCell(cellData.cellIndex, null, localCellData); ВЕРНУТЬ ПОТОМ!!! 16.04.2025
            ClearInventoryCell(cellData: cellData);
            return;
        }
        InventoryUtils.RedrawMainInventoryCell(cellData.cellIndex, cellData : cellData);
    }

    // Метод, который очищает конкретную ячейку в инвентаре (сбрасывает ее до изначального состояния)
    // 
    // 
    public static void ClearInventoryCell(GameObject cell = null, SCellData cellData = null)
    {
        if (cell == null)
        {
            cell = cellData.mainInventoryCell;
        }
        cell.GetComponent<CellData>().spriteObject.SetActive(false);
        if (cellData == null)
        {
            cellData = cell.GetComponent<CellData>().data;
        }

        MainItemManager.Instance.RemovePassiveItem(cellData.prefabItem);
        SCellData localCellData = SCellData.CreateInstance<SCellData>();
        localCellData.cellIndex = cellData.cellIndex;
        RedrawMainInventoryCell(cellData.cellIndex, null, localCellData);
        Debug.Log("очистка инвентаря");
    }

    // Метод принимает на вход предмет, возвращает true если он может быть дропнут в игре, и false - если нет
    public static bool ItemCanBeDropped(GameObject itemPrefab)
    {
        if (MainItemManager.Instance.itemsCanRepeat ||
           (itemPrefab.GetComponentInChildren<ItemInfo>().itemType == Utils.ItemTypes.consumables) ||
           (itemPrefab.GetComponentInChildren<ItemInfo>().canBeRepeatAnyway) ||
           (InventoryUtils.FindItemCell(prefabItem : itemPrefab) == null) ||
           (itemPrefab.GetComponentInChildren<ItemInfo>().itemType != Utils.ItemTypes.weapon && itemPrefab.GetComponentInChildren<ItemInfo>().rareType == Utils.RareTypes.common))
        {
            Debug.Log($"230{itemPrefab.name.Replace("_sprite", "_weapon")}");
            Debug.Log($"{itemPrefab} может быть дропнут");
            return true;
        }
        else
        {
            Debug.Log($"{itemPrefab} НЕ может быть дропнут");
            return false;
        }
    }

}
