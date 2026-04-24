using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class happyClover_item : ItemLogic, IStatModifier
{
    private MainItemManager localItemManager;

    [Tooltip("�� ������� ����� ��������� ����� ����� �������� ��������")]
    public float luckBonus = 0.5f;

    // Update is called once per frame
    void Update()
    {

    }

    public void OnItemTaking()
    {
        MainItemManager.Instance.AddModify(MainItemManager.ModifyTypes.LuckKf, 0.5f);
    }

    public void OnItemLoosing()
    {
       MainItemManager.Instance.RemoveModify(MainItemManager.ModifyTypes.LuckKf, 0.5f);
    }
}
