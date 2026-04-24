using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CellShopData : MonoBehaviour
{
    public SCellShopData data;

    public GameObject spriteObject;
    public TMP_Text countText;
    public TMP_Text priceText;

    public GameObject shopItem;
    void Awake()
    {
        if (data == null)
        {
            data = ScriptableObject.CreateInstance<SCellShopData>();
        }
        //GameObject.Find("UIObject").GetComponent<InventorySystem>().RedrawItemCell(startCellItem.transform.GetChild(0).gameObject, gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
