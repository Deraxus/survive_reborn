using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{

    public List<IOnEnemyDied> onEnemyDiedScripts = new List<IOnEnemyDied>();
    public List<IOnEnemyDamage> onEnemyDamageScripts = new List<IOnEnemyDamage>();
    public List<IStatModifier> statModifiersScripts = new List<IStatModifier>();
    public List<IOnItemBuying> onItemBuyingScripts = new List<IOnItemBuying>();
    public List<IEverySec> everySecScripts = new List<IEverySec>();

    public GameObject player;

    public List<GameObject> passiveItems;
    public List<GameObject> weaponsSprites;
    public List<GameObject> weaponsItems;
    public List<GameObject> consumablesItems;

    public List<GameObject> allItemList;

    public Dictionary<ModifyTypes, float> modifiers = new Dictionary<ModifyTypes, float>();

    public virtual void Awake()
    {

        modifiers[ModifyTypes.Health] = Player.Instance.MaxHP;
        modifiers[ModifyTypes.Speed] = Player.Instance.speed;

        modifiers[ModifyTypes.SpeedKf] = 1.0f;
        modifiers[ModifyTypes.DamageKf] = 1.0f;
        modifiers[ModifyTypes.RateKf] = 1.0f;
        modifiers[ModifyTypes.LuckKf] = 1.0f;
        modifiers[ModifyTypes.RecoilKf] = 1.0f;
        modifiers[ModifyTypes.BulletLTKf] = 1.0f;

        foreach (GameObject item in passiveItems)
        {
            allItemList.Add(item);
        }

        foreach (GameObject item in weaponsSprites)
        {
            allItemList.Add(item);
        }

        foreach (GameObject item in consumablesItems)
        {
            allItemList.Add(item);
        }

        EventManager.Instance.OnEnemyDiedEvent += OnEnemyDied;
        EventManager.Instance.OnEnemyDamageEvent += OnEnemyDamage;
        EventManager.Instance.OnItemBuyingEvent += OnItemBuying;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public enum ModifyTypes
    {
        Health,
        Speed,
        Damage,

        HealthKf,
        SpeedKf,
        DamageKf,
        RateKf,
        LuckKf,
        RecoilKf, // ��� ���� ����, ��� ������� ������
        BulletLTKf, // ��� ���� ����, ��� ������ ����� ����
        PatronsKF, // ��� ���� ����, ��� ������ ������ ����� � ������ � �������
        SomethingElse,
    }


    public float GetModify(ModifyTypes type)
    {
        return modifiers[type];
    }

    public void SetModify(ModifyTypes type, float value)
    {
        modifiers[type] = value;
        ReconnectModify(type);
    }

    public void AddModify(ModifyTypes type, float value)
    {
        modifiers[type] += value;
        ReconnectModify(type, value);
    }

    public void RemoveModify(ModifyTypes type, float value)
    {
        modifiers[type] -= value;
        ReconnectModify(type, value);
    }

    public void ResetModify(ModifyTypes type)
    {
        modifiers[type] = 1;
        ReconnectModify(type);
    }

    public void ReconnectModify(ModifyTypes type, float upValue = 0)
    {
        Debug.Log("222");
        switch (type)
        {
            case ModifyTypes.Health:
                Player.Instance.MaxHP = modifiers[type];
                if (Player.Instance.HP + upValue > Player.Instance.MaxHP)
                {
                    Player.Instance.HP = modifiers[type];
                }
                else
                {
                    Player.Instance.HP = Player.Instance.HP + upValue;
                }
                break;

            case ModifyTypes.Speed:
                break;

            case ModifyTypes.SpeedKf:
                Debug.Log("333");
                Player.Instance.speedKf = modifiers[type];
                break;

            case ModifyTypes.DamageKf:
                Player.Instance.damageKf = modifiers[type];
                break;

            case ModifyTypes.RateKf:
                Player.Instance.rateKf = modifiers[type];
                break;
        }

        
    }

    public void RegisterPassiveItem(GameObject prefabItem)
    {
        ItemLogic localItemScript = prefabItem.GetComponentInChildren<ItemInfo>().mainItemScript;
        if (localItemScript == null)
        {
            localItemScript = prefabItem.GetComponent<ItemInfo>().mainItemScript;
        }

        {
            
        }
        if (localItemScript is IOnEnemyDied onDied)
        {
            onEnemyDiedScripts.Add(onDied);
        }

        if (localItemScript is IOnEnemyDamage onDamage)
        {
            onEnemyDamageScripts.Add(onDamage);
        }

        if (localItemScript is IStatModifier modifier)
        {
            statModifiersScripts.Add(modifier);
            modifier.OnItemTaking();
        }

        if (localItemScript is IOnItemBuying onBuying)
        {
            onItemBuyingScripts.Add(onBuying);
        }

        if (localItemScript is IEverySec everySec)
        {
            Debug.Log("��������");
            everySecScripts.Add(everySec);
            everySec.OnItemTaking();
            everySec.EverySec();
        }
    }

    public void RemovePassiveItem(GameObject prefabItem)
    {
        ItemLogic localItemScript = prefabItem.GetComponentInChildren<ItemInfo>().mainItemScript;
        if (localItemScript == null)
        {
            localItemScript = prefabItem.GetComponent<ItemInfo>().mainItemScript;
        }

        {

        }
        if (localItemScript is IOnEnemyDied onDied)
        {
            onEnemyDiedScripts.Remove(onDied);
        }

        if (localItemScript is IStatModifier modifier)
        {
            statModifiersScripts.Remove(modifier);
            modifier.OnItemLoosing();
        }

        if (localItemScript is IOnItemBuying onBuying)
        {
            onItemBuyingScripts.Remove(onBuying);
        }

        if (localItemScript is IEverySec everySec)
        {
            Debug.Log("��������");
            everySecScripts.Remove(everySec);
            everySec.OnItemLoosing();
        }
    }

    public void OnEnemyDied(GameObject enemy)
    {
        foreach (IOnEnemyDied localScript in onEnemyDiedScripts)
        {
            localScript.OnEnemyDied(enemy);
        }
    }

    public void OnItemBuying(GameObject item)
    {
        foreach (IOnItemBuying localScript in onItemBuyingScripts)
        {
            localScript.OnItemBuying(item, this);
        }
    }

    public void OnEnemyDamage(GameObject enemy, float damage)
    {
        foreach (IOnEnemyDamage localScript in onEnemyDamageScripts)
        {
            localScript.OnEnemyDamage(enemy, damage);
        }
    }
}
