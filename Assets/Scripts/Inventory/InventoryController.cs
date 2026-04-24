using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject tabPanel;
    public GameObject inventoryPanel;
    public GameObject shopPanel;
    public GameObject craftingPanel;
    public GameObject consoleObj;
    public KeyCode inventoryOpenKey = KeyCode.Tab;
    public KeyCode consoleOpenKey = KeyCode.BackQuote;
    public KeyCode activateKey = KeyCode.E;

    private bool consoleShowing = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(inventoryOpenKey))
        {
            tabPanel.SetActive(true);
        }
        else {
            tabPanel.SetActive(false);
        }

        if (Input.GetKeyDown(consoleOpenKey) && (consoleShowing == false))
        {
            consoleShowing = true;
        }
        else if (Input.GetKeyDown(consoleOpenKey) && (consoleShowing == true)) {
            consoleShowing = false;
        }

        if (Input.GetKeyDown(consoleOpenKey)) {
            Debug.Log("������ �������");
        }

        if ((consoleShowing) && (consoleObj.activeSelf == false)) {
            consoleObj.SetActive(true);
        }
        else if (!consoleShowing) {
            consoleObj.SetActive(false);
        }
    }

}
