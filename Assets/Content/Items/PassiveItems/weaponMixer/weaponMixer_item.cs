using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;
using static ItemManager;

public class weaponMixer_item : ItemLogic, IEverySec
{
    [SerializeField]
    public float everySecAmount = 10;

    [SerializeField]
    public float damageKfUp = 1f;

    private Dictionary<ModifyTypes, float> naturalModifiers = new Dictionary<ModifyTypes, float>();

    public void Awake()
    {
    }

    public void OnItemTaking()
    {
        naturalModifiers = new Dictionary<ModifyTypes, float>(MainItemManager.Instance.modifiers);
        MainItemManager.Instance.AddModify(ModifyTypes.DamageKf, damageKfUp);
    }

    public void OnItemLoosing()
    {
    }

    public void EverySec()
    {
        Utils.StartGlobalCoroutine(MixStats());
    }

    public IEnumerator MixStats()
    {
        while (true)
        {
            yield return new WaitForSeconds(everySecAmount);

            float allStats = MainItemManager.Instance.GetModify(ModifyTypes.DamageKf) + MainItemManager.Instance.GetModify(ModifyTypes.RateKf)
              + MainItemManager.Instance.GetModify(ModifyTypes.RecoilKf) + MainItemManager.Instance.GetModify(ModifyTypes.BulletLTKf);

            // 1 - �������

            List<int> statsList = new List<int>();
            statsList.Add(0); // �����
            statsList.Add(1); // ����������������
            statsList.Add(2); // �������
            statsList.Add(3); // ���������

            Debug.Log($"����� {allStats}\n���� - {MainItemManager.Instance.GetModify(ModifyTypes.DamageKf)}\nRate - {MainItemManager.Instance.GetModify(ModifyTypes.RateKf)}\nRecoil - {MainItemManager.Instance.GetModify(ModifyTypes.RecoilKf)}\n��������� - {MainItemManager.Instance.GetModify(ModifyTypes.BulletLTKf)}");

            for (int i = 0; i < 4; i++)
            {
                int randInt = statsList[Random.Range(0, statsList.Count)];
                float statValue = Random.Range((allStats / 8), allStats / 2);

                if (statsList.Count == 1)
                {
                    statValue = allStats;
                }
                switch (randInt)
                {
                    case 0:
                        MainItemManager.Instance.SetModify(ModifyTypes.DamageKf, statValue);
                        break;
                    case 1:
                        MainItemManager.Instance.SetModify(ModifyTypes.RateKf, statValue);
                        break;
                    case 2:
                        MainItemManager.Instance.SetModify(ModifyTypes.RecoilKf, statValue);
                        break;
                    case 3:
                        MainItemManager.Instance.SetModify(ModifyTypes.BulletLTKf, statValue);
                        break;
                }
                Debug.Log(randInt);
                
                allStats -= statValue;
                statsList.Remove(randInt);
            }
        }
    }

}
