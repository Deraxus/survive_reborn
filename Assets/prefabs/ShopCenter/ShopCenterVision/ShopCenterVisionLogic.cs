using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCenterVisionLogic : MonoBehaviour
{
    public GameObject mainUI;
    public GameObject shopCenterParent;
    private ShopCenterLogic localShopCenterLogic;
    void Start()
    {
        if (mainUI == null)
        {
            mainUI = GameObject.Find("UIObject");
        }
        localShopCenterLogic = shopCenterParent.GetComponentInChildren<ShopCenterLogic>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            if (!localShopCenterLogic.firstContactLoaded)
            {
                localShopCenterLogic.LoadItemList(3, MainItemManager.Instance.passiveItems, MainItemManager.Instance.weaponsSprites, MainItemManager.Instance.consumablesItems);
                localShopCenterLogic.firstContactLoaded = true;
            }
            mainUI.GetComponent<InventorySystem>().shopPanel.gameObject.SetActive(true);
            InventoryUtils.RedrawShopCell(shopCenterParent.GetComponent<ShopCenterLogic>().sellItemList[0], mainUI.GetComponent<InventorySystem>().allShopCells[0], shopCenterParent);
            InventoryUtils.RedrawShopCell(shopCenterParent.GetComponent<ShopCenterLogic>().sellItemList[1], mainUI.GetComponent<InventorySystem>().allShopCells[1], shopCenterParent);
            InventoryUtils.RedrawShopCell(shopCenterParent.GetComponent<ShopCenterLogic>().sellItemList[2], mainUI.GetComponent<InventorySystem>().allShopCells[2], shopCenterParent);

            InventorySystem.Instance.rerollShopButton.GetComponentInChildren<RerollButtonLogic>().currentShopCentre = shopCenterParent;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            mainUI.GetComponent<InventorySystem>().shopPanel.gameObject.SetActive(false);
        }
    }
}
