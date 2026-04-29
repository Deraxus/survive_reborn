using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopCellClick : MonoBehaviour
{
    public float needHoldingTime = 0.5f;   
    
    private Image cellName;
    private Image cellType;
    private Image cellDescription;
    
    private float timer = 0;
    private bool isOnCell = false;
    
    private PointerEventData lastEventData;

    private void Start()
    {
        cellName = UIManager.Instance.cellName;
        cellType = UIManager.Instance.cellType;
        cellDescription = UIManager.Instance.cellDescription;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            UseItem(gameObject);
            
            // Обновляем панель чтобы убрать отображение текста
            if (eventData.pointerEnter != null && eventData.pointerEnter.GetComponent<CellData>().data.itemName == "Empty")
            {
                print("52352355252525252");
                ResetCell();
            }
            
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if ((eventData.pointerEnter.CompareTag("inventoryCell") || (eventData.pointerEnter.CompareTag("inventoryCell"))))
        {
            if (eventData.pointerEnter.GetComponent<CellData>().data.itemName != "Empty")
            {            
                lastEventData = eventData;
                isOnCell = true;  
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if ((eventData.pointerEnter.CompareTag("inventoryCell") || (eventData.pointerEnter.CompareTag("inventoryCell"))))
        {
            ResetCell();
        }
    }

    public void ResetCell()
    {
        UIManager.Instance.cellPanelObject.SetActive(false);
        timer = 0;
        isOnCell = false;
    }
    private void Update()
    {
        if (isOnCell)
        {
            timer += Time.deltaTime;
            if (timer >= needHoldingTime)
            {
                UIManager.Instance.cellPanelObject.SetActive(true);

                RectTransform rect = UIManager.Instance.cellPanelObject.GetComponent<RectTransform>();
                Canvas canvas = UIManager.Instance.cellPanelObject.GetComponentInParent<Canvas>();

                Vector2 localPoint;

                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvas.transform as RectTransform,
                    Input.mousePosition,
                    canvas.worldCamera,
                    out localPoint
                );

                rect.anchoredPosition = localPoint;// + new Vector2(20f, -20f);
                ShowInfoPanel();
            }
        }
    }

    private void ShowInfoPanel()
    {
        switch (gameObject.tag)
        {
            case "inventoryCell":
                if (GetComponent<CellData>().data.itemName == "Empty")
                {
                    return;
                }
                InventoryUtils.RedrawInfoPanels(GetComponent<CellData>().data, cellName, cellType, cellDescription, UIManager.Instance.mainUIData);
                break;
            case "shopCell":
                InventoryUtils.RedrawShopInfoPanels(GetComponent<CellShopData>().data, cellName, cellType, cellDescription);
                break;
            default:
                break;

        }
    }

    private void UseItem(GameObject cell)
    {
        try
        {
            SCellData mainInventoryData = GameObject.Find("MainManager").GetComponent<MainManager>().mainPlayer.GetComponent<MainInventory>().mainInventory[cell.GetComponent<CellData>().data.cellIndex];
            cell.GetComponent<CellData>().data.prefabItem.GetComponent<ItemInfo>().mainItemScript.OnItemUse(mainInventoryData);
        }
        catch (Exception)
        {
            Debug.Log("123");
        }
    }
}
