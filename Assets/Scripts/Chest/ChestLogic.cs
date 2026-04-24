using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestLogic : Loot
{
    private GameObject eventManager;
    public bool canBeOpen = false;
    public bool isOpen = false;
    public bool needKey = false;

    

    // Update is called once per frame
    /*void Update()
    {
        if (canBeOpen && Input.GetKeyDown(KeyCode.E) && !isOpen)
        {
            LootDrop();
            isOpen = true;
        }
    }*/

    override public void Awake()
    {
        eventManager = MainManager.Instance.eventManager;
        EventManager.Instance.OnActivateKeyPressedEvent += OnActivateKeyPressed;
        MainManager.Instance.GetComponent<MainManager>();
        mainItemManager = MainItemManager.Instance;
    }

    void OnActivateKeyPressed()
    {
        if (needKey)
        {
            if (canBeOpen && !isOpen)
            {
                if (InventoryUtils.FindItemCell("chestKey_sprite") != null)
                {
                    GameObject vr = InventoryUtils.FindItemCell("chestKey_sprite");
                    InventoryUtils.FindItemCell("chestKey_sprite").GetComponent<CellData>().data.prefabItem.GetComponent<ChestKey_item>().OnItemUse(InventoryUtils.FindItemCell("chestKey_sprite").GetComponent<CellData>().data);
                    OpenChest();
                }
                else
                {
                    Utils.StartNewMessage("<color=white>Для открытия этого сундука необходим ключ!");
                }
            }
        }
        else
        {
            if (canBeOpen && !isOpen)
            {
                OpenChest();
            }
        }
    }

    void OpenChest()
    {
        LootDrop();
        isOpen = true;
    }

    public override IEnumerator FinishDropping(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(transform.parent.gameObject);
    }
}
