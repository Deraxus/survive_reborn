using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SUIData : ScriptableObject
{
    [SerializeField] public string commonColor = "<color=white>";
    [SerializeField] public string rareColor = "<color=#00BFFF>";
    [SerializeField] public string mythicColor = "<color=purple>";
    [SerializeField] public string legendaryColor = "<color=red>";

    [SerializeField] public Sprite commonCell;
    [SerializeField] public Sprite rareCell;
    [SerializeField] public Sprite mythicCell;
    [SerializeField] public Sprite legendaryCell;
    
    [SerializeField] public Sprite commonShopCell;
    [SerializeField] public Sprite rareShopCell;
    [SerializeField] public Sprite mythicShopCell;
    [SerializeField] public Sprite legendaryShopCell;

    [SerializeField] public GameObject mainManager;
    [SerializeField] public GameObject itemManager;
    [SerializeField] public GameObject eventManager;
    [SerializeField] public GameObject uiobject;

   
    [SerializeField] public float magnetSpeed = 2f;
}
