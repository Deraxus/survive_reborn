using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hpheart_item : ItemLogic, IStatModifier
{
    public int HPUp = 5;

    public GameObject player;
    void Awake()
    {
        player = MainManager.Instance.mainPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnItemTaking()
    {
        Debug.Log("������� ������");
        MainItemManager.Instance.AddModify(MainItemManager.ModifyTypes.Health, HPUp);
    }

    public void OnItemLoosing()
    {
        MainItemManager.Instance.RemoveModify(MainItemManager.ModifyTypes.Health, HPUp);
    }
}
