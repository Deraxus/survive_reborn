using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class MainInventory : MonoBehaviour
{
    [Tooltip("Объекты указывать в том виде, в котором они попадаются в игре (в виде sprite)")]
    public List<GameObject> startStuff = new List<GameObject>();


    public SCellData[] mainInventory = new SCellData[15];
    public List<GameObject> passiveItems = new List<GameObject>();
    public int currentCursor = 0; // 10 ячеек инвентаря
    public List<GameObject> old_items = new List<GameObject>();

    public List<GameObject> allHands = new List<GameObject>();
    public List<GameObject> allItems = new List<GameObject>();

    public GameObject currentWeapon = null;

    public bool canSwap = true;
    private string currentItem = "";
    private string itemName = "";
    // Start is called before the first frame update
    void Start()
    {
        InventorySystem localInventorySystem = MainManager.Instance.UIManager.GetComponent<InventorySystem>();
        canSwap = true;
        for (int i = 0; i < mainInventory.Length; i++)
        {
            //mainInventory[i] = SCellData.CreateInstance<SCellData>();
            mainInventory[i] = localInventorySystem.allInventoryCells[i].GetComponent<CellData>().data;
            mainInventory[i].cellIndex = i;
        }
        Debug.Log(startStuff.Count);
        foreach (GameObject obj in startStuff)
        {
            GameObject a = ItemTakingUtils.GetFullItem(obj);
            Debug.Log(a.name + "Привет");
            InventoryUtils.GiveItem(a, localInventorySystem.allInventoryCells);
            Debug.Log("ggg");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("1") && canSwap == true && currentCursor != 0) { 
            currentCursor = 0;
        }
        else if (Input.GetKey("2") && canSwap == true && currentCursor != 1)
        {
            currentCursor = 1;
        }
        else if (Input.GetKey("3") && canSwap == true && currentCursor != 2)
        {
            currentCursor = 2;
        }
        else if (Input.GetKey("4") && canSwap == true && currentCursor != 3)
        {
            currentCursor = 3;
        }
        else if (Input.GetKey("5") && canSwap == true && currentCursor != 4)
        {
            currentCursor = 4;
        }
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if ((scroll > 0f) && (currentCursor < 4) && canSwap)
        {
            currentCursor += 1;
        }
        else if ((scroll < 0f) && (currentCursor > 0) && canSwap) {
            currentCursor -= 1;
        }
        ChangeWeapon(currentCursor);
        /*switch (mainInventory[currentCursor].prefabItem.name) {
            case "blaster_weapon":
                if (currentItem != "blaster_weapon")
                {
                    ChooseWeapon("blaster_weapon", "hand1_pistol");
                }
                break;
            case "pistol_weapon":
                ChooseWeapon("pistol_weapon", "hand1_pistol");
                break;
            case "weapon_ak":
                if (currentItem != "weapon_ak")
                {
                    InventoryClear("");
                    GameObject obj1 = transform.Find("hand1_ak").gameObject;
                    GameObject obj2 = transform.Find("hand2_ak").gameObject;
                    obj1.GetComponent<SpriteRenderer>().enabled = true;
                    obj2.GetComponent<SpriteRenderer>().enabled = true;
                    foreach (Transform child in obj1.transform)
                        child.gameObject.SetActive(true);
                    foreach (Transform child in obj2.transform)
                        child.gameObject.SetActive(true);
                    currentItem = "weapon_ak";
                }
                break;
            // АВТОМАТЫ В ДОРАБОТКЕ!
            // БЛИЖАЙШИЕ ЗАДАЧИ - АВТОМАТИЧЕСКОЕ ПОДТЯГИВАНИЕ ПРЕДМЕТОВ ИЗ ОБЪЕКТА НА СЦЕНЕ, ЧТОБЫ НЕ ПИСАТЬ ВЕЧНО weapon_blaster

        }*/
    }

    void ChooseWeapon(string weaponName, string hand1Name)
    {
        int currentFaceKf = 1;
        float currentOffset = 0;
        if (currentItem != weaponName)
        {
            if (currentWeapon != null)
            {
                // Отключаем все фишки старого оружия
                Weapon oldWeaponScript = currentWeapon.GetComponentInChildren<Weapon>();
                if (oldWeaponScript != null)
                {
                    currentFaceKf = oldWeaponScript.faceKf;
                    currentOffset = oldWeaponScript.offset;
                    foreach (WeaponFeature weaponFeature in oldWeaponScript.weaponFeatures)
                    {
                        weaponFeature.OnWeaponHandLoosing();
                    }
                }   
            }
            
            itemName = weaponName;
            InventoryClear(itemName);
            GameObject obj1 = transform.Find("Visuals").Find("RightHandAnchor").Find(hand1Name).gameObject;
            GameObject obj2 = transform.Find("Visuals").Find("LeftHandAnchor").Find(hand1Name.Replace("1", "2")).gameObject;
            obj1.GetComponent<SpriteRenderer>().enabled = true;
            obj2.GetComponent<SpriteRenderer>().enabled = true;
            foreach (Transform obj in obj1.transform)
                if (obj.name == itemName)
                {
                    obj.gameObject.SetActive(true);
                    currentWeapon = obj.gameObject;
                    
                    // Подключаем фишки нового оружия которое мы щас взяли
                    Weapon weaponScript = currentWeapon.GetComponentInChildren<Weapon>();
                    if (weaponScript != null)
                    {
                        weaponScript.faceKf = currentFaceKf;
                        weaponScript.offset = currentOffset;
                        foreach (WeaponFeature weaponFeature in weaponScript.weaponFeatures)
                        {
                            weaponFeature.OnWeaponHandTaking();
                        }
                    }
                }
            currentItem = itemName;
        }
    }

    void UseItem(string itemName)
    {
    
    }

    public bool InventoryClear(string itemName) 
    {
        /*if (item_name != currentItem)
        {
            for (int i = 0; i <= old_items.Count - 1; i++)
            {
                old_items[i].SetActive(false);
            }
            currentItem = inventory[currentCursor].name;
            return true;
        }
        else {
            return false;
        }*/
        for (int i = 0; i <= allHands.Count - 1; i++)
        {
            allHands[i].GetComponent<SpriteRenderer>().enabled = false;
            foreach (Transform child in allHands[i].transform)
            {
                if (child.name != itemName)
                {
                    child.gameObject.SetActive(false);
                }
            }
            /*for (int i = 0; i <= allItems.Count - 1; i++) {
                allItems[i].SetActive(false);
            }*/
            
        }
        return true;
    }

    public void HandCleaner(GameObject hand, string itemName) {
        foreach (Transform item in hand.transform)
            if (item.gameObject.name != itemName) {
                item.gameObject.SetActive(false);
                Debug.Log(item.name);
            }

    }

    public int GetFreeCell() { 
        for (int i = 0;i < mainInventory.Length;i++)
        {
            if (mainInventory[i].prefabItem == null)
            {
                return i;
            }
        }
        return currentCursor;
    }

    // Метод принимает на вход индекс ячейку, на которую будет менять, и свапает оружие игрока на то, которое в ячейке.
    public void ChangeWeapon(int cellCursorIndex)
    {
        SCellData cellData = InventorySystem.Instance.allInventoryCells[cellCursorIndex].GetComponent<CellData>().data;
        string realWeaponName = cellData.prefabItem.name;
        
        // Тут надо будет добавить тип руки для других оружий, например для дробовиков. Пока - только пистолетные руки
        ChooseWeapon(realWeaponName, "hand1_pistol");
        
    }

}
