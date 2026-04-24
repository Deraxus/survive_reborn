using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CursorLogic : MonoBehaviour
{
    public SUIData uidata;
    public float secondsStaying = 0f;
    public GameObject targetCell;
    public GameObject mainUiObj;

    public Image cellName;
    public Image cellType;
    public Image cellDescription;

    public GameObject currentCell;

    public GameObject cellPanelObject;
    void Start()
    {
        cellName = mainUiObj.GetComponent<InventorySystem>().cellNamePanel;
        cellType = mainUiObj.GetComponent<InventorySystem>().cellTypePanel;
        cellDescription = mainUiObj.GetComponent<InventorySystem>().cellDescriptionPanel;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 vector = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        transform.position = Camera.main.ScreenToWorldPoint(vector);
        if (secondsStaying >= 0.5f)
        {
            switch (currentCell.tag) {
                case "inventoryCell":
                    InventoryUtils.RedrawInfoPanels(currentCell.GetComponent<CellData>().data, cellName, cellType, cellDescription, uidata);
                    break;
                case "shopCell":
                    InventoryUtils.RedrawShopInfoPanels(currentCell.GetComponent<CellShopData>().data, cellName, cellType, cellDescription);
                    break;
                default:
                    break;

            }
            cellPanelObject.SetActive(true);
        }
        else {
            cellPanelObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0) && (targetCell != null))
        {
            UseItem(targetCell);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "shopCell") || (collision.gameObject.tag == "inventoryCell")) {
            if (collision.gameObject.tag == "inventoryCell")
            {
                targetCell = collision.gameObject;
            }
            try
            {
                if ((collision.gameObject.tag == "shopCell") || (!collision.GetComponent<CellData>().data.isFree)) {
                    secondsStaying += Time.deltaTime;
                    currentCell = collision.gameObject;
                }
            }
            catch (NullReferenceException){
                
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "inventoryCell")
        {
            targetCell = null;
        }
        if ((collision.gameObject.tag == "inventoryCell") || (collision.gameObject.tag == "shopCell")) {
            secondsStaying = 0;
            currentCell = null;
        }
    }

    private void UseItem(GameObject cell)
    {
        try
        {
            SCellData mainInventoryData = GameObject.Find("MainManager").GetComponent<MainManager>().mainPlayer.GetComponent<MainInventory>().mainInventory[cell.GetComponent<CellData>().data.cellIndex];
            cell.GetComponent<CellData>().data.prefabItem.GetComponent<ItemInfo>().mainItemScript.OnItemUse(mainInventoryData);
        }
        catch (DivideByZeroException)
        {
            Debug.Log("123");
        }
    }
}
