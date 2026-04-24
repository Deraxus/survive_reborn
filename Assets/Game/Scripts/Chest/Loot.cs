using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public bool includeAllItems = false;
    public List<GameObject> exceptItems = new List<GameObject>();
    public List<GameObject> fullLootList = new List<GameObject>();


    //public List<GameObject> LootRare = new List<GameObject>();
    public GameObject coin;
    public GameObject mutagen;

    public int minCoinCount = 1;
    public int maxCoinCount = 20;

    [Tooltip("Шанс на то, что выпадет предмет")]
    public float itemChance = 0.2f;
    
    [Tooltip("Шанс на то, что монетка заменится на мутаген. По умолчанию - 0% шанс")]
    public float mutagenChance = 0;
    
    public float forceUp = 400f;
    public float forceLR = 100f;
    public float lootDelay = 2;
    public float fallDelay = 2;

    private float LootNumber;

    public List<GameObject> lootCommonList = new List<GameObject>();
    public List<GameObject> lootRareList = new List<GameObject>();
    public List<GameObject> lootMythicList = new List<GameObject>();
    public List<GameObject> lootLegendaryList = new List<GameObject>();

    public float rareItemChance = 0.2f;
    public float mythicItemChance = 0.05f;
    public float legendrayItemChance = 0.01f;

    GameObject publicLoot;
    public MainManager mainManager;
    public MainItemManager mainItemManager;

    public virtual void Awake()
    {
        mainManager = MainManager.Instance;
        mainItemManager = MainItemManager.Instance;
    }

    virtual public void Start()
    {
        LoadItemList();
    }

    virtual public void LootDrop()
    {
        Awake();
        LoadItemList();
        Debug.Log("�������� �������");
        float randNumber = Random.Range(0f, 1f);
        if (randNumber <= itemChance) {
 
            StartCoroutine(LootDropCor(mode : 0));
        }
        else
        {
            int randCoinNumber = Random.Range(minCoinCount, maxCoinCount);
            StartCoroutine(LootDropCor(mode : 1, dropCount : randCoinNumber));
        }
    }

    /*void SpamLoot(int lootCount, float localLootDelay) {
        for (int i = 0; i < lootCount; i++)
        {
            if (i == 0)
            {
                StartCoroutine(SpamLootCor(lootDelay));
            }
            else {
                StartCoroutine(SpamLootCor(localLootDelay));
            }

        }
    }
    IEnumerator SpamLootCor(float localLootDelay) {
        yield return new WaitForSeconds(localLootDelay);
        StartCoroutine(LootDropCor(1));

    }*/

    // mode 0 - классическое выпадение рандомного предмета
    // mode 1 - выпадение монетки (в таком случае mutagenType для мутагенов - тип выпадающих мутагенов, пока их 4)
    // mode 2 - выпадение конкретного предмета
    // mode 3 - дроп числа (вообще не выпадение предмета)
    public IEnumerator LootDropCor(int mode = 0, int dropCount = 1,int localCount = 0, Vector2 whereSpawn = default, GameObject droppedItem = null, int mutagenType = -1){  // mode = 0 - ������� �����, 1 - ������� �� �������, 2 - ��������, 3 - �������� + ���
        List<GameObject> localCommonLootList = new List<GameObject>();
        List<GameObject> localRareLootList = new List<GameObject>();
        List<GameObject> localMythicLootList = new List<GameObject>();
        List<GameObject> localLegendaryLootList = new List<GameObject>();
        foreach (GameObject obj in lootCommonList) {
            bool togl = true;
            foreach (GameObject localLoot in MainManager.Instance.mainPlayer.GetComponent<MainInventory>().allItems) { 
                if ((obj.name.Equals(localLoot.name))){
                    togl = false;
                }
            }
            if (togl) { 
                localCommonLootList.Add(obj);
            }
        }

        foreach (GameObject obj in lootRareList)
        {
            bool togl = true;
            foreach (GameObject localLoot in MainManager.Instance.mainPlayer.GetComponent<MainInventory>().allItems)
            {
                if ((obj.name.Equals(localLoot.name))){
                    togl = false;
                }
            }
            if (togl)
            {
                localRareLootList.Add(obj);
            }
        }

        foreach (GameObject obj in lootMythicList)
        {
            bool togl = true;
            foreach (GameObject localLoot in MainManager.Instance.mainPlayer.GetComponent<MainInventory>().allItems)
            {
                if ((obj.name.Equals(localLoot.name)))
                {
                    togl = false;
                }
            }

            if (togl)
            {
                localMythicLootList.Add(obj);
            }
        }

        foreach (GameObject obj in lootLegendaryList)
        {
            bool togl = true;
            foreach (GameObject localLoot in MainManager.Instance.mainPlayer.GetComponent<MainInventory>().allItems)
            {
                if ((obj.name.Equals(localLoot.name)))
                {
                    togl = false;
                }
            }

            if (togl)
            {
                localLegendaryLootList.Add(obj);
            }
        }

        /*foreach (GameObject obj in LootRare)
        {
            if (GameObject.Find("Player").GetComponent<MainInventory>().allItems.Contains(obj) == false)
            {
                localRareLootList.Add(obj);
            }
        }*/
        float coinLootDelay;
        GameObject loot = droppedItem;
        coinLootDelay = Mathf.Pow(dropCount * 3, -1);
        if (((mode == 1) || (mode == 2)) && (localCount > 0))
        {
            yield return new WaitForSeconds(coinLootDelay);
        }
        else {
            Debug.Log("����" + localCount);
            yield return new WaitForSeconds(lootDelay);
        }

        // Подготавливаем переменную для мутагенов/монет
        float randNumber = Random.Range(0f, 1f);
        if (droppedItem != null && droppedItem.tag != "mutagen" && droppedItem.tag != "coin")
        {
            loot = ItemTakingUtils.GetFullItem(droppedItem);
        }
        else if (mode == 0)
        {
            loot = GetRandomItem(localCommonLootList, localRareLootList, localMythicLootList, localLegendaryLootList);
        }
        else if (mode == 1)
        {
            loot = coin;
            
            // Пытаемся понять что выпадет: монетка или мутагены. Если повезет, монеты заменятся на мутагены
            if (droppedItem == null && randNumber <= mutagenChance)
            {
                loot = mutagen;
            }

            // Пытаемся понять, вдруг система уже решила что должно выпадать что-то конкретное
            if (droppedItem != null)
            {
                Debug.Log("Теперь мутагены.");
                loot = droppedItem;
            }
            
            if (loot.tag == "mutagen")
            {
                Debug.Log("Проверка прошла - тег проверен");
                loot.GetComponentInChildren<Mutagen>().mutagenType = mutagenType;
            }
        }
        else if (mode == 2)
        {
            loot = droppedItem;
        }

        if ((mode == 0) && (loot.GetComponentInChildren<ItemInfo>().itemType == Utils.ItemTypes.consumables) && (loot.GetComponentInChildren<ItemInfo>().dropCountMax != 1))
        {
            mode = 2;
            if (loot.GetComponentInChildren<ItemInfo>().dropCountMin == loot.GetComponentInChildren<ItemInfo>().dropCountMax)
            {
                dropCount = loot.GetComponentInChildren<ItemInfo>().dropCountMin;
            }
            else
            {
                dropCount = Random.Range(loot.GetComponentInChildren<ItemInfo>().dropCountMin, loot.GetComponentInChildren<ItemInfo>().dropCountMax);
            }
        }

        if (loot != null && loot.name == "EmptyPrefab") {
            yield break;
        }

        Vector3 vector;
        Quaternion rotation = new Quaternion(0, 0, 0, 0);

        if (whereSpawn == default)
        {
            vector = new Vector3(transform.position.x, transform.position.y, transform.position.z + 3);
            Debug.Log(vector + "���");
        }
        else
        {
            vector = new Vector3(whereSpawn.x, whereSpawn.y, 20);
            Debug.Log(vector + "���");
        }

        Debug.Log($"Инфо - мод: {mode}, loot - {loot}, droppedItem - {droppedItem}");
        if ((loot.tag != "coin") && (loot.tag != "mutagen"))
        {
            Debug.Log($"Спауню {loot.name}");
            loot = Instantiate(ItemTakingUtils.GetFullItem(loot), vector, rotation);
        }
        else
        {
            loot = Instantiate(loot, vector, rotation);
        }

        Debug.Log("���������");
        float randForce = Random.Range(forceUp - (forceUp / 10), (forceUp + (forceUp / 10)));
        float randDelayUp = randForce / 220;
        float randDelayLR = randForce / 4 - forceUp / 5;
        fallDelay = randDelayUp;
        loot.GetComponentInChildren<Rigidbody2D>().AddForce(Vector2.up * randForce * 0.02f, ForceMode2D.Impulse);

        if (Random.Range(0, 2) == 1) {
            loot.GetComponentInChildren<Rigidbody2D>().AddForce(Vector2.left * randDelayLR * 0.02f, ForceMode2D.Impulse);
        }
        else
        {
            loot.GetComponentInChildren<Rigidbody2D>().AddForce(Vector2.right * randDelayLR * 0.02f, ForceMode2D.Impulse);
        }

        Utils.StartGlobalCoroutine(StopForce(loot));

        localCount++;
        if ((mode == 1) && (localCount < dropCount))
        {
            Debug.Log("222");
            Debug.Log("Второй заход...");
            if (droppedItem != null && droppedItem.CompareTag("mutagen"))
            {
                Utils.StartGlobalCoroutine(LootDropCor(mode: 1, localCount : localCount, dropCount : dropCount, whereSpawn : whereSpawn, mutagenType : mutagenType, droppedItem : mutagen));
            }
            else if (droppedItem != null && droppedItem.CompareTag("coin"))
            {
                Utils.StartGlobalCoroutine(LootDropCor(mode: 1, localCount : localCount, dropCount : dropCount, whereSpawn : whereSpawn, mutagenType : mutagenType, droppedItem : coin));
            }
            else if (randNumber <= mutagenChance)
            {
                Utils.StartGlobalCoroutine(LootDropCor(mode: 1, localCount : localCount, dropCount : dropCount, whereSpawn : whereSpawn, mutagenType : mutagenType, droppedItem : mutagen));
            }
            else
            {
                // Умножаем на 1.5 так как монеты менее ценные чем мутагены
                // Если mutagen не указан - значит это скорее всего сундук, а там только монеты
                if (mutagen == null)
                {
                    dropCount *= (int)1.5f;
                }
                Utils.StartGlobalCoroutine(LootDropCor(mode: 1, localCount : localCount, dropCount : dropCount, whereSpawn : whereSpawn, mutagenType : mutagenType, droppedItem : coin));
            }
        }
        else if ((mode == 2) && (localCount < dropCount))
        {
            Utils.StartGlobalCoroutine(LootDropCor(mode: 2, localCount : localCount, dropCount : dropCount, droppedItem : loot, whereSpawn : whereSpawn));
        }
        else
        {
            Utils.StartGlobalCoroutine(FinishDropping(lootDelay));
        }
    }

    IEnumerator StopForce(GameObject loot)
    {
        yield return new WaitForSeconds(fallDelay/2);
        try
        {
            loot.GetComponentInChildren<ItemMagnet>().enabled = true;
        }
        catch (System.Exception)
        {
        }
        yield return new WaitForSeconds(fallDelay/2);
        loot.GetComponentInChildren<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        loot.GetComponentInChildren<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        loot.GetComponentInChildren<BoxCollider2D>().enabled = true;
    }

    GameObject GetRandomItem(List<GameObject> lootCommonLocal, List<GameObject> lootRareLocal = null, List<GameObject> lootMythicLocal = null, List<GameObject> lootLegendaryLocal = null) {
        switch (Utils.GetRandomRareType(rareItemChance, mythicItemChance, legendrayItemChance)) {
            case Utils.RareTypes.legendary:
                return lootLegendaryList[Random.Range(0, lootLegendaryList.Count)];
            case Utils.RareTypes.mythic:
                return lootMythicList[Random.Range(0, lootMythicList.Count)];
            case Utils.RareTypes.rare:
                return lootRareList[Random.Range(0, lootRareList.Count)];
            case Utils.RareTypes.common:
                return lootCommonList[Random.Range(0, lootCommonList.Count)];
            default:
                return null;
        }
    }

    public void LoadItemList()
    {
        lootCommonList.Clear();
        lootRareList.Clear();
        lootMythicList.Clear();
        lootLegendaryList.Clear();

        if (includeAllItems)
        {
            fullLootList.Clear();

            Debug.Log($"Скрипт лута готов к бою!");

            // Проблема - у подарков надо обнулять эти списки перед открытием подарка. Пока обнуление работает только если есть галочка includeAllItems, если ее убрать - могут быть проблемы. Пофиксить
            // Самое банальное - просто поставить эти строчки выше в обычном запуске Start
            foreach (GameObject item in mainItemManager.allItemList)
            {
                if (!exceptItems.Contains(item))
                {
                    fullLootList.Add(item);
                }
            }
        }
        foreach (GameObject item in fullLootList)
        {
            Utils.RareTypes rareType = item.gameObject.GetComponentInChildren<ItemInfo>().rareType;
            if (InventoryUtils.ItemCanBeDropped(item))
            {
                if (rareType == Utils.RareTypes.common)
                {
                    lootCommonList.Add(item);
                }
                else if (rareType == Utils.RareTypes.rare)
                {
                    lootRareList.Add(item);
                }
                else if (rareType == Utils.RareTypes.mythic)
                {
                    lootMythicList.Add(item);
                }
                else if (rareType == Utils.RareTypes.legendary)
                {
                    lootLegendaryList.Add(item);
                }
                else
                {
                    lootCommonList.Add(item);
                }
            }
            else
            {
                Debug.Log($"Пропускаю {item.name} - он не может выпасть");
            }
        }
    }

    public virtual IEnumerator FinishDropping(float delay)
    {
        yield return new WaitForSeconds(delay);
    }
}
