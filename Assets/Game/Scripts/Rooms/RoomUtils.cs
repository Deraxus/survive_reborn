using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.Mathf;

public static class RoomUtils
{
    public enum RoomTypes { unknown, defaultRoom, enemyCamp, shop};

    public enum RoomSide { left, right, top, bot };

    public enum DirectionType { fromBotToTop, fromTopToBot, fromLeftToRight, fromRightToLeft, noDirection };

    public enum RoomSizes { tiny, small, medium, big };
    public enum CampDifficults { noCamp, easy, medium, hard, insane };

    public enum CampFillings { skeletons, spiders };
    public enum ChestTypes { classicChest, classicShopCenter };



    public static void SelectCampTypes(List<GameObject> roomList, float mediumCampChance, float hardCampChance, float insaneCampChance)
    {
        foreach (GameObject room in roomList)
        {
            room.GetComponent<RoomInfo>().roomType = RoomUtils.RoomTypes.enemyCamp;
            Utils.RareTypes rareType = Utils.GetRandomRareType(mediumCampChance, hardCampChance, insaneCampChance);
            switch (rareType)
            {
                case Utils.RareTypes.legendary:
                    room.GetComponent<RoomInfo>().campDifficult = RoomUtils.CampDifficults.insane;
                    break;
                case Utils.RareTypes.mythic:
                    room.GetComponent<RoomInfo>().campDifficult = RoomUtils.CampDifficults.hard;
                    break;
                case Utils.RareTypes.rare:
                    room.GetComponent<RoomInfo>().campDifficult = RoomUtils.CampDifficults.medium;
                    break;
                case Utils.RareTypes.common:
                    room.GetComponent<RoomInfo>().campDifficult = RoomUtils.CampDifficults.easy;
                    break;
                default:
                    break;
            }
            room.GetComponent<RoomInfo>().campFilling = RoomUtils.GetRandomCampFillings<RoomUtils.CampFillings>();
            // ��������
            // ����������� � ������� ���������� ���������� �������� � Enum, ������� ���, ����� ������� ���������� � enum ��������� NoEnum (�� �����������)
            // ������ ������������� ������, ������� ����� ��������� ������� �� ������� �������
        }
    }

    public static List<GameObject> GetRandomSpawners(GameObject room, int spawnersCount)
    {
        List<GameObject> spawners = new List<GameObject>();
        spawners.AddRange(room.GetComponent<RoomInfo>().enemySpawners);
        List<GameObject> goldSpawners = new List<GameObject>();
        goldSpawners.AddRange(room.GetComponent<RoomInfo>().enemySpawners);
        List<GameObject> returnedList = new List<GameObject>();
        GameObject spawner = null;
        for (int i = 0; i < spawnersCount; i++)
        {
            if (spawners.Count == 0)
            {
                spawner = goldSpawners[UnityEngine.Random.Range(0, goldSpawners.Count)];
            }
            else
            {
                spawner = spawners[UnityEngine.Random.Range(0, spawners.Count)];
                spawners.Remove(spawner);
            }
            returnedList.Add(spawner);
        }
        return returnedList;
    }

    public static void SpawnEnemyOnSpawners(List<GameObject> spawners, RoomUtils.CampFillings filling, SRoomInfoGenerator data)
    {
        GameObject enemy = null;
        List<GameObject> localSpawners = new List<GameObject>();
        localSpawners.AddRange(spawners);
        switch (filling)
        {
            case RoomUtils.CampFillings.skeletons:
                enemy = data.skeleton;
                break;
            case RoomUtils.CampFillings.spiders:
                enemy = data.spiders;
                break;
        }
        foreach (GameObject spawner in localSpawners)
        {
            GameObject spawnedEnemy = GameObject.Instantiate(enemy, spawner.transform.position, new Quaternion(0, 0, 0, 0));
            spawnedEnemy.GetComponentInChildren<EnemyLogic>().enemySpawnType = EnemyLogic.enemySpawnTypes.camping;
            Debug.Log(spawner.transform.parent.parent.Find("FullRoom").gameObject);
            GameObject enemyZone = spawner.transform.parent.parent.Find("EnemyZone").gameObject;
            spawnedEnemy.GetComponentInChildren<EnemyLogic>().homeObject = spawner.transform.GetChild(0).gameObject;
            spawnedEnemy.GetComponentInChildren<EnemyLogic>().detectObject = enemyZone;
            enemyZone.GetComponent<DetectPlayer>().enemyList.Add(spawnedEnemy);
        }
    }
    public static T GetRandomCampFillings<T>()
    {
        System.Array localList = RoomUtils.CampFillings.GetValues(typeof(RoomUtils.CampFillings));
        T returned;
        returned = (T)localList.GetValue(UnityEngine.Random.Range(0, localList.Length));
        return returned;
    }

    public static List<GameObject> SelectRoomPool(List<GameObject> roomList, int roomSelectCount, bool selectAgain = false)
    {
        //roomSelectCount -= 1;
        // �� ���� ������, �� ����� ������ �� 1 ������� ������ ��� ��������, ������� ���� ��������� �������
        List<GameObject> localRoomList = new List<GameObject>(roomList);
        Debug.Log($"������� ����� ����������� - {localRoomList.Count}");
        for (int i = localRoomList.Count - 1; i >= 0; i--)
        {
            if (localRoomList[i].GetComponent<RoomInfo>().roomType != RoomUtils.RoomTypes.unknown)
            {
                Debug.Log($"��� ������� ��������! ���� {i}");
                localRoomList.RemoveAt(i);
            }
        }
        Debug.Log($"������� ������ � ���������, ������� ������� - {localRoomList.Count}");

        List<GameObject> returnedRooms = new List<GameObject>();
        GameObject selectedRoom;
        for (int i = roomSelectCount - 1; i >= 0; i--)
        {
            Debug.Log($"����� �������� ������� ����� ������� ����� {localRoomList.Count}");
            selectedRoom = localRoomList[UnityEngine.Random.Range(0, localRoomList.Count)];
            returnedRooms.Add(selectedRoom);
            if (selectAgain == false)
            {
                localRoomList.Remove(selectedRoom);
            }
        }
        return returnedRooms;
    }

    public static GameObject GetRandomRoom(List<GameObject> roomList)
    {
        return roomList[UnityEngine.Random.Range(0, roomList.Count)];
    }

    public static List<GameObject> GetRandomChestList(List<GameObject> roomList, SRoomInfoGenerator data)
    {
        List<GameObject> localRoomList = new List<GameObject>();
        localRoomList.AddRange(roomList);

        int returnedChestCount = 0;
        return null;
    }

    public static bool FullChestSpawning(List<GameObject> roomList, RoomUtils.ChestTypes chestType, int chestCount, SRoomInfoGenerator data, bool includesCamps = false)
    {

        List<GameObject> localRoomList = new List<GameObject>();
        if (includesCamps == false)
        {
            foreach (GameObject room in roomList)
            {
                if (room.GetComponent<RoomInfo>().roomType == RoomTypes.defaultRoom)
                {
                    localRoomList.Add(room);
                }
            }
        }
        else
        {
            localRoomList.AddRange(roomList);
        }
        GameObject localChest = new GameObject();
        switch (chestType)
        {
            case ChestTypes.classicChest:
                localChest = data.classicChest;
                break;
            case ChestTypes.classicShopCenter:
                localChest = data.classicShopCenter;
                break;
            default:
                localChest = data.classicChest;
                break;
        }
        RoomUtils.RoomTypes roomType;
        for (int i = 0; i < chestCount; i++)
        {
            GameObject localRoom = GetRandomRoom(localRoomList);
            SpawnSingleChest(localRoom, localChest, 1);
        }
        return true;
    }

    public static bool SpawnSingleChest(GameObject room, GameObject chest, int chestCount = 1)
    {
        GameObject targetSpawner = null;
        for (int i = 0; i < chestCount; i++)
        {
            List<GameObject> localSpawnerList = room.GetComponent<RoomInfo>().lootSpawners;
            if (localSpawnerList.Count == 0)
            {
                return false;
            }
            targetSpawner = localSpawnerList[UnityEngine.Random.Range(0, localSpawnerList.Count)];
            if (GameObject.Instantiate(chest, targetSpawner.transform.position, new Quaternion(0, 0, 0, 0)) != null)
            {
                localSpawnerList.Remove(targetSpawner);
                GameObject.Destroy(targetSpawner);
            }
        }
        return true;
    }

    public static int GetRandomCorridorWeigh(int min, int max)
    {
        return UnityEngine.Random.Range(min, max);
    }

    // ����� ��������� ���� �� ������ ������� �������.
    // ������ - ������� ���, ����� ��������� ������� ��������� ����� ��������, �������� ����� ��� ���������� � ��������� ������, � ��� ��������� �������� �������
    // ��� ��� ������ - ����� ��������� ������� ������� ������ � ������� ������� ��������.
    // ��� ����� ������� - ������� �� ��� ������� �������, ������� ���, ����� ������� ����������� ����� �� ������ ������, � �� ������ ��� � ����

    // ��������: ����������� ����� �������� � �������������� ��������� ����� ���������. ���� ����������� �� �� � ������� ������� - ��� ������� �������. ���������
    public static SRoomAfterData FillRoomSide(GameObject room, RoomSide roomSide, SRoomTilesData tilesData, SRoomAfterData roomAfterData = null)
    {
        bool willCloseSide = false;
        SRoomAfterData localRoomAfterInfo = ScriptableObject.CreateInstance<SRoomAfterData>();

        if (roomAfterData != null)
        {
            localRoomAfterInfo = roomAfterData;
        }

        float doorSize = 0;
        RoomInfo currentRoomInfo = room.GetComponent<RoomInfo>();

        Tilemap localTilemap = room.transform.Find("Grid").Find("blocks").GetComponent<Tilemap>();
        Tilemap polTilemap = room.transform.Find("Grid").Find("pol").GetComponentInChildren<Tilemap>();

        Vector2 localPos0 = currentRoomInfo.side0;
        Vector2 localPos1 = currentRoomInfo.side1;
        Vector2 localPos2 = currentRoomInfo.side2;
        Vector2 localPos3 = currentRoomInfo.side3;

        if (localPos0 != new Vector2(0, 0) && localPos1 != new Vector2(0, 0) && localPos2 != new Vector2(0, 0) && localPos3 != new Vector2(0, 0))
        {
            //
        }

        Vector2 topCorridorLeftPoint = currentRoomInfo.topCorridorLeftPoint;
        Vector2 topCorridorRightPoint = currentRoomInfo.topCorridorRightPoint;

        Vector2 rightCorridorTopPoint = currentRoomInfo.rightCorridorTopPoint;
        Vector2 rightCorridorBottomPoint = currentRoomInfo.rightCorridorBotPoint;

        Vector2 bottomCorridorLeftPoint = currentRoomInfo.botCorridorLeftPoint;
        Vector2 bottomCorridorRightPoint = currentRoomInfo.botCorridorRightPoint;

        Vector2 leftCorridorTopPoint = currentRoomInfo.leftCorridorTopPoint;
        Vector2 leftCorridorBotPoint = currentRoomInfo.leftCorridorBotPoint;

        // ������ �� �������� ������
        int blocksCount = 0;
        int randPos = 0;

        // ����, � ������� ��������� ��������, ������� ������ ����� � ������, � ����� � ����� ������� ��������
        switch (roomSide)
        {
            case RoomSide.top:
                if (localRoomAfterInfo.roomDirectionFromTo == RoomUtils.DirectionType.fromTopToBot || localRoomAfterInfo.roomDirectionFromTo == DirectionType.noDirection)
                {
                    blocksCount = localRoomAfterInfo.doorTopSize;
                }
                else
                {
                    int topRange = (int)((Abs(topCorridorRightPoint.x - topCorridorLeftPoint.x) - 1) * localRoomAfterInfo.roomSizeKoef);
                    Debug.Log($"231 {topRange}");
                    topRange = Mathf.Max(localRoomAfterInfo.minimumDoorSizeVert, topRange);
                    blocksCount = Random.Range(localRoomAfterInfo.minimumDoorSizeVert, topRange);
                    // ����� ���� * 2 / 3 ��� � ������� ����
                    //blocksCount = Random.Range(6, (int)(topCorridorRightPoint.x - topCorridorLeftPoint.x - 1) * 2 / 3);
                }
                randPos = Random.Range((int)room.GetComponent<RoomInfo>().topCorridorLeftPoint.x + 1, (int)room.GetComponent<RoomInfo>().topCorridorRightPoint.x - blocksCount);
                break;

            case RoomSide.right:
                if (localRoomAfterInfo.roomDirectionFromTo == RoomUtils.DirectionType.fromRightToLeft)
                {
                    blocksCount = localRoomAfterInfo.doorRightSize;
                }
                else
                {
                    int topRange = (int)((Abs(rightCorridorTopPoint.y - rightCorridorBottomPoint.y) - 1) * localRoomAfterInfo.roomSizeKoef);
                    topRange = Mathf.Max(localRoomAfterInfo.minimumDoorSizeHor, topRange);
                    blocksCount = Random.Range(localRoomAfterInfo.minimumDoorSizeHor, topRange);
                }
                randPos = Random.Range((int)room.GetComponent<RoomInfo>().rightCorridorBotPoint.y + 1, (int)room.GetComponent<RoomInfo>().rightCorridorTopPoint.y - blocksCount);
                break;

            case RoomSide.bot:
                if (localRoomAfterInfo.roomDirectionFromTo == RoomUtils.DirectionType.fromBotToTop)
                {
                    blocksCount = localRoomAfterInfo.doorBotSize;
                }
                else
                {
                    int topRange = (int)((Abs(bottomCorridorRightPoint.x - bottomCorridorLeftPoint.x) - 1) * localRoomAfterInfo.roomSizeKoef);
                    topRange = Mathf.Max(localRoomAfterInfo.minimumDoorSizeVert, topRange);
                    blocksCount = Random.Range(localRoomAfterInfo.minimumDoorSizeVert, topRange);
                }
                randPos = Random.Range((int)room.GetComponent<RoomInfo>().botCorridorLeftPoint.x + 1, (int)room.GetComponent<RoomInfo>().botCorridorRightPoint.x - blocksCount);
                break;

            case RoomSide.left:
                if (localRoomAfterInfo.roomDirectionFromTo == RoomUtils.DirectionType.fromLeftToRight)
                {
                    blocksCount = localRoomAfterInfo.doorLeftSize;
                }
                else
                {
                    int topRange = (int)((Abs(leftCorridorTopPoint.y - leftCorridorBotPoint.y) - 1) * localRoomAfterInfo.roomSizeKoef);
                    topRange = Mathf.Max(localRoomAfterInfo.minimumDoorSizeHor, topRange);
                    blocksCount = Random.Range(localRoomAfterInfo.minimumDoorSizeHor, topRange);
                }
                randPos = Random.Range((int)room.GetComponent<RoomInfo>().leftCorridorBotPoint.y + 1, (int)room.GetComponent<RoomInfo>().leftCorridorTopPoint.y - blocksCount);
                break;


                // ������������ ������� � ���������� ������ - ������ ��� � 1 ������ ������ 2 �����
        }
        //  && (roomSide == RoomSide.top || roomSide == RoomSide.bot)
        if ((randPos % 2 != 0) && (roomSide == RoomSide.right || roomSide == RoomSide.left))
        {
            randPos++;
        }

        if ((randPos % 2 == 0) && (roomSide == RoomSide.top || roomSide == RoomSide.bot))
        {
            randPos++;
        }

        if (blocksCount % 2 != 0)
        {
            blocksCount++;
        }

        if (blocksCount == 0)
        {
            willCloseSide = true;
        }

        // ��������� ���������� � ���, ������� ������ ����� � ���� ������
        switch (roomSide)
        {
            case RoomSide.top:
                localRoomAfterInfo.doorTopSize = blocksCount;
                break;
            case RoomSide.right:
                localRoomAfterInfo.doorRightSize = blocksCount;
                break;
            case RoomSide.bot:
                localRoomAfterInfo.doorBotSize = blocksCount;
                break;
            case RoomSide.left:
                localRoomAfterInfo.doorLeftSize = blocksCount;
                break;
        }

        List<int> exceptedPos = new List<int>();
        /*if (roomSide == RoomSide.left || roomSide == RoomSide.right)
        {
            randPos++;
        }*/

        // ��������� ����������, ����� ����� �������
        // ��������� ������� �������, ����� � �������� ��� �����
        for (int i = 0; i < blocksCount; i++)
        {
            switch (roomSide)
            {
                case RoomSide.top:
                    // ���������� �����
                    // ���������, ��� �� ������� �� ������� ��������
                    if (randPos >= topCorridorLeftPoint.x && randPos <= topCorridorRightPoint.x)
                    {
                        localTilemap.SetTile(new Vector3Int(randPos, (int)topCorridorLeftPoint.y), null);

                        // ��������� ���
                        for (int j = 0; j < 7; j++)
                        {
                            if (j % 2 != 0)
                            {
                                if (i % 2 == 0)
                                {
                                    polTilemap.SetTile(new Vector3Int(randPos, (int)(topCorridorLeftPoint.y - j)), tilesData.classicPol0);
                                }
                                else
                                {
                                    polTilemap.SetTile(new Vector3Int(randPos, (int)(topCorridorLeftPoint.y - j)), tilesData.classicPol1);
                                }
                            }
                            else
                            {
                                if (i % 2 == 0)
                                {
                                    polTilemap.SetTile(new Vector3Int(randPos, (int)(topCorridorLeftPoint.y - j)), tilesData.classicPol3);
                                }
                                else
                                {
                                    polTilemap.SetTile(new Vector3Int(randPos, (int)(topCorridorLeftPoint.y - j)), tilesData.classicPol2);
                                }
                            }
                            // ��� �������� ����������� ���� - �������� ������, ����� ����� 100% ��� ������ �� ����
                            localTilemap.SetTile(new Vector3Int(randPos, (int)(topCorridorLeftPoint.y - j)), null);
                        }
                        if (i == blocksCount / 2 + 1)
                        {
                            GameObject localEndBlock = room.transform.Find("endblock_top").gameObject;

                            // ���� ����� ��������� �������������� ���������� - �� ��� ���� �������� � ����������� y
                            //localEndBlock.transform.localPosition = new Vector2((float) (randPos - 1) / 2f, (int)(topCorridorLeftPoint.y) / 2);
                            localEndBlock.transform.localPosition = new Vector2((float)(randPos - 1) / 2f, localEndBlock.transform.localPosition.y);
                            localRoomAfterInfo.endblock_top = localEndBlock;

                            // ��������� �������, � ������� �������� ���� ����
                            if (localRoomAfterInfo.roomDirectionFromTo == DirectionType.noDirection)
                            {
                                GameObject.Instantiate(tilesData.startGameBlock, new Vector2(localEndBlock.transform.position.x, localEndBlock.transform.position.y + 18), new Quaternion(0, 0, 0, 0)).transform.parent = room.transform;
                                GameObject.Instantiate(tilesData.startGameBlock, new Vector2(localEndBlock.transform.position.x - 1, localEndBlock.transform.position.y + 18), new Quaternion(0, 0, 0, 0)).transform.parent = room.transform;
                                GameObject.Instantiate(tilesData.startGameBlock, new Vector2(localEndBlock.transform.position.x - 2, localEndBlock.transform.position.y + 18), new Quaternion(0, 0, 0, 0)).transform.parent = room.transform;
                                GameObject.Instantiate(tilesData.startGameBlock, new Vector2(localEndBlock.transform.position.x + 2, localEndBlock.transform.position.y + 18), new Quaternion(0, 0, 0, 0)).transform.parent = room.transform;
                                GameObject.Instantiate(tilesData.startGameBlock, new Vector2(localEndBlock.transform.position.x + 1, localEndBlock.transform.position.y + 18), new Quaternion(0, 0, 0, 0)).transform.parent = room.transform;
                            }

                        }
                    }
                    break;
                case RoomSide.right:
                    if (randPos <= rightCorridorTopPoint.y && randPos >= rightCorridorBottomPoint.y)
                    {
                        if (randPos % 2 == 0)
                        {
                            polTilemap.SetTile(new Vector3Int((int)rightCorridorTopPoint.x, randPos), tilesData.classicPol3);
                        }
                        else
                        {
                            polTilemap.SetTile(new Vector3Int((int)rightCorridorTopPoint.x, randPos), tilesData.classicPol0);
                        }

                        localTilemap.SetTile(new Vector3Int((int)rightCorridorTopPoint.x, randPos), null);

                        if (i == ((blocksCount - 6) / 2 + 1))
                        {
                            GameObject localEndBlock = room.transform.Find("endblock_right").gameObject;

                            //localEndBlock.transform.localPosition = new Vector2((int)(rightCorridorTopPoint.x) / 2 + 0.5f, (float)randPos / 2 - 0.5f);
                            localEndBlock.transform.localPosition = new Vector2(localEndBlock.transform.localPosition.x, (float)randPos / 2 - 0.5f);
                            localRoomAfterInfo.endblock_right = localEndBlock;
                        }

                        // ���������� ����
                    }
                    break;
                case RoomSide.bot:
                    if (i % 2 == 0)
                    {
                        polTilemap.SetTile(new Vector3Int(randPos, (int)bottomCorridorRightPoint.y), tilesData.classicPol0);
                    }
                    else
                    {
                        polTilemap.SetTile(new Vector3Int(randPos, (int)bottomCorridorRightPoint.y), tilesData.classicPol1);
                    }

                    localTilemap.SetTile(new Vector3Int(randPos, (int)bottomCorridorRightPoint.y), null);

                    if (i == blocksCount / 2 + 1)
                    {
                        GameObject localEndBlock = room.transform.Find("endblock_bot").gameObject;

                        //localEndBlock.transform.localPosition = new Vector2((float)(randPos - 1) / 2f, (int)(bottomCorridorLeftPoint.y) / 2);
                        localEndBlock.transform.localPosition = new Vector2((float)(randPos - 1) / 2f, localEndBlock.transform.localPosition.y);
                        localRoomAfterInfo.endblock_bot = localEndBlock;
                    }

                    break;
                case RoomSide.left:
                    if ((randPos <= leftCorridorTopPoint.y) && (randPos >= leftCorridorBotPoint.y))
                    {
                        if (randPos % 2 == 0)
                        {
                            polTilemap.SetTile(new Vector3Int((int)leftCorridorBotPoint.x, randPos), tilesData.classicPol2);
                        }
                        else
                        {
                            polTilemap.SetTile(new Vector3Int((int)leftCorridorBotPoint.x, randPos), tilesData.classicPol1);
                        }

                        localTilemap.SetTile(new Vector3Int((int)leftCorridorBotPoint.x, randPos), null);

                        if (i == ((blocksCount - 6) / 2 + 1))
                        {
                            GameObject localEndBlock = room.transform.Find("endblock_left").gameObject;

                            //localEndBlock.transform.localPosition = new Vector2((int)(leftCorridorTopPoint.x) / 2 + 0.5f, (float) randPos / 2 - 0.5f);
                            localEndBlock.transform.localPosition = new Vector2(localEndBlock.transform.localPosition.x, (float)randPos / 2 - 0.5f);
                            localRoomAfterInfo.endblock_left = localEndBlock;
                        }
                    }
                    break;
            }
            exceptedPos.Add(randPos);
            randPos++;
            Debug.Log($"������ ����! {localTilemap}");
        }

        switch (roomSide)
        {
            case RoomSide.right:
                if ((randPos <= rightCorridorTopPoint.y) && (randPos >= rightCorridorBottomPoint.y))
                {
                    polTilemap.SetTile(new Vector3Int((int)rightCorridorTopPoint.x, randPos - 1), tilesData.perPolStartFirst0);
                    polTilemap.SetTile(new Vector3Int((int)rightCorridorTopPoint.x, randPos - 2), tilesData.perPolStartFirst1);
                    polTilemap.SetTile(new Vector3Int((int)rightCorridorTopPoint.x, randPos - 3), tilesData.perPolStartFirst2);
                    polTilemap.SetTile(new Vector3Int((int)rightCorridorTopPoint.x, randPos - 4), tilesData.perPolStartFirst3);
                    polTilemap.SetTile(new Vector3Int((int)rightCorridorTopPoint.x, randPos - 5), tilesData.perPolStartFirst4);
                    polTilemap.SetTile(new Vector3Int((int)rightCorridorTopPoint.x, randPos - 6), tilesData.perPolStartFirst5);

                    // ������� �����, ����� �� ��������� ����� ���� ������
                    localTilemap.SetTile(new Vector3Int((int)rightCorridorTopPoint.x, randPos - 1), null);
                    localTilemap.SetTile(new Vector3Int((int)rightCorridorTopPoint.x, randPos - 2), null);
                    localTilemap.SetTile(new Vector3Int((int)rightCorridorTopPoint.x, randPos - 3), null);
                    localTilemap.SetTile(new Vector3Int((int)rightCorridorTopPoint.x, randPos - 4), null);
                    localTilemap.SetTile(new Vector3Int((int)rightCorridorTopPoint.x, randPos - 5), null);
                    localTilemap.SetTile(new Vector3Int((int)rightCorridorTopPoint.x, randPos - 6), null);
                }
                break;
            case RoomSide.left:
                if ((randPos - 6 <= leftCorridorTopPoint.y) && (randPos - 6 >= leftCorridorBotPoint.y))
                {
                    polTilemap.SetTile(new Vector3Int((int)leftCorridorTopPoint.x, randPos - 1), tilesData.perPolStartSecond0);
                    polTilemap.SetTile(new Vector3Int((int)leftCorridorTopPoint.x, randPos - 2), tilesData.perPolStartSecond1);
                    polTilemap.SetTile(new Vector3Int((int)leftCorridorTopPoint.x, randPos - 3), tilesData.perPolStartSecond2);
                    polTilemap.SetTile(new Vector3Int((int)leftCorridorTopPoint.x, randPos - 4), tilesData.perPolStartSecond3);
                    polTilemap.SetTile(new Vector3Int((int)leftCorridorTopPoint.x, randPos - 5), tilesData.perPolStartSecond4);
                    polTilemap.SetTile(new Vector3Int((int)leftCorridorTopPoint.x, randPos - 6), tilesData.perPolStartSecond5);

                    // ������� �����, ����� �� ��������� ����� ���� ������
                    localTilemap.SetTile(new Vector3Int((int)leftCorridorTopPoint.x, randPos - 1), null);
                    localTilemap.SetTile(new Vector3Int((int)leftCorridorTopPoint.x, randPos - 2), null);
                    localTilemap.SetTile(new Vector3Int((int)leftCorridorTopPoint.x, randPos - 3), null);
                    localTilemap.SetTile(new Vector3Int((int)leftCorridorTopPoint.x, randPos - 4), null);
                    localTilemap.SetTile(new Vector3Int((int)leftCorridorTopPoint.x, randPos - 5), null);
                    localTilemap.SetTile(new Vector3Int((int)leftCorridorTopPoint.x, randPos - 6), null);

                }
                break;
        }

        List<GameObject> allSpawners = TechUtils.GetAllChilds(room.transform.Find("RoomSpawners").gameObject);

        switch (roomSide)
        {
            case RoomSide.top:
                // ���������� ������� ������ ����, ��� ��� ��������
                for (int x = (int)topCorridorLeftPoint.x + 1; x < topCorridorRightPoint.x; x++)
                {
                    if (!exceptedPos.Contains(x) || willCloseSide)
                    {
                        if (x % 2 != 0)
                        {
                            localTilemap.SetTile(new Vector3Int(x, (int)topCorridorLeftPoint.y), tilesData.topWallFirst);

                            polTilemap.SetTile(new Vector3Int(x, (int)topCorridorLeftPoint.y - 1), tilesData.perPolMidFirst0);
                            polTilemap.SetTile(new Vector3Int(x, (int)topCorridorLeftPoint.y - 2), tilesData.perPolMidFirst1);
                            polTilemap.SetTile(new Vector3Int(x, (int)topCorridorLeftPoint.y - 3), tilesData.perPolMidFirst2);
                            polTilemap.SetTile(new Vector3Int(x, (int)topCorridorLeftPoint.y - 4), tilesData.perPolMidFirst3);
                            polTilemap.SetTile(new Vector3Int(x, (int)topCorridorLeftPoint.y - 5), tilesData.perPolMidFirst4);
                            polTilemap.SetTile(new Vector3Int(x, (int)topCorridorLeftPoint.y - 6), tilesData.perPolMidFirst5);
                        }
                        else
                        {
                            localTilemap.SetTile(new Vector3Int(x, (int)topCorridorLeftPoint.y), tilesData.topWallSecond);

                            polTilemap.SetTile(new Vector3Int(x, (int)topCorridorLeftPoint.y - 1), tilesData.perPolMidSecond0);
                            polTilemap.SetTile(new Vector3Int(x, (int)topCorridorLeftPoint.y - 2), tilesData.perPolMidSecond1);
                            polTilemap.SetTile(new Vector3Int(x, (int)topCorridorLeftPoint.y - 3), tilesData.perPolMidSecond2);
                            polTilemap.SetTile(new Vector3Int(x, (int)topCorridorLeftPoint.y - 4), tilesData.perPolMidSecond3);
                            polTilemap.SetTile(new Vector3Int(x, (int)topCorridorLeftPoint.y - 5), tilesData.perPolMidSecond4);
                            polTilemap.SetTile(new Vector3Int(x, (int)topCorridorLeftPoint.y - 6), tilesData.perPolMidSecond5);
                        }
                    }
                    else // �������� �� ����������. �������� ������� ��, � ������� ������ ������� ��-�� ���������� ������
                    {
                        foreach (GameObject spawner in allSpawners)
                        {
                            if ((Mathf.Abs(spawner.transform.localPosition.x * 2 - x) <= 2) && (Mathf.Abs(topCorridorLeftPoint.y - spawner.transform.localPosition.y * 2) <= 6))
                            {
                                spawner.SetActive(false);
                            }
                        }
                    }
                }

                // ���� ������� �� ������� ������� � ��� ����� ������ ������ - ���������� ��� ����������� ��������
                // �������� ������ �� ���������� ���� �������� ������ �� ������� �������. � ������� ����� ��������� � ������� �� ������ ��������
                if (blocksCount == 0)
                {
                    allSpawners = TechUtils.GetAllChilds(room.transform.Find("RoomSpawners").gameObject, false, false);
                    foreach (GameObject spawner in allSpawners)
                    {
                        if ((spawner != null) && (Mathf.Abs(topCorridorLeftPoint.y - spawner.transform.localPosition.y * 2) <= 8) &&
                            (Abs(topCorridorLeftPoint.x - spawner.transform.localPosition.x * 2) >= 6) && (Abs(topCorridorRightPoint.x - spawner.transform.localPosition.x * 2) >= 6))
                        {
                            spawner.SetActive(true);
                        }
                    }
                }
                break;

            case RoomSide.right:
                for (int y = (int)rightCorridorBottomPoint.y + 1; y < rightCorridorTopPoint.y; y++)
                {
                    if (!exceptedPos.Contains(y) || willCloseSide)
                    {
                        if (y % 2 != 0)
                        {
                            localTilemap.SetTile(new Vector3Int((int)rightCorridorTopPoint.x, y), tilesData.rightWallFirst);
                        }
                        else
                        {
                            localTilemap.SetTile(new Vector3Int((int)rightCorridorTopPoint.x, y), tilesData.rightWallSecond);
                        }
                    }
                    else // �������� �� ����������. �������� ������� ��, � ������� ������ ������� ��-�� ���������� ������
                    {
                        foreach (GameObject spawner in allSpawners)
                        {
                            if (((Mathf.Abs(spawner.transform.localPosition.y * 2 - y) <= 3)) && (Mathf.Abs(rightCorridorTopPoint.x - spawner.transform.localPosition.x * 2) <= 6))
                            {
                                spawner.SetActive(false);
                            }
                        }
                    }
                }

                if (blocksCount == 0)
                {
                    allSpawners = TechUtils.GetAllChilds(room.transform.Find("RoomSpawners").gameObject, false, false);
                    foreach (GameObject spawner in allSpawners)
                    {
                        if ((spawner != null) && (Mathf.Abs(rightCorridorTopPoint.x - spawner.transform.localPosition.x * 2) <= 4) &&
                            (Abs(rightCorridorTopPoint.y - spawner.transform.localPosition.y * 2) >= 4) && (Abs(rightCorridorBottomPoint.x - spawner.transform.localPosition.y * 2) >= 4))
                        {
                            spawner.SetActive(true);
                        }
                    }
                }
                break;

            case RoomSide.bot:
                for (int x = (int)bottomCorridorLeftPoint.x + 1; x < bottomCorridorRightPoint.x; x++)
                {
                    if (!exceptedPos.Contains(x) || willCloseSide)
                    {
                        if (x % 2 != 0)
                        {
                            localTilemap.SetTile(new Vector3Int(x, (int)bottomCorridorRightPoint.y), tilesData.bottomWallFirst);
                        }
                        else
                        {
                            localTilemap.SetTile(new Vector3Int(x, (int)bottomCorridorRightPoint.y), tilesData.bottomWallSecond);
                        }
                    }

                    else // �������� �� ����������. �������� ������� ��, � ������� ������ ������� ��-�� ���������� ������
                    {
                        foreach (GameObject spawner in allSpawners)
                        {
                            if ((Mathf.Abs(spawner.transform.localPosition.x * 2 - x) <= 3) && (Mathf.Abs(bottomCorridorLeftPoint.y - spawner.transform.localPosition.y * 2) <= 4))
                            {
                                spawner.SetActive(false);
                                //spawner.GetComponentInChildren<SpriteRenderer>().color = Color.red;
                            }
                        }
                    }

                }

                if (blocksCount == 0)
                {
                    allSpawners = TechUtils.GetAllChilds(room.transform.Find("RoomSpawners").gameObject, false, false);
                    foreach (GameObject spawner in allSpawners)
                    {
                        if ((spawner != null) && (Mathf.Abs(bottomCorridorLeftPoint.y - spawner.transform.localPosition.y * 2) <= 8) &&
                            (Abs(bottomCorridorLeftPoint.x - spawner.transform.localPosition.x * 2) >= 6) && (Abs(bottomCorridorRightPoint.x - spawner.transform.localPosition.x * 2) >= 6))
                        {
                            spawner.SetActive(true);
                        }
                    }
                }
                break;

            case RoomSide.left:
                for (int y = (int)leftCorridorBotPoint.y + 1; y < leftCorridorTopPoint.y; y++)
                {
                    if (!exceptedPos.Contains(y) || willCloseSide)
                    {
                        if (y % 2 != 0)
                        {
                            localTilemap.SetTile(new Vector3Int((int)leftCorridorBotPoint.x, y), tilesData.leftWallFirst);
                        }
                        else
                        {
                            localTilemap.SetTile(new Vector3Int((int)leftCorridorBotPoint.x, y), tilesData.leftWallSecond);
                        }
                    }

                    else // �������� �� ����������. �������� ������� ��, � ������� ������ ������� ��-�� ���������� ������
                    {
                        foreach (GameObject spawner in allSpawners)
                        {
                            if (((Mathf.Abs(spawner.transform.localPosition.y * 2 - y) <= 3)) && (Mathf.Abs(leftCorridorTopPoint.x - spawner.transform.localPosition.x * 2) <= 6))
                            {
                                spawner.SetActive(false);

                            }
                        }
                    }
                }

                if (blocksCount == 0)
                {
                    allSpawners = TechUtils.GetAllChilds(room.transform.Find("RoomSpawners").gameObject, false, false);
                    foreach (GameObject spawner in allSpawners)
                    {
                        if ((spawner != null) && (Mathf.Abs(leftCorridorTopPoint.x - spawner.transform.localPosition.x * 2) <= 4) &&
                            (Abs(leftCorridorTopPoint.y - spawner.transform.localPosition.y * 2) >= 4) && (Abs(leftCorridorBotPoint.x - spawner.transform.localPosition.y * 2) >= 4))
                        {
                            spawner.SetActive(true);
                            //spawner.GetComponentInChildren<SpriteRenderer>().color = Color.green;
                        }
                    }
                }
                break;
        }

        if (willCloseSide)
        {
            switch (roomSide)
            {
                case RoomSide.top:
                    break;
            }
        }

        // ��������� �������� - ������ ��������� � �����
        for (int x = (int)bottomCorridorLeftPoint.x; x <= (int)bottomCorridorRightPoint.x; x++)
        {
            for (int y = (int)bottomCorridorLeftPoint.y - 1; y >= (int)bottomCorridorLeftPoint.y - 6; y--)
            {
                polTilemap.SetTile(new Vector3Int(x, y), null);
            }
        }
        //polTilemap.SetTile(new Vector3Int((int)leftCorridorBotPoint.x, (int)leftCorridorBotPoint.y), null);
        polTilemap.SetTile(new Vector3Int((int)leftCorridorBotPoint.x, (int)leftCorridorBotPoint.y - 1), null);
        polTilemap.SetTile(new Vector3Int((int)leftCorridorBotPoint.x, (int)leftCorridorBotPoint.y - 2), null);
        polTilemap.SetTile(new Vector3Int((int)leftCorridorBotPoint.x, (int)leftCorridorBotPoint.y - 3), null);
        polTilemap.SetTile(new Vector3Int((int)leftCorridorBotPoint.x, (int)leftCorridorBotPoint.y - 4), null);
        polTilemap.SetTile(new Vector3Int((int)leftCorridorBotPoint.x, (int)leftCorridorBotPoint.y - 5), null);

        polTilemap.SetTile(new Vector3Int((int)rightCorridorBottomPoint.x, (int)rightCorridorBottomPoint.y - 1), null);
        polTilemap.SetTile(new Vector3Int((int)rightCorridorBottomPoint.x, (int)rightCorridorBottomPoint.y - 2), null);
        polTilemap.SetTile(new Vector3Int((int)rightCorridorBottomPoint.x, (int)rightCorridorBottomPoint.y - 3), null);
        polTilemap.SetTile(new Vector3Int((int)rightCorridorBottomPoint.x, (int)rightCorridorBottomPoint.y - 4), null);
        polTilemap.SetTile(new Vector3Int((int)rightCorridorBottomPoint.x, (int)rightCorridorBottomPoint.y - 5), null);

        return localRoomAfterInfo;
    }

    public static SRoomAfterData FillFullRoom(GameObject room, SRoomTilesData tilesData, DirectionType directionFromTo, SRoomAfterData roomAfterData = null)
    {
        SRoomAfterData localRoomAfterInfo = ScriptableObject.CreateInstance<SRoomAfterData>();
        if (roomAfterData != null)
        {
            localRoomAfterInfo = roomAfterData;
        }

        localRoomAfterInfo.roomDirectionFromTo = directionFromTo;

        // ������ ��� ������ �������, ��� ������ ��������
        if (directionFromTo == DirectionType.noDirection)
        {
            localRoomAfterInfo = FillRoomSide(room, RoomSide.top, tilesData, localRoomAfterInfo);
            localRoomAfterInfo.readyToBuildRoomsHere[0] = true;
            return localRoomAfterInfo;
        }

        float randNumber = Random.Range(0f, 1f);
        if ((randNumber <= localRoomAfterInfo.roomSpawnChance || directionFromTo == DirectionType.fromTopToBot) && (localRoomAfterInfo.roomHasCorridorTop))
        {
            localRoomAfterInfo = FillRoomSide(room, RoomSide.top, tilesData, localRoomAfterInfo);
            if (directionFromTo != DirectionType.fromTopToBot)
            {
                localRoomAfterInfo.readyToBuildRoomsHere[0] = true;
            }
        }

        randNumber = Random.Range(0f, 1f);
        if ((randNumber <= localRoomAfterInfo.roomSpawnChance || directionFromTo == DirectionType.fromRightToLeft) && (localRoomAfterInfo.roomHasCorridorRight))
        {
            localRoomAfterInfo = FillRoomSide(room, RoomSide.right, tilesData, localRoomAfterInfo);
            if (directionFromTo != DirectionType.fromRightToLeft)
            {
                localRoomAfterInfo.readyToBuildRoomsHere[1] = true;
            }
        }

        randNumber = Random.Range(0f, 1f);
        if ((randNumber <= localRoomAfterInfo.roomSpawnChance || directionFromTo == DirectionType.fromBotToTop) && (localRoomAfterInfo.roomHasCorridorBot))
        {
            localRoomAfterInfo = FillRoomSide(room, RoomSide.bot, tilesData, localRoomAfterInfo);
            if (directionFromTo != DirectionType.fromBotToTop)
            {
                localRoomAfterInfo.readyToBuildRoomsHere[2] = true;
            }
        }

        randNumber = Random.Range(0f, 1f);
        if ((randNumber <= localRoomAfterInfo.roomSpawnChance || directionFromTo == DirectionType.fromLeftToRight) && (localRoomAfterInfo.roomHasCorridorLeft))
        {
            localRoomAfterInfo = FillRoomSide(room, RoomSide.left, tilesData, localRoomAfterInfo);
            if (directionFromTo != DirectionType.fromLeftToRight)
            {
                localRoomAfterInfo.readyToBuildRoomsHere[3] = true;
            }
        }

        return localRoomAfterInfo;
    }
    public static SCorridorAfterData CreateCorridor(GameObject corridor, DirectionType corridorType, int doorSize, int minCorridorSize, int maxCorridorSize, SRoomTilesData tilesData, SCorridorAfterData corridorAfterData = null, SRoomAfterData predRoomAfterData = null)
    {
        SCorridorAfterData localCorridorAfterInfo = ScriptableObject.CreateInstance<SCorridorAfterData>();

        if (corridorAfterData != null)
        {
            localCorridorAfterInfo = corridorAfterData;
        }

        Tilemap blocksTilemap = corridor.transform.Find("Grid").Find("blocks").GetComponent<Tilemap>();
        Tilemap polTilemap = corridor.transform.Find("Grid").Find("pol").GetComponentInChildren<Tilemap>();
        int corridorSize = Random.Range(minCorridorSize, maxCorridorSize + 1);
        if (corridorSize % 2 == 1)
        {
            corridorSize++;
        }

        // ������ ���, ����� ������ ������ ���� ������ ���-�� (����� � ��� �����, �� ����� ���� ��������)
        // ���� ������ ����� - ����� ������������ �� ������ ������
        /*if ((corridorSize / 2) % 2 == 0)
        {
            corridorSize += 2;
        }*/

        Vector2 pos0, pos1, pos2, pos3, endBlockBotPos, endBlockTopPos, endBlockRightPos, endBlockLeftPos;

        switch (corridorType)
        {
            case DirectionType.fromBotToTop:
            case DirectionType.fromTopToBot:

                // ���� ������ ����� ����� � ����������� ��������� fullRoom
                pos0 = new Vector2(-1, corridorSize + 1);
                pos1 = new Vector2(doorSize + 3, corridorSize + 1);
                pos2 = new Vector2(doorSize + 3, -1);
                pos3 = new Vector2(-1, -1);

                endBlockBotPos = new Vector2((float)doorSize / 4f + 0.5f, 0.5f);
                endBlockTopPos = new Vector2((float)doorSize / 4f + 0.5f, corridorSize / 2 - 0.5f);

                // �������� �������, � ������� �������� ����� ����� ����� ����������� ��������
                //corridor.transform.Find("Grid").Find("limit_block_3").transform.localPosition = new Vector2(1, -1);
                //corridor.transform.Find("Grid").Find("limit_block_2").transform.localPosition = new Vector2(doorSize / 2 + 0.75f, -1);

                corridor.transform.Find("limit_block_0").transform.localPosition = new Vector2(-0.25f, corridorSize / 2 - 0.75f);
                corridor.transform.Find("limit_block_1").transform.localPosition = new Vector2(doorSize / 2 + 1.25f, corridorSize / 2 - 0.75f);
                corridor.transform.Find("limit_block_2").transform.localPosition = new Vector2(doorSize / 2 + 1.25f, 0.75f);
                corridor.transform.Find("limit_block_3").transform.localPosition = new Vector2(-0.25f, 0.75f);

                corridor.transform.Find("FullRoom").transform.localScale = new Vector2(doorSize / 2 + 0.5f, corridorSize / 2 - 1.25f);

                // ������������ FullRoom
                if ((doorSize / 2) % 2 == 0)
                {
                    if ((corridorSize / 2) % 2 == 1)
                    {
                        corridor.transform.Find("FullRoom").transform.localPosition = new Vector2((doorSize + 2) / 4 + 0.5f, corridorSize / 4 + 0.5f);
                    }
                    else
                    {
                        corridor.transform.Find("FullRoom").transform.localPosition = new Vector2((doorSize + 2) / 4 + 0.5f, corridorSize / 4);
                    }
                }
                else
                {
                    if ((corridorSize / 2) % 2 == 1)
                    {
                        corridor.transform.Find("FullRoom").transform.localPosition = new Vector2((doorSize + 2) / 4, corridorSize / 4 + 0.5f);
                    }
                    else
                    {
                        corridor.transform.Find("FullRoom").transform.localPosition = new Vector2((doorSize + 2) / 4, corridorSize / 4);
                    }
                }

                for (int y = 0; y < corridorSize; y++)
                {
                    if (y % 2 == 0)
                    {
                        blocksTilemap.SetTile(new Vector3Int(0, y), tilesData.leftWallFirst);
                    }
                    else
                    {
                        blocksTilemap.SetTile(new Vector3Int(0, y), tilesData.leftWallSecond);
                    }
                    for (int x = 1; x <= doorSize; x++)
                    {
                        if ((x % 2 == 1) && (y % 2 == 0))
                        {
                            polTilemap.SetTile(new Vector3Int(x, y), tilesData.classicPol0);
                        }
                        else if ((x % 2 == 0) && (y % 2 == 0))
                        {
                            polTilemap.SetTile(new Vector3Int(x, y), tilesData.classicPol1);
                        }
                        else if ((x % 2 == 1) && (y % 2 == 1))
                        {
                            polTilemap.SetTile(new Vector3Int(x, y), tilesData.classicPol3);
                        }
                        else if ((x % 2 == 0) && (y % 2 == 1))
                        {
                            polTilemap.SetTile(new Vector3Int(x, y), tilesData.classicPol2);
                        }
                    }
                    if (y % 2 == 0)
                    {
                        blocksTilemap.SetTile(new Vector3Int(doorSize + 1, y), tilesData.rightWallFirst);
                    }
                    else
                    {
                        blocksTilemap.SetTile(new Vector3Int(doorSize + 1, y), tilesData.rightWallSecond);
                    }
                }
                //GameObject.Find("endblock_bot").transform.position = endBlockBotPos;
                //GameObject.Find("endblock_top").transform.position = endBlockTopPos;

                corridor.transform.Find("endblock_bot").transform.localPosition = endBlockBotPos;
                corridor.transform.Find("endblock_top").transform.localPosition = endBlockTopPos;

                Debug.Log($"���� {localCorridorAfterInfo.endblock_bot} {corridor.transform.Find("endblock_bot").gameObject}");

                localCorridorAfterInfo.endblock_bot = corridor.transform.Find("endblock_bot").gameObject;
                localCorridorAfterInfo.endblock_top = corridor.transform.Find("endblock_top").gameObject;

                break;

            case DirectionType.fromLeftToRight:
            case DirectionType.fromRightToLeft:

                // ���� ������ ����� ����� � ����������� ��������� fullRoom
                pos0 = new Vector2(-1, corridorSize + 1);
                pos1 = new Vector2(doorSize + 3, corridorSize + 1);
                pos2 = new Vector2(doorSize + 3, -1);
                pos3 = new Vector2(-1, -1);

                endBlockRightPos = new Vector2(corridorSize / 2 - 0.5f, (float)doorSize / 4f - 0.5f);
                endBlockLeftPos = new Vector2(0.5f, (float)doorSize / 4f - 0.5f);

                corridor.transform.Find("limit_block_0").transform.localPosition = new Vector2(0.75f, doorSize / 2 + 1.25f);
                corridor.transform.Find("limit_block_1").transform.localPosition = new Vector2(corridorSize / 2 - 0.75f, doorSize / 2 + 1.25f);
                corridor.transform.Find("limit_block_2").transform.localPosition = new Vector2(corridorSize / 2 - 0.75f, -0.25f);
                corridor.transform.Find("limit_block_3").transform.localPosition = new Vector2(0.75f, -0.25f);

                corridor.transform.Find("FullRoom").transform.localScale = new Vector2(corridorSize / 2 - 1.25f, doorSize / 2 + 0.75f);
                corridor.transform.Find("FullRoom").transform.localPosition = new Vector2((corridorSize) / 4 + 0.5f, doorSize / 4 + 0.5f);

                // ������������ FullRoom
                // ����� ��������� - ��� 2 ��� ������, ����� ��� �������� ������-����
                if ((doorSize / 2) % 2 == 0)
                {
                    if ((corridorSize / 2) % 2 == 1)
                    {
                        corridor.transform.Find("FullRoom").transform.localPosition = new Vector2((corridorSize) / 4 + 0.5f, doorSize / 4 + 0.5f);
                    }
                    else
                    {
                        corridor.transform.Find("FullRoom").transform.localPosition = new Vector2((corridorSize) / 4, doorSize / 4 + 0.5f);
                    }
                }
                else
                {
                    if ((corridorSize / 2) % 2 == 1)
                    {
                        corridor.transform.Find("FullRoom").transform.localPosition = new Vector2((corridorSize) / 4 + 0.5f, doorSize / 4 + 1f);
                    }
                    else
                    {
                        corridor.transform.Find("FullRoom").transform.localPosition = new Vector2((corridorSize) / 4, doorSize / 4 + 1f);
                    }
                }

                for (int x = 0; x < corridorSize; x++)
                {
                    if (x % 2 == 1)
                    {
                        blocksTilemap.SetTile(new Vector3Int(x, 0), tilesData.topWallFirst);
                    }
                    else
                    {
                        blocksTilemap.SetTile(new Vector3Int(x, 0), tilesData.topWallSecond);
                    }
                    for (int y = 1; y <= doorSize; y++)
                    {
                        if ((x % 2 == 1) && (y % 2 == 0))
                        {
                            polTilemap.SetTile(new Vector3Int(x, y), tilesData.classicPol0);
                        }
                        else if ((x % 2 == 0) && (y % 2 == 0))
                        {
                            polTilemap.SetTile(new Vector3Int(x, y), tilesData.classicPol1);
                        }
                        else if ((x % 2 == 1) && (y % 2 == 1))
                        {
                            polTilemap.SetTile(new Vector3Int(x, y), tilesData.classicPol3);
                        }
                        else if ((x % 2 == 0) && (y % 2 == 1))
                        {
                            polTilemap.SetTile(new Vector3Int(x, y), tilesData.classicPol2);
                        }
                    }
                    if (x % 2 == 1)
                    {
                        blocksTilemap.SetTile(new Vector3Int(x, doorSize + 1), tilesData.bottomWallFirst);

                        polTilemap.SetTile(new Vector3Int(x, doorSize), tilesData.perPolStartFirst0);
                        polTilemap.SetTile(new Vector3Int(x, doorSize - 1), tilesData.perPolMidFirst1);
                        polTilemap.SetTile(new Vector3Int(x, doorSize - 2), tilesData.perPolMidFirst2);
                        polTilemap.SetTile(new Vector3Int(x, doorSize - 3), tilesData.perPolMidFirst3);
                        polTilemap.SetTile(new Vector3Int(x, doorSize - 4), tilesData.perPolMidFirst4);
                        polTilemap.SetTile(new Vector3Int(x, doorSize - 5), tilesData.perPolMidFirst5);
                    }
                    else
                    {
                        blocksTilemap.SetTile(new Vector3Int(x, doorSize + 1), tilesData.bottomWallSecond);

                        polTilemap.SetTile(new Vector3Int(x, doorSize), tilesData.perPolStartSecond0);
                        polTilemap.SetTile(new Vector3Int(x, doorSize - 1), tilesData.perPolMidSecond1);
                        polTilemap.SetTile(new Vector3Int(x, doorSize - 2), tilesData.perPolMidSecond2);
                        polTilemap.SetTile(new Vector3Int(x, doorSize - 3), tilesData.perPolMidSecond3);
                        polTilemap.SetTile(new Vector3Int(x, doorSize - 4), tilesData.perPolMidSecond4);
                        polTilemap.SetTile(new Vector3Int(x, doorSize - 5), tilesData.perPolMidSecond5);
                    }
                }
                //GameObject.Find("endblock_bot").transform.position = endBlockBotPos;
                //GameObject.Find("endblock_top").transform.position = endBlockTopPos;

                corridor.transform.Find("endblock_right").transform.localPosition = endBlockRightPos;
                corridor.transform.Find("endblock_left").transform.localPosition = endBlockLeftPos;

                localCorridorAfterInfo.endblock_right = corridor.transform.Find("endblock_right").gameObject;
                localCorridorAfterInfo.endblock_left = corridor.transform.Find("endblock_left").gameObject;


                break;
        }

        switch (corridorType)
        {
            case DirectionType.fromBotToTop:
                break;
        }

        return localCorridorAfterInfo;
    }

    public static GameObject SelectOneRoomUltimate(RoomSide roomHasCorridorSide, List<GameObject> allAvailableRooms, int doorSize = -1)
    {
        RoomGeneratorNew localRoomGenerator = RoomGeneratorNew.Instance;
        List<GameObject> availableCommonRooms = new List<GameObject>();
        List<GameObject> availableRareRooms = new List<GameObject>();
        List<GameObject> availableMythicRooms = new List<GameObject>();
        List<GameObject> availableLegendaryRooms = new List<GameObject>();

        foreach (GameObject room in allAvailableRooms)
        {
            RoomInfo currentRoomInfo = room.GetComponent<RoomInfo>();
            int roomHasDoorSize = 0;
            if (doorSize != -1)
            {
                switch (roomHasCorridorSide)
                {
                    case RoomSide.top:
                        roomHasDoorSize = Mathf.Abs((int)currentRoomInfo.topCorridorRightPoint.x - (int)currentRoomInfo.topCorridorLeftPoint.x - 1);
                        break;
                    case RoomSide.right:
                        roomHasDoorSize = Mathf.Abs((int)currentRoomInfo.rightCorridorTopPoint.y - (int)currentRoomInfo.rightCorridorBotPoint.y - 1);
                        break;
                    case RoomSide.bot:
                        roomHasDoorSize = Mathf.Abs((int)currentRoomInfo.botCorridorRightPoint.x - (int)currentRoomInfo.botCorridorLeftPoint.x - 1);
                        break;
                    case RoomSide.left:
                        roomHasDoorSize = Mathf.Abs((int)currentRoomInfo.leftCorridorTopPoint.y - (int)currentRoomInfo.leftCorridorBotPoint.y - 1);
                        break;
                }
            }
            string localEndBlockName = $"endblock_{roomHasCorridorSide}";
            if ((room.transform.Find(localEndBlockName) != null) && ((doorSize == -1) || (roomHasDoorSize >= doorSize)))
            {
                switch (room.GetComponent<RoomInfo>().roomRare)
                {
                    case Utils.RareTypes.legendary:
                        availableLegendaryRooms.Add(room);
                        break;
                    case Utils.RareTypes.mythic:
                        availableMythicRooms.Add(room);
                        break;
                    case Utils.RareTypes.rare:
                        availableRareRooms.Add(room);
                        break;
                    case Utils.RareTypes.common:
                        availableCommonRooms.Add(room);
                        break;
                }
            }
        }

        Utils.RareTypes localRoomType = Utils.GetRandomRareType(localRoomGenerator.rareRoomChance, localRoomGenerator.mythicRoomChance, localRoomGenerator.legendaryRoomChance);

        switch (localRoomType)
        {
            case Utils.RareTypes.legendary:
                if (availableLegendaryRooms.Count > 0)
                {
                    return availableLegendaryRooms[Random.Range(0, availableLegendaryRooms.Count)];
                }
                break;
            case Utils.RareTypes.mythic:
                if (availableMythicRooms.Count > 0)
                {
                    return availableMythicRooms[Random.Range(0, availableMythicRooms.Count)];
                }
                break;
            case Utils.RareTypes.rare:
                if (availableRareRooms.Count > 0)
                {
                    return availableRareRooms[Random.Range(0, availableRareRooms.Count)];
                }
                break;
            case Utils.RareTypes.common:
                return availableCommonRooms[Random.Range(0, availableCommonRooms.Count)];
            default:
                return availableCommonRooms[Random.Range(0, availableCommonRooms.Count)];
        }
        return availableCommonRooms[Random.Range(0, availableCommonRooms.Count)];
    }

    // ����� ���������, �������� �� ������� ������� (��� � ������ ������� ����� � ������������
    public static bool CheckSideRoom(GameObject room)
    {
        List<GameObject> endblocksList = new List<GameObject>();
        List<RaycastHit2D> rayCastsList = new List<RaycastHit2D>();

        Transform endblock_top = room.transform.Find("endblock_top");
        Transform endblock_right = room.transform.Find("endblock_right");
        Transform endblock_bot = room.transform.Find("endblock_bot");
        Transform endblock_left = room.transform.Find("endblock_left");

        bool[] roomHasCorridors = new bool[4];

        if (endblock_top != null)
        {
            endblocksList.Add(endblock_top.gameObject);
            RaycastHit2D localRayHit = Physics2D.Raycast(endblock_top.transform.position, Vector2.up);

            if (localRayHit.distance <= 1 && localRayHit.distance != 0)
            {
                roomHasCorridors[0] = false;
            }
            else
            {
                roomHasCorridors[0] = true;
            }

            if (localRayHit.collider != null)
                Debug.Log($"Hit {localRayHit.collider.name}, distance = {localRayHit.distance}");
            else
                Debug.Log("������ �� �����");
        }
        else
        {
            Debug.Log($"������� �� ����� �������� ������! 400");
        }

        if (endblock_right != null)
        {
            endblocksList.Add(endblock_right.gameObject);
            RaycastHit2D localRayHit = Physics2D.Raycast(endblock_right.transform.position, Vector2.right);

            if (localRayHit.distance <= 1)
            {
                roomHasCorridors[1] = false;
            }
            else
            {
                roomHasCorridors[1] = true;
            }
        }

        if (endblock_bot != null)
        {
            endblocksList.Add(endblock_bot.gameObject);
            RaycastHit2D localRayHit = Physics2D.Raycast(endblock_bot.transform.position, Vector2.down);

            if (localRayHit.distance <= 1)
            {
                roomHasCorridors[2] = false;
            }
            else
            {
                roomHasCorridors[2] = true;
            }
        }

        if (endblock_left != null)
        {
            endblocksList.Add(endblock_left.gameObject);
            RaycastHit2D localRayHit = Physics2D.Raycast(endblock_left.transform.position, Vector2.left);

            if (localRayHit.distance <= 1)
            {
                roomHasCorridors[3] = false;
            }
            else
            {
                roomHasCorridors[3] = true;
            }
        }

        int trueCount = 0;
        for (int i = 0; i < roomHasCorridors.Length; i++)
        {
            if (roomHasCorridors[i] == true)
            {
                trueCount++;
            }
        }

        if (trueCount > 1)
        {
            Debug.Log($"������ {trueCount}");
            return false;
        }
        Debug.Log($"������ {trueCount}");
        return true;


    }
}
