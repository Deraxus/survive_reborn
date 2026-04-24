using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RerollButtonLogic : MonoBehaviour
{
    public int rerollPrice = 5;
    public int rerollPriceStep = 2;
    public GameObject currentShopCentre;
    public void ShopRefresh()
    {
        if (MainManager.Instance.mainPlayer.GetComponent<Player>().coins >= rerollPrice)
        {
            MainManager.Instance.mainPlayer.GetComponent<Player>().coins -= rerollPrice;
            // Издать звук покупки
            currentShopCentre.GetComponentInChildren<ShopCenterLogic>().LoadItemList(3, MainItemManager.Instance.passiveItems, MainItemManager.Instance.weaponsSprites, MainItemManager.Instance.consumablesItems);
        }
    }
}
