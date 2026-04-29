using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SRoomInfoGenerator : ScriptableObject
{
    public GameObject escapeBlock;
    
    [Header("��������")]
    public GameObject startRoom;

    [Tooltip("��� �������� ������ ������� ����� ���������")]
    public List<GameObject> publicRooms = new List<GameObject>();

    [Tooltip("�������� ���������")]
    public List<GameObject> publicCorridorsVertical = new List<GameObject>();
    public List<GameObject> publicCorridorsHorizontal = new List<GameObject>();

    [Header("��������� ����������")]
    [Tooltip("������� �������� ������ ����� ���������� ���������")]
    public int roomLimit = 50;

    [Tooltip("������� ������� ������ ����� ���� ����������")]
    public int roomLimitMin = 20;

    [Tooltip("��������� ������ � ������ ����� ������ ��������� ��� ������ ������")]
    public int farFromSpawn = 5;

    [Tooltip("���������� ������� ��������")]
    public int enemyCampsCount = 1;

    [Tooltip("���� �� ����� ������� � ������, �������� - 0.52, 0.8")]
    public float roomSpawnChance = 0.5f; // ���� �� ����� �������, �� ��������� 2 - 50%, 3 - 66%...

    public float rareRoomChance = 0.2f;
    public float mythicRoomChance = 0.5f;
    public float legendaryRoomChance = 0.01f; // ����� �� ����� ������ ��������� ������ �� 0 �� 100

    [Header("������ ������")]
    public GameObject skeleton;
    public GameObject spiders;

    public int EasyRoomSpawnersCount = 3;
    public int MediumRoomSpawnersCount = 5;
    public int HardRoomSpawnersCount = 8;
    public int InsaneRoomSpawnersCount = 15;

    public float mediumChance = 0.3f;
    public float hardChance = 0.2f;
    public float insaneChance = 0.05f;

    [Header("������� � ������ �������")]
    public GameObject classicChest;
    public GameObject classicShopCenter;

       
}
