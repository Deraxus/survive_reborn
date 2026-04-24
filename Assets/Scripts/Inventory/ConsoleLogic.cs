using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleLogic : MonoBehaviour
{
    public GameObject player;

    public GameObject detectBlock;

    public TMP_InputField inputField; // Ссылка на InputField
    public TMP_Text outputText; // Ссылка на Text для вывода информаци

    public string itemManagerName = "ItemManager"; // Необязательно, можно указать в инспекторе объект 
    public string UIObjectName = "UIObject"; // Необязательно, можно указать в инспекторе объект 
    public GameObject itemManager;
    public GameObject UIObject;


    List<GameObject> allItems = new List<GameObject>();
    List<GameObject> passiveItems = new List<GameObject>();
    List<GameObject> weaponsItems = new List<GameObject>();
    List<GameObject> consumablesItems = new List<GameObject>();


    private bool techBool;
    void Start()
    {
        inputField.onEndEdit.AddListener(InputLogic);
        if (itemManager == null)
        {
            itemManager = GameObject.Find(itemManagerName);
        }
        if (UIObject == null)
        {
            UIObject = GameObject.Find(UIObjectName);
        }
        allItems = itemManager.GetComponent<MainItemManager>().allItemList;
        passiveItems = itemManager.GetComponent<MainItemManager>().passiveItems;
        weaponsItems = itemManager.GetComponent<MainItemManager>().weaponsSprites;
        consumablesItems = itemManager.GetComponent<MainItemManager>().consumablesItems;

        if (player == null)
        {
            player = GameObject.Find("MainManager").GetComponent<MainManager>().mainPlayer;
        }
    }

    private void InputLogic(string inputText)
    {
        string[] fullCommand = inputText.Split(" ");
        string commandString = inputText.Split(" ")[0];
        switch (commandString)
        {
            case "give":
                if (fullCommand[1].Equals("money") || fullCommand[1].Equals("coins"))
                {
                    string AmountString = inputText.Split(" ")[2];
                    GiveMoneyCommand(int.Parse(AmountString));
                }
                else
                {
                    string IDString = inputText.Split(" ")[1];
                    GiveCommand(int.Parse(IDString));
                }
                break;
            case "method":
                MethodCommand(inputText);
                break;
            case "tp":
                TeleportCommand(int.Parse(fullCommand[1]), int.Parse(fullCommand[2]), int.Parse(fullCommand[3]));
                break;
            case "clear":
                if (fullCommand.Length == 1)
                {
                    ClearInventoryCommand();
                }
                else
                {
                    ClearInventoryCommand(int.Parse(fullCommand[1]));
                }
                break;
        }
    }
    void Update()
    {

    }

    void GiveCommand(int ID)
    {
        List<GameObject> itemList = new List<GameObject>();
        switch (ID.ToString()[0].ToString())
        {
            case "1":
                itemList.AddRange(weaponsItems);
                break;
            case "2":
                itemList.AddRange(passiveItems);
                break;
            case "3":
                itemList.AddRange(consumablesItems); 
                break; 
        }
        foreach (GameObject item in itemList)
        {
            ItemInfo localItemInfo = item.gameObject.GetComponentInChildren<ItemInfo>();
            if (localItemInfo.itemID == ID)
            {
                if (InventoryUtils.GiveItem(item, UIObject.GetComponent<InventorySystem>().allInventoryCells))
                {
                    outputText.text += $"\n> Предмет {localItemInfo.itemName} был выдан";
                }
                else
                {
                    outputText.text += $"\n> Ошибка при выдаче предмета";
                }
            }
        }
    }

    void GiveMoneyCommand(int amount)
    {
        PlayerUtils.GiveMoney(amount);
        outputText.text += $"\n> Игроку было выдано {amount} монет";
    }

    void TeleportCommand(int entityId, float x, float y)
    {
        if (entityId == 0)
        {
            Utils.TeleportEntity(x, y, player);
            outputText.text += $"\n> Игрок был успешно телепортирован на координаты {x} {y}";

        }
    }

    void ClearInventoryCommand(int cellIndex = -1)
    {
        if (cellIndex == -1)
        {
            for (int i = 1; i < InventorySystem.Instance.allInventoryCells.Count; i++)
            {
                InventoryUtils.ClearInventoryCell(InventorySystem.Instance.allInventoryCells[i]);
            }
            outputText.text += $"\n> Инвентарь был полностью очищен";
        }

        else
        {
            InventoryUtils.ClearInventoryCell(InventorySystem.Instance.allInventoryCells[cellIndex - 1]);
            outputText.text += $"\n> Ячейка под номером {cellIndex} была очищена";
        }

    }

    void MethodCommand(string fullCommand) { 
        string[] fullCommandList = fullCommand.Split(' ');
        switch (fullCommandList[1]) {
            case "DetectBlockAvailable":
                StartCoroutine(Utils.CDetectBlockAvailable(new Vector3(int.Parse(fullCommandList[2]), int.Parse(fullCommandList[3]), 0), detectBlock, TechMethod));
                break;
        }
    }

    void TechMethod(bool smth) {
        Debug.Log("запустил");
        if (smth)
        {
            outputText.text += $"\n> Проверка прошла успешно";
        }
        else
        {
            outputText.text += $"\n> Проверка не прошла";
        }
    }

}
