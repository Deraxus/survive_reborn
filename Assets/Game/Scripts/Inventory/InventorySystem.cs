using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;

    public List<GameObject> allInventoryCells = new List<GameObject>();
    public List<GameObject> allShopCells = new List<GameObject>();
    public GameObject rerollShopButton;

    public GameObject ItemManagerTrue;
    public GameObject ItemManagerSleep;

    public GameObject startItemSprite;

    public Image cellNamePanel;
    public Image cellTypePanel;
    public Image cellDescriptionPanel;

    public Image inventoryPanel;
    public Image shopPanel;
    public Image craftingPanel;
    public Image consolePanel;

    float seconds;
    bool started = false;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //GetStartItem(startItemSprite);
        shopPanel.GetComponent<GridLayoutGroup>().enabled = false;
    }

    // Update is called once per frame
    /*void Update()
    {
        if (!started) {
            seconds += Time.deltaTime;
            if (seconds >= 0.5f)
            {
                InventoryUtils.GiveItem(startItemSprite, allInventoryCells);
                started = true;
            }
        }
    }*/

    public void RedrawMainInventoryCell(int mainInventoryCellIndex, GameObject item) {
        GameObject localItem = null;
        localItem = ItemTakingUtils.GetFullItem(item);
        GameObject.Find("Player").GetComponent<MainInventory>().mainInventory[mainInventoryCellIndex].prefabItem = localItem;
    }


    public bool OnItemTaking(GameObject item, GameObject publicTargetCell = null) {
        Debug.Log("������� ������ � ����������");
        GameObject targetCell;
        if (publicTargetCell != null) {
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
            if (targetCell == null) {
                return false;
            }
            targetCell.GetComponent<CellData>().data.itemCount = 1;
        }
        else {
            targetCell = InventoryUtils.FindItemCell(item.GetComponent<ItemInfo>().itemName);
            targetCell.GetComponent<CellData>().data.itemCount += 1;
        }
        InventoryUtils.RedrawItemCell(item, allInventoryCells, targetCell);
        if ((targetCell.GetComponent<CellData>().data.cellIndex != -1) && (item.GetComponent<ItemInfo>().itemType == Utils.ItemTypes.weapon)) {
            RedrawMainInventoryCell(targetCell.GetComponent<CellData>().data.cellIndex, item);
        }
        Debug.Log($"����������� ������ {targetCell.name}");
        return true;
    }
    
    public void CloseInventory()
    {
        foreach (GameObject cell in allInventoryCells)
        {
            cell.GetComponent<CellClick>().ResetCell();
        }
    }

    /*public bool GetStartItem(GameObject item) {
        GameObject localItem = item.transform.GetChild(0).gameObject;
        OnItemTaking(localItem, allInventoryCells[0]);
        return true;
    }*/
}
