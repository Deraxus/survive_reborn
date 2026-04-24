using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class regularClient_item : ItemLogic, IOnItemBuying
{

    private MainItemManager localItemManager;

    [Tooltip("����� ����� �� ��������� �������� ����� ���������� ������ (�������� 0.2 - 20% �� ���������)")]
    public float returnedGold = 0.25f;
    void Awake()
    {
        localItemManager = GameObject.Find("ItemManager").GetComponent<MainItemManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnItemTaking(ItemManager itemManager)
    {
        itemManager.onItemBuyingScripts.Add(this);
    }

    public void OnItemLoosing(ItemManager itemManager)
    {
        itemManager.onItemBuyingScripts.Remove(this);
    }

    public void OnItemBuying(GameObject item, ItemManager itemManager)
    {
        if (item.name != gameObject.name)
        {
            int coinCount = (int)(item.GetComponentInChildren<ItemInfo>().shopPrice * returnedGold);
            GetComponent<regularClientLoot>().minCoinCount = coinCount;
            GetComponent<regularClientLoot>().maxCoinCount = coinCount;
            GetComponent<regularClientLoot>().LootDrop();
        }
    }
}
