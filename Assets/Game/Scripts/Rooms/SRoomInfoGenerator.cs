using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SRoomInfoGenerator : ScriptableObject
{
    [Header("Основное")]
    public GameObject startRoom;

    [Tooltip("Все вариации комнат которые могут появиться")]
    public List<GameObject> publicRooms = new List<GameObject>();

    [Tooltip("Варианты коридоров")]
    public List<GameObject> publicCorridorsVertical = new List<GameObject>();
    public List<GameObject> publicCorridorsHorizontal = new List<GameObject>();

    [Header("Параметры генератора")]
    [Tooltip("Сколько максимум комнат может заспаунить генератор")]
    public int roomLimit = 50;

    [Tooltip("Сколько минимум комнат может быть заспаунено")]
    public int roomLimitMin = 20;

    [Tooltip("Насколько высоко и широко может отойти генератор при спауне комнат")]
    public int farFromSpawn = 5;

    [Tooltip("Количество лагерей монстров")]
    public int enemyCampsCount = 1;

    [Tooltip("Шанс на спаун комнаты в дробях, например - 0.52, 0.8")]
    public float roomSpawnChance = 0.5f; // Шанс на спавн комнаты, по умолчанию 2 - 50%, 3 - 66%...

    public float rareRoomChance = 0.2f;
    public float mythicRoomChance = 0.5f;
    public float legendaryRoomChance = 0.01f; // Шансы на спаун каждой категории комнат от 0 до 100

    [Header("Секция врагов")]
    public GameObject skeleton;
    public GameObject spiders;

    public int EasyRoomSpawnersCount = 3;
    public int MediumRoomSpawnersCount = 5;
    public int HardRoomSpawnersCount = 8;
    public int InsaneRoomSpawnersCount = 15;

    public float mediumChance = 0.3f;
    public float hardChance = 0.2f;
    public float insaneChance = 0.05f;

    [Header("Сундуки и другие награды")]
    public GameObject classicChest;
    public GameObject classicShopCenter;

       
}
