using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    public GameObject damageCounter;

    public delegate void OnEnemyDamageDelegate(GameObject enemy, float damage = 0);
    public delegate void OnEnemyDiedDelegate(GameObject enemy);
    public delegate void OnItemBuyingDelegate(GameObject item);
    public delegate void OnEverySecDelegate();
    public delegate void OnEveryMicSecDelegate();
    public delegate void OnActivateKeyPressedDelegate();
    
    public delegate void OnReloadDelegate(GameObject weapon);
    public delegate void OnShootDelegate(GameObject bullet);
    
    public event OnEnemyDamageDelegate OnEnemyDamageEvent;
    public event OnEnemyDiedDelegate OnEnemyDiedEvent;
    public event OnEverySecDelegate OnEverySecEvent;
    public event OnEveryMicSecDelegate OnEveryMicSecEvent;
    public event OnActivateKeyPressedDelegate OnActivateKeyPressedEvent;
    public event OnItemBuyingDelegate OnItemBuyingEvent;
    public event OnReloadDelegate OnReloadEvent;
    public event OnShootDelegate OnShootEvent;

    public GameObject mainManager;

    private KeyCode activateKey;
    private float timer;
    private int count;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        mainManager = GameObject.Find("MainManager");
        activateKey = mainManager.GetComponent<MainManager>().mainPlayer.GetComponent<InventoryController>().activateKey;
        OnEnemyDamageEvent += SpawnDamageCount;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= 0.1)
        {
            OnEveryMicSecEvent?.Invoke();
            count++;
            timer = 0;
        }
        if (count >= 10)
        {
            OnEverySecEvent?.Invoke();
            timer = 0;
            count = 0;
        }
        if (Input.GetKeyDown(activateKey))
        {
            OnActivateKeyPressed();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(activateKey))
        {
            OnActivateKeyPressed();
        }
    }

    public void OnEnemyDamage(GameObject enemy, float damage = 0) 
    {
        OnEnemyDamageEvent?.Invoke(enemy, damage);
        Debug.Log("78");
    }

    public void OnEnemyDied(GameObject enemy) {
        OnEnemyDiedEvent?.Invoke(enemy);
        Debug.Log("78");
    }

    public void OnEverySec() {
        OnEverySecEvent?.Invoke();
    }
    public void OnEveryMicSec()
    {
        OnEveryMicSecEvent?.Invoke();
    }

    public void OnActivateKeyPressed()
    {
        OnActivateKeyPressedEvent?.Invoke();
    }

    public void OnItemBuying(GameObject item = null)
    {
        OnItemBuyingEvent?.Invoke(item);
        Debug.Log($"������ ������� {item.name}!");
    }

    public void OnReload(GameObject weapon)
    {
        OnReloadEvent?.Invoke(weapon);
    }
    public void OnShoot(GameObject bullet)
    {
        OnShootEvent?.Invoke(bullet);
    }


    private int prevDir = 1;
    public void SpawnDamageCount(GameObject enemy, float damageCount)
    {
        GameObject spawnedPref = GameObject.Instantiate(damageCounter, enemy.transform.position, new Quaternion(0, 0, 0, 0));
        spawnedPref.GetComponentInChildren<DamageShowing>().damageCount = damageCount;
        damageCount = (float) Math.Round(damageCount, 1);
        prevDir = prevDir * -1;
        spawnedPref.GetComponentInChildren<DamageShowing>().prevDir = prevDir;
        spawnedPref.GetComponentInChildren<TMP_Text>().text = damageCount.ToString();
        spawnedPref.GetComponentInChildren<Canvas>().worldCamera = mainManager.GetComponent<MainManager>().mainCamera;
        Debug.Log(91);
    }
}
