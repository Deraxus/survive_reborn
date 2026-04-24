using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CellData : MonoBehaviour
{
    public SCellData data;

    public GameObject spriteObject;
    public TMP_Text countText;
    public GameObject startCellItem = null;
    void Awake()
    {
        if (data == null) {
            data = ScriptableObject.CreateInstance<SCellData>();
        }
        //GameObject.Find("UIObject").GetComponent<InventorySystem>().RedrawItemCell(startCellItem.transform.GetChild(0).gameObject, gameObject);
    }

    private void Start()
    {
        if (data != null)
        {
            data.mainInventoryCell = gameObject;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
