using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class ShopCenterLogic : MonoBehaviour
{
    public bool includePassiveItems = true;
    public bool includeWeapons = true;
    public bool includeConsumables = false;

    public List<GameObject> exceptItems = new List<GameObject>();

    public float rareItemChance = 0.2f;
    public float mythicItemChance = 0.05f;
    public float legendrayItemChance = 0.01f;

    public List<SItemShopInfo> sellItemList = new List<SItemShopInfo>();
    public GameObject visionObject;

    public GameObject mainItemManager;
    public GameObject mainUI;

    public bool firstContactLoaded = false;
    void Start()
    {

        //LoadItemList(3, MainItemManager.Instance.passiveItems, MainItemManager.Instance.weaponsSprites, MainItemManager.Instance.consumablesItems);
        //Debug.Log($"{mainItemManager.GetComponent<MainItemManager>().passiveItems.Count} booba");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadItemList(int itemCount = 3, List<GameObject> passiveItemList = null, List<GameObject> weaponItemList = null, List<GameObject> consumablesItemList = null)
    {
        sellItemList.Clear();
        
        for (int i = 0; i < itemCount; i++)
        {
            SItemShopInfo itemInfo = ScriptableObject.CreateInstance<SItemShopInfo>();
            sellItemList.Add(itemInfo);
        }
        List<GameObject> localItemList = new List<GameObject>();
        if ((passiveItemList != null) && (includePassiveItems))
        {
            localItemList.AddRange(passiveItemList);
        }
        if ((weaponItemList != null) && (includeWeapons))
        {
            localItemList.AddRange(weaponItemList);
        }
        if ((consumablesItemList != null) && (includeConsumables))
        {
            localItemList.AddRange(consumablesItemList);   
        }

        List<GameObject> localCommonItemList = new List<GameObject>();
        List<GameObject> localRareItemList = new List<GameObject>();
        List<GameObject> localMythicItemList = new List<GameObject>();
        List<GameObject> localLegendaryItemList = new List<GameObject>();

        // �������� ������, ����� �� ���� ������� ��������� � ������ � ������ ������
        List<GameObject> filterLocalItemList = new List<GameObject>();
        foreach (GameObject item in localItemList)
        {
            if (InventoryUtils.ItemCanBeDropped(item))
            {
                filterLocalItemList.Add(item);
            }
        }
        localItemList = filterLocalItemList;

        for (int i = localItemList.Count - 1; i > 0; i--)
        {
            if ((localItemList[i].GetComponentInChildren<ItemInfo>().shopPrice == 0) || (exceptItems.Contains(localItemList[i])))
            {
                localItemList.RemoveAt(i);
            }
        }

        foreach (GameObject localItem in localItemList)
        {
            switch (localItem.GetComponentInChildren<ItemInfo>().rareType)
            {
                case Utils.RareTypes.common:
                    localCommonItemList.Add(localItem);
                    break;
                case Utils.RareTypes.rare:
                    localRareItemList.Add(localItem);
                    break;
                case Utils.RareTypes.mythic:
                    localMythicItemList.Add(localItem);
                    break;
                case Utils.RareTypes.legendary:
                    localLegendaryItemList.Add(localItem);
                    break;
            }
        }

        List<Utils.RareTypes> itemRareTypes = new List<Utils.RareTypes>();
        for (int i = 0; i < itemCount; i++)
        {
            itemRareTypes.Add(Utils.GetRandomRareType(rareItemChance, mythicItemChance, legendrayItemChance));
        }

        List<GameObject> loadedItemList = new List<GameObject>();

        foreach (Utils.RareTypes listRareType in itemRareTypes)
        {
            GameObject targetItem;
            try
            {
                switch (listRareType)
                {
                    case Utils.RareTypes.common:
                        targetItem = localCommonItemList[UnityEngine.Random.Range(0, localCommonItemList.Count)];
                        loadedItemList.Add(targetItem);
                        localCommonItemList.Remove(targetItem);
                        break;
                    case Utils.RareTypes.rare:
                        targetItem = localRareItemList[UnityEngine.Random.Range(0, localRareItemList.Count)];
                        loadedItemList.Add(targetItem);
                        localRareItemList.Remove(targetItem);
                        break;
                    case Utils.RareTypes.mythic:
                        targetItem = localMythicItemList[UnityEngine.Random.Range(0, localMythicItemList.Count)];
                        loadedItemList.Add(targetItem);
                        localMythicItemList.Remove(targetItem);
                        break;
                    case Utils.RareTypes.legendary:
                        targetItem = localLegendaryItemList[UnityEngine.Random.Range(0, localLegendaryItemList.Count)];
                        loadedItemList.Add(targetItem);
                        localLegendaryItemList.Remove(targetItem);
                        break;

                }
            }
            catch (ArgumentOutOfRangeException)
            {
                targetItem = localCommonItemList[UnityEngine.Random.Range(0, localCommonItemList.Count)];
                loadedItemList.Add(targetItem);
            }
        }

        /*List<int> localIndexList = new List<int>();
        localIndexList = Utils.GetListRandomNumbers(0, localItemList.Count, 3);
        Debug.Log(localIndexList);
        for (int i = 0; i < localIndexList.Count; i++)
        {
            loadedItemList.Add(localItemList[localIndexList[i]]);
        }*/
        
        List<SItemShopInfo> sortedItemList = new List<SItemShopInfo>();
        /*for (int i = localItemList.Count - 1; i >= 0; i--) {
            int minPrice = 9999999;
            foreach (GameObject item in loadedItemList)
            {
                if (item.GetComponentInChildren<ItemInfo>().shopPrice <= minPrice) {
                    minPrice = item.GetComponentInChildren<ItemInfo>().shopPrice;
                }
            }
            for (int g = loadedItemList.Count - 1; g >= 0; g--)
            {
                if (loadedItemList[g].GetComponentInChildren<ItemInfo>().shopPrice == minPrice) {
                    sortedItemList.Add(loadedItemList[g]);
                    loadedItemList.Remove(loadedItemList[g]);
                    break;
                }
            }
        }*/
        
        List<SItemShopInfo> finalDataList = new List<SItemShopInfo>();
        for (int i = 0; i < itemCount; i++)
        {
            sellItemList[i].itemPrefab = loadedItemList[i];
        }
        
        // Выясняем, сколько единиц товара будет продаваться
        foreach (SItemShopInfo itemShopInfo in sellItemList)
        {
            if (itemShopInfo.itemPrefab == null)
            {
                continue;
            }
            ItemInfo itemInfoScript = itemShopInfo.itemPrefab.GetComponentInChildren<ItemInfo>();
            
            // Ставим базовую цену. Если это расходники - цена изменится потом
            itemShopInfo.itemPrice = itemInfoScript.shopPrice;
            
            if (itemInfoScript.itemType == Utils.ItemTypes.consumables)
            {
                itemShopInfo.itemCount = UnityEngine.Random.Range(itemInfoScript.dropCountMin,  itemInfoScript.dropCountMax + 1);
                itemShopInfo.itemPrice = itemInfoScript.shopPrice * itemShopInfo.itemCount;
            }
        }
        
        for (int i = itemCount - 1; i >= 0; i--) {
            int minPrice = 9999999;
            foreach (SItemShopInfo localData in sellItemList)
            {
                if (localData.itemPrice <= minPrice) {
                    minPrice = localData.itemPrice;
                }
            }
            for (int g = sellItemList.Count - 1; g >= 0; g--)
            {
                if (sellItemList[g].itemPrice == minPrice) {
                    sortedItemList.Add(sellItemList[g]);
                    sellItemList.Remove(sellItemList[g]);
                    break;
                }
            }
        }
        
        sellItemList = sortedItemList;
        // Хочу здесь отсортировать sellItemList по полю price
    }
}
