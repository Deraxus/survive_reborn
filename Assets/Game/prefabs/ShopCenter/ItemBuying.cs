using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBuying : MonoBehaviour
{
    public GameObject player;
    public GameObject mainUI;
    public GameObject emptyPrefab;

    EventManager localEventManager;

    public static ItemBuying Instance;

    void Start()
    {
        Instance = this;
        if (mainUI == null) {
            mainUI = GameObject.Find("UIObject");
        }

        if (player == null)
        {
            player = GameObject.Find("Player");
        }

        localEventManager = GameObject.FindGameObjectWithTag("eventManager").GetComponent<EventManager>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuyItem() 
    {
        GameObject mainParentObj = transform.parent.gameObject;
        SCellShopData localData = gameObject.GetComponentInParent<CellShopData>().data;
        if (player.GetComponent<Player>().coins >= localData.itemPrice)
        {
            InventoryUtils.GiveItem(localData.mainItemPrefab, mainUI.GetComponent<InventorySystem>().allInventoryCells);
            if (localData.mainItemPrefab.GetComponentInChildren<ItemInfo>().itemType == Utils.ItemTypes.consumables)
            {
                for (int i = 0; i < localData.itemCount - 1; i++)
                {
                    InventoryUtils.GiveItem(localData.mainItemPrefab, mainUI.GetComponent<InventorySystem>().allInventoryCells);
                }
            }
            player.GetComponent<Player>().coins -= localData.itemPrice;
            InventoryUtils.RemoveItemFromShopCenter(localData.mainItemPrefab, localData.linkedShopCenter);
            InventoryUtils.RedrawShopCell(ScriptableObject.CreateInstance<SItemShopInfo>(), mainParentObj);
            localEventManager.OnItemBuying(localData.mainItemPrefab);
        }
    }

}
