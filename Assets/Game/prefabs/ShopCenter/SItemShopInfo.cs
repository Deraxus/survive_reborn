using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shop item info", menuName = "Shop item info")]
public class SItemShopInfo : ScriptableObject
{
    public GameObject itemPrefab;
    public int itemCount = 1;
    public int itemPrice;
}
