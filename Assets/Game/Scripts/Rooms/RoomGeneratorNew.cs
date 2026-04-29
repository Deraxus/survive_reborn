using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Pathfinding;
public class RoomGeneratorNew : MonoBehaviour
{
    // Здесь все тайлы для построения комнат
    public SRoomTilesData roomTilesData;

    public bool mainStartRoom = true;
    public SRoomInfoGenerator data;
    public bool drivenByData = true;

    [Tooltip("Начальная комната - всегда фиксированная, с одним выходом")]
    public GameObject startRoom; // Начальная комната - всегда фиксированная и с одним выходом

    [Header("Варианты комнат")]
    [Tooltip("Список доступных для спауна обычных комнат")]
    public List<GameObject> publicRooms = new List<GameObject>();

    [Header("Шансы на спаун комнат")]
    public float rareRoomChance = 0.2f;
    public float mythicRoomChance = 0.5f;
    public float legendaryRoomChance = 0.01f;

    [Header("Варианиты вертикальных и горизонтальных коридоров")]
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
    public float roomSpawnChance = 0.5f;

    [Tooltip("Могут ли появиться удлиненные коридоры")]
    public bool moreCorridors = false;

    private int bad_corridors_count;
    public float setka_size = 4;
    public int[] limits = new int[4];
    public List<GameObject> rooms = new List<GameObject>();
    public int count;
    public int finishCount;

    [HideInInspector] public List<GameObject> spawned_rooms = new List<GameObject>();
    [HideInInspector] public List<GameObject> spawnedEscapeBlocks = new List<GameObject>();
    
    [HideInInspector] public List<GameObject> bad_corridors = new List<GameObject>();
    [HideInInspector] public List<GameObject> spawned_corridors = new List<GameObject>();
    private GameObject spawned_room, last_room, spawned_corridor;
    private List<Vector2> quests = new List<Vector2>();

    private int vert, hor;
    private bool isFinished = false;

    public int roomCounter;

    public static RoomGeneratorNew Instance;

    int deleteThisRoomCount = 0;
    

    // Start is called before the first frame update

    private void Awake()
    {
        Instance = this;

        if (drivenByData)
        {
            startRoom = data.startRoom;
            publicRooms = data.publicRooms;
            publicCorridorsHorizontal = data.publicCorridorsHorizontal;
            publicCorridorsVertical = data.publicCorridorsVertical;
            roomLimit = data.roomLimit;
            roomLimitMin = data.roomLimitMin;
            farFromSpawn = data.farFromSpawn;
            enemyCampsCount = data.enemyCampsCount;
            roomSpawnChance = data.roomSpawnChance;
            rareRoomChance = data.rareRoomChance;
            mythicRoomChance = data.mythicRoomChance;
            legendaryRoomChance = data.legendaryRoomChance;
        }
    }

    void Start()
    {
        SRoomAfterData firstRoomData = ScriptableObject.CreateInstance<SRoomAfterData>();
        firstRoomData.doorTopSize = 10;
        SpawnRoom(new Vector2(0.5f, 0.5f), startRoom, RoomUtils.DirectionType.noDirection, 0, 0, firstRoomData);
    }

    // Update is called once per frame
    void Update()
    {
        if (isFinished) return;
        if ((RoomsCollision2(spawned_rooms, spawned_corridors) == false) | roomCounter < roomLimitMin)
        {
            for (int i = 0; i <= spawned_rooms.Count - 1; i++)
            {
                try
                {
                    Destroy(spawned_rooms[i]);
                }
                catch
                {

                }
            }
            for (int i = 0; i <= spawned_corridors.Count - 1; i++)
            {
                try
                {
                    Destroy(spawned_corridors[i]);
                }
                catch
                {

                }
            }
            spawned_rooms.Clear();
            spawned_corridors.Clear();
            bad_corridors.Clear();
            hor = 0;
            vert = 0;
            roomCounter = 0;
            SRoomAfterData firstRoomData = ScriptableObject.CreateInstance<SRoomAfterData>();
            firstRoomData.doorTopSize = 10;
            SpawnRoom(new Vector2(0.5f, 0.5f), startRoom, RoomUtils.DirectionType.noDirection, 0, 0, firstRoomData);
        }
        else
        {
            finishCount++;
            if (finishCount >= 30)
            {
                isFinished = true;
                Finish();
                finishCount = 0;
            }
        }
        count += 1;
        if (count > 10000)
        {
            //gameObject.GetComponent<RoomGenerator>().enabled = false;
            Debug.Log("� ���� ������");
        }
    }

    bool RoomsCollision2(List<GameObject> rooms, List<GameObject> corridors)
    {
        GameObject fullRoom = null;
        GameObject fullCorridor = null;
        for (int i = 0; i <= corridors.Count - 1; i++)
        {
            try
            {
                fullCorridor = corridors[i].transform.Find("FullRoom").gameObject;
                fullCorridor.gameObject.SetActive(true);
            }
            catch
            {

            }
        }
        for (int i = 0; i <= rooms.Count - 1; i++)
        {
            try
            {
                fullRoom = rooms[i].transform.Find("FullRoom").gameObject;
                fullRoom.gameObject.SetActive(true);
                if (fullRoom.GetComponent<RoomCollision>().tog == 0)
                {
                    fullRoom.gameObject.SetActive(false);
                    return false;
                }
                else
                {
                }
            }
            catch { }
        }
        for (int i = 0; i <= corridors.Count - 1; i++)
        {
            try
            {
                fullCorridor = corridors[i].transform.Find("FullRoom").gameObject;
                fullCorridor.gameObject.SetActive(true);
                if (fullCorridor.GetComponent<RoomCollision>().tog == 0)
                {
                    fullCorridor.SetActive(false);
                    return false;
                }
                else
                {
                }
            }
            catch
            {

            }
        }
        try
        {
            fullCorridor.SetActive(false);
            fullRoom.SetActive(false);
        }
        catch
        {
        }
        return true;

    }

    bool RoomsCollision(List<GameObject> rooms, List<GameObject> corridors)
    {
        for (int i = 0; i <= rooms.Count - 1; i++)
        {
            try
            {
                GameObject fullRoom = rooms[i].transform.Find("FullRoom").gameObject;
                fullRoom.gameObject.SetActive(true);
            }
            catch
            {
            }
        }
        return true;

    }
    
    bool SpawnRoom(Vector3 position, GameObject room, RoomUtils.DirectionType directionFromTo, int farFromSpawnVer, int farFromSpawnHor, SRoomAfterData roomAfterData = null)
    {
        Vector3[] vectors;
        vectors = new Vector3[4];

        spawned_room = Instantiate(room, position, new Quaternion(0, 0, 0, 0));

        SRoomAfterData localRoomAfterData = ScriptableObject.CreateInstance<SRoomAfterData>();
        if (roomAfterData != null)
        {
            localRoomAfterData = roomAfterData;
        }
        
        Transform endblock_top, endblock_right, endblock_bot, endblock_left;
        endblock_top = spawned_room.transform.Find("endblock_top");
        endblock_right = spawned_room.transform.Find("endblock_right");
        endblock_bot = spawned_room.transform.Find("endblock_bot");
        endblock_left = spawned_room.transform.Find("endblock_left");

        if (endblock_top == null)
        {
            localRoomAfterData.roomHasCorridorTop = false;
        }
        if (endblock_right == null)
        {
            localRoomAfterData.roomHasCorridorRight = false;
        }
        if (endblock_bot == null)
        {
            localRoomAfterData.roomHasCorridorBot = false;
        }
        if (endblock_left == null)
        {
            localRoomAfterData.roomHasCorridorLeft = false;
        }

        Debug.Log($"�����������: {directionFromTo}");
        localRoomAfterData.roomSpawnChance = roomSpawnChance;
        localRoomAfterData = RoomUtils.FillFullRoom(spawned_room, roomTilesData, directionFromTo, localRoomAfterData);

        Debug.Log($"������ ����� {localRoomAfterData.doorRightSize}, ����� - {localRoomAfterData.doorLeftSize}");

        switch (directionFromTo)
        {
            case RoomUtils.DirectionType.fromBotToTop:
                vert += 1;
                Debug.Log("���");
                spawned_room.transform.position = new Vector2(position.x - localRoomAfterData.endblock_bot.transform.localPosition.x, position.y + Mathf.Abs(localRoomAfterData.endblock_bot.transform.localPosition.y) + 1);
                break;
                // ��� ����� �������: ������� ������� ����������, ����� ����� ������ ��������� � �����, ��� ����� ����� �������. �� ������ ������ ������� ����� ������ ����� �����, ���� ������ ����� ����� ��������
                // ����������� �� ��������� ����������� �������
            case RoomUtils.DirectionType.fromLeftToRight:
                hor += 1;
                Debug.Log("���");
                spawned_room.transform.position = new Vector2(position.x + Mathf.Abs(localRoomAfterData.endblock_left.transform.localPosition.x) + 1, position.y - localRoomAfterData.endblock_left.transform.localPosition.y - 0.5f);
                break;
            
            // ��� ��� ���� - � ���������
            case RoomUtils.DirectionType.fromTopToBot:
                vert += 1;
                Debug.Log("���");
                spawned_room.transform.position = new Vector2(position.x - localRoomAfterData.endblock_top.transform.localPosition.x, position.y - localRoomAfterData.endblock_top.transform.localPosition.y - 1f);
                break;
            case RoomUtils.DirectionType.fromRightToLeft:
                hor += 1;
                Debug.Log("���");
                spawned_room.transform.position = new Vector2(position.x - Mathf.Abs(localRoomAfterData.endblock_right.transform.localPosition.x) - 1, position.y - localRoomAfterData.endblock_right.transform.localPosition.y - 0.5f);
                break;
            default:
                break;
        }
        int vertLocal = vert, horLocal = hor;// ����� X
        if (Mathf.Abs(vert) > farFromSpawn || Mathf.Abs(hor) > farFromSpawn)
        {
            Debug.Log(position + " - �� ���� ����������, �����, ����� ��� ��� ��� " + vert + " " + hor);
            if (directionFromTo == RoomUtils.DirectionType.fromBotToTop || directionFromTo == RoomUtils.DirectionType.fromTopToBot)
            {
                vert -= 1;
                vertLocal -= 1;
            }
            else
            {
                hor -= 1;
                horLocal -= 1;
            }
            return false;
        }
        //int vertLocal = vert, horLocal = hor; // ���� ����� ����, ����������� �� ����� X
        GameObject gold_room = spawned_room;
        //spawned_room = Instantiate(room, position, new Quaternion(0, 0, 0, 0)); // ���������� ������� 1 �� �������� � ������ �����������

        // �������� �������. �������� ��������� ������� ������� ����� �������
        //SRoomAfterData localRoomAfterData = RoomUtils.FillFullRoom(spawned_room, roomTilesData, directionFromTo,roomSpawnChance);
        for (int i = 0; i < 4; i++)
        {
            if (localRoomAfterData.readyToBuildRoomsHere[i])
            {
                //vectors[i] = endblock0.transform.position;
                Debug.Log($"{localRoomAfterData.readyToBuildRoomsHere} �����");
                switch (i)
                {
                    case 0:
                        vectors[0] = localRoomAfterData.endblock_top.transform.position;
                        break;
                    case 1:
                        vectors[1] = localRoomAfterData.endblock_right.transform.position;
                        break;
                    case 2:
                        vectors[2] = localRoomAfterData.endblock_bot.transform.position;
                        break;
                    case 3:
                        vectors[3] = localRoomAfterData.endblock_left.transform.position;
                        break;
                }
                // �������� �������
            }
        }
        for (int i = 0; i < spawned_rooms.Count; i++) // �����, �������, ��� 2 �����, 1 ���� �����, �� ������ ��������
        {
            if ((AllowToSpawn(spawned_room, spawned_rooms[i]) == false) || (roomCounter >= roomLimit)) // ������ ��� �� 0 ��� ����
            {
                Destroy(spawned_room);
                spawned_room = gold_room;
                if (directionFromTo == RoomUtils.DirectionType.fromBotToTop || directionFromTo == RoomUtils.DirectionType.fromTopToBot)
                {
                    vert -= 1;
                    vertLocal -= 1;
                }
                else
                {
                    hor -= 1;
                    horLocal -= 1;
                }
                for (int g = 0; g < bad_corridors.Count; g++)
                {
                    Destroy(bad_corridors[g]);
                }
                return false;
            }
        }
        for (int i = 0; i < spawned_corridors.Count - 1; i++) // �����, �������, ��� 2 �����, 1 ���� �����, �� ������ ��������
        {
            if (AllowToSpawn(spawned_room, spawned_corridors[i]) == false)
            {
                Destroy(spawned_room);
                spawned_room = gold_room;
                if (directionFromTo == RoomUtils.DirectionType.fromBotToTop || directionFromTo == RoomUtils.DirectionType.fromTopToBot)
                {
                    vert -= 1;
                    vertLocal -= 1;
                }
                else
                {
                    hor -= 1;
                    horLocal -= 1;
                }
                for (int g = 0; g < bad_corridors.Count; g++)
                {
                    Destroy(bad_corridors[g]);
                }
                return false;
            }
        }

        gold_room = spawned_room;
        bad_corridors.Clear();
        spawned_rooms.Add(spawned_room);
        int[] roomTypes = new int[4];
        roomCounter += 1;
        SRoomAfterData finalRoomData = new SRoomAfterData();

        // ����� - ��������� �������, ���� �� ������ ���������.
        if (vertLocal >= farFromSpawn || horLocal >= farFromSpawn)
        {
            switch (directionFromTo)
            {
                case RoomUtils.DirectionType.fromBotToTop:
                    finalRoomData.roomDirectionFromTo = RoomUtils.DirectionType.fromTopToBot;
                    RoomUtils.FillRoomSide(spawned_room, RoomUtils.RoomSide.top, roomTilesData, finalRoomData);
                    finalRoomData.roomDirectionFromTo = RoomUtils.DirectionType.fromRightToLeft;
                    RoomUtils.FillRoomSide(spawned_room, RoomUtils.RoomSide.right, roomTilesData, finalRoomData);
                    finalRoomData.roomDirectionFromTo = RoomUtils.DirectionType.fromLeftToRight;
                    RoomUtils.FillRoomSide(spawned_room, RoomUtils.RoomSide.left, roomTilesData, finalRoomData);
                    break;

                case RoomUtils.DirectionType.fromRightToLeft:
                    finalRoomData.roomDirectionFromTo = RoomUtils.DirectionType.fromBotToTop;
                    RoomUtils.FillRoomSide(spawned_room, RoomUtils.RoomSide.bot, roomTilesData, finalRoomData);
                    finalRoomData.roomDirectionFromTo = RoomUtils.DirectionType.fromTopToBot;
                    RoomUtils.FillRoomSide(spawned_room, RoomUtils.RoomSide.top, roomTilesData, finalRoomData);
                    finalRoomData.roomDirectionFromTo = RoomUtils.DirectionType.fromLeftToRight;
                    RoomUtils.FillRoomSide(spawned_room, RoomUtils.RoomSide.left, roomTilesData, finalRoomData);
                    break;

                case RoomUtils.DirectionType.fromTopToBot:
                    finalRoomData.roomDirectionFromTo = RoomUtils.DirectionType.fromBotToTop;
                    RoomUtils.FillRoomSide(spawned_room, RoomUtils.RoomSide.bot, roomTilesData, finalRoomData);
                    finalRoomData.roomDirectionFromTo = RoomUtils.DirectionType.fromRightToLeft;
                    RoomUtils.FillRoomSide(spawned_room, RoomUtils.RoomSide.right, roomTilesData, finalRoomData);
                    finalRoomData.roomDirectionFromTo = RoomUtils.DirectionType.fromLeftToRight;
                    RoomUtils.FillRoomSide(spawned_room, RoomUtils.RoomSide.left, roomTilesData, finalRoomData);
                    break;

                case RoomUtils.DirectionType.fromLeftToRight:
                    finalRoomData.roomDirectionFromTo = RoomUtils.DirectionType.fromBotToTop;
                    RoomUtils.FillRoomSide(spawned_room, RoomUtils.RoomSide.bot, roomTilesData, finalRoomData);
                    finalRoomData.roomDirectionFromTo = RoomUtils.DirectionType.fromTopToBot;
                    RoomUtils.FillRoomSide(spawned_room, RoomUtils.RoomSide.top, roomTilesData, finalRoomData);
                    finalRoomData.roomDirectionFromTo = RoomUtils.DirectionType.fromRightToLeft;
                    RoomUtils.FillRoomSide(spawned_room, RoomUtils.RoomSide.right, roomTilesData, finalRoomData);
                    break;
            }
            //RoomUtils.FillFullRoom(spawned_room, roomTilesData, directionFromTo, 1, finalRoomData);
            return true;
        }
        last_room = spawned_room;
        for (int i = 0; i <= vectors.Length - 1; i++)
        {
            finalRoomData = SRoomAfterData.CreateInstance<SRoomAfterData>();
            vert = vertLocal;
            hor = horLocal;
            Vector3 vr_vector = new Vector3(0f, 0f, 0f);
            if (vectors[i] != vr_vector)
            {
                if (roomCounter <= roomLimit)
                {
                    if (i == 0)
                    {
                        vert = vertLocal; 
                        if (directionFromTo == RoomUtils.DirectionType.noDirection)
                        {
                            SpawnCorridor(vectors[i], publicCorridorsVertical[0], RoomUtils.DirectionType.noDirection, localRoomAfterData.doorTopSize, vert, hor);
                        }
                        else
                        {
                            if (SpawnCorridor(vectors[i], publicCorridorsVertical[0], RoomUtils.DirectionType.fromBotToTop, localRoomAfterData.doorTopSize, vert, hor) == false)
                            {
                                Debug.Log("�� ���� ����");
                                finalRoomData.roomDirectionFromTo = RoomUtils.DirectionType.fromTopToBot;
                                RoomUtils.FillRoomSide(gold_room, RoomUtils.RoomSide.top, roomTilesData, finalRoomData);
                                Debug.Log($"{finalRoomData.roomDirectionFromTo}, {finalRoomData.doorTopSize} ������");
                            };
                        }
                    }
                    else if (i == 1)
                    {
                        hor = horLocal;
                        if (SpawnCorridor(vectors[i], publicCorridorsHorizontal[0], RoomUtils.DirectionType.fromLeftToRight, localRoomAfterData.doorRightSize, vert, hor) == false)
                        {
                            Debug.Log("�� ���� �����");
                            finalRoomData.roomDirectionFromTo = RoomUtils.DirectionType.fromRightToLeft;
                            RoomUtils.FillRoomSide(gold_room, RoomUtils.RoomSide.right, roomTilesData, finalRoomData);
                            Debug.Log($"{finalRoomData.roomDirectionFromTo}, {finalRoomData.doorRightSize} ������");
                        };
                    }
                    else if (i == 2)
                    {
                        vert = vertLocal;
                        if (SpawnCorridor(vectors[i], publicCorridorsVertical[0], RoomUtils.DirectionType.fromTopToBot, localRoomAfterData.doorBotSize, vert, hor) == false)
                        {
                            Debug.Log("�� ���� ���");
                            finalRoomData.roomDirectionFromTo = RoomUtils.DirectionType.fromBotToTop;
                            RoomUtils.FillRoomSide(gold_room, RoomUtils.RoomSide.bot, roomTilesData, finalRoomData);
                            Debug.Log($"{finalRoomData.roomDirectionFromTo}, {finalRoomData.doorBotSize} ������");
                        };
                    }
                    else if (i == 3)
                    {
                        hor = horLocal;
                        Debug.Log("�����!");
                        if (SpawnCorridor(vectors[i], publicCorridorsHorizontal[0], RoomUtils.DirectionType.fromRightToLeft, localRoomAfterData.doorLeftSize, vert, hor) == false)
                        {
                            Debug.Log("�� ���� �����");
                            finalRoomData.roomDirectionFromTo = RoomUtils.DirectionType.fromLeftToRight;
                            RoomUtils.FillRoomSide(gold_room, RoomUtils.RoomSide.left, roomTilesData, finalRoomData);
                            Debug.Log($"{finalRoomData.roomDirectionFromTo}, {finalRoomData.doorLeftSize} ������");
                        };
                    }

                }
                else
                {
                    Debug.Log("�������� ������!");
                }
            }
        }
        Vector3 vr_vector2 = new Vector3(0f, 0f, 0f);
        int lim = 0;
        for (int i = 0; i <= vectors.Length - 1; i++)
        {
            if (vectors[i] == vr_vector2)
            {
                lim += 1;
            }
        }

        deleteThisRoomCount++;
        //spawned_room.GetComponent<RoomInfo>().AfterFullSpawn(deleteThisRoomCount);
        return true;

    }


    public bool SpawnCorridor(Vector3 position, GameObject corridor, RoomUtils.DirectionType directionFromTo, int doorSize, int farFromSpawnVert = 0, int farFromSpawnHor = 0, bool moreCorridors = false)
    // mode = 0 - ��������� ��������, mode = 1 - ������������� ��������
    {

        int vertLocal = farFromSpawnVert, horLocal = farFromSpawnHor;
        if (vert > farFromSpawn || hor > farFromSpawn)
        {
            Debug.Log(position + " - �� ���� ���������� �������, �����, ����� ��� ��� ��� " + vertLocal + " " + horLocal);
            return false;
        }

        Vector3[] vectors;
        vectors = new Vector3[4];
        Vector2 spawnblock_pos;
        GameObject blocksNot, endblock, spawnblock, closeblock;

        GameObject newcorridor;
        if (directionFromTo == RoomUtils.DirectionType.fromBotToTop || directionFromTo == RoomUtils.DirectionType.fromTopToBot)
        {
            newcorridor = publicCorridorsVertical[Random.Range(0, publicCorridorsVertical.Count)];
        }
        else
        {
            newcorridor = publicCorridorsHorizontal[Random.Range(0, publicCorridorsHorizontal.Count)];
        }

        SCorridorAfterData corridorAfterInfo = new SCorridorAfterData();
        GameObject nextRoom;
        SRoomAfterData newRoomData = new SRoomAfterData();

        switch (directionFromTo)
        {
            case RoomUtils.DirectionType.fromBotToTop:
                spawned_corridor = Instantiate(corridor, position, new Quaternion(0, 0, 0, 0)); // ���������� ������� 1 �� �������� � ������ �����������

                corridorAfterInfo = RoomUtils.CreateCorridor(spawned_corridor, directionFromTo, doorSize, 4, 20, roomTilesData);

                bad_corridors.Add(spawned_corridor);
                spawned_corridors.Add(spawned_corridor);
                bad_corridors_count++;

                spawnblock = spawned_corridor.transform.Find("endblock_bot").gameObject;
                spawnblock_pos = spawnblock.transform.position;
                position = new Vector3(spawned_corridor.transform.position.x - spawnblock.transform.localPosition.x, spawned_corridor.transform.position.y + spawnblock.transform.localPosition.y, 0);
                spawned_corridor.transform.position = position;

                nextRoom = RoomUtils.SelectOneRoomUltimate(RoomUtils.RoomSide.bot, publicRooms, doorSize); // ��� ����� ���� � ������ ������� - ����� ���������
                newRoomData = new SRoomAfterData();
                newRoomData.doorBotSize = doorSize;
                if (!SpawnRoom(corridorAfterInfo.endblock_top.transform.position, nextRoom, RoomUtils.DirectionType.fromBotToTop, farFromSpawnVert, farFromSpawnHor, newRoomData))
                {
                    return false;
                }
                break;

            case RoomUtils.DirectionType.fromLeftToRight:
                spawned_corridor = Instantiate(corridor, position, new Quaternion(0, 0, 0, 0)); // ���������� ������� 1 �� �������� � ������ �����������

                corridorAfterInfo = RoomUtils.CreateCorridor(spawned_corridor, directionFromTo, doorSize, 4, 20, roomTilesData);

                bad_corridors.Add(spawned_corridor);
                spawned_corridors.Add(spawned_corridor);
                bad_corridors_count++;

                spawnblock = spawned_corridor.transform.Find("endblock_left").gameObject;
                spawnblock_pos = spawnblock.transform.position;
                position = new Vector3(spawned_corridor.transform.position.x + spawnblock.transform.localPosition.x, spawned_corridor.transform.position.y - spawnblock.transform.localPosition.y + 0.5f, 0);
                spawned_corridor.transform.position = position;

                nextRoom = RoomUtils.SelectOneRoomUltimate(RoomUtils.RoomSide.left, publicRooms, doorSize); // ��� ����� ���� � ������ ������� - ����� ���������
                newRoomData = new SRoomAfterData();
                newRoomData.doorLeftSize = doorSize;
                if (!SpawnRoom(corridorAfterInfo.endblock_right.transform.position, nextRoom, RoomUtils.DirectionType.fromLeftToRight, farFromSpawnVert, farFromSpawnHor, newRoomData))
                {
                    return false;
                }
                break;

            case RoomUtils.DirectionType.fromTopToBot:
                spawned_corridor = Instantiate(corridor, position, new Quaternion(0, 0, 0, 0)); // ���������� ������� 1 �� �������� � ������ �����������

                corridorAfterInfo = RoomUtils.CreateCorridor(spawned_corridor, directionFromTo, doorSize, 4, 20, roomTilesData);

                bad_corridors.Add(spawned_corridor);
                spawned_corridors.Add(spawned_corridor);
                bad_corridors_count++;

                spawnblock = spawned_corridor.transform.Find("endblock_top").gameObject;
                spawnblock_pos = spawnblock.transform.position;
                position = new Vector3(spawned_corridor.transform.position.x - spawnblock.transform.localPosition.x, spawned_corridor.transform.position.y - spawnblock.transform.localPosition.y - 1, 0);
                spawned_corridor.transform.position = position;

                nextRoom = RoomUtils.SelectOneRoomUltimate(RoomUtils.RoomSide.top, publicRooms, doorSize); // ��� ����� ���� � ������ ������� - ����� ���������
                newRoomData = new SRoomAfterData();
                newRoomData.doorTopSize = doorSize;
                if (!SpawnRoom(corridorAfterInfo.endblock_bot.transform.position, nextRoom, RoomUtils.DirectionType.fromTopToBot, farFromSpawnVert, farFromSpawnHor, newRoomData))
                {
                    return false;
                }
                break;

            case RoomUtils.DirectionType.fromRightToLeft:
                Debug.Log("�����!2");
                spawned_corridor = Instantiate(corridor, position, new Quaternion(0, 0, 0, 0)); // ���������� ������� 1 �� �������� � ������ �����������

                corridorAfterInfo = RoomUtils.CreateCorridor(spawned_corridor, directionFromTo, doorSize, 4, 20, roomTilesData);

                bad_corridors.Add(spawned_corridor);
                spawned_corridors.Add(spawned_corridor);
                bad_corridors_count++;

                spawnblock = spawned_corridor.transform.Find("endblock_right").gameObject;
                spawnblock_pos = spawnblock.transform.position;
                position = new Vector3(spawned_corridor.transform.position.x - spawnblock.transform.localPosition.x - 1, spawned_corridor.transform.position.y - spawnblock.transform.localPosition.y + 0.5f, 0);
                spawned_corridor.transform.position = position;

                nextRoom = RoomUtils.SelectOneRoomUltimate(RoomUtils.RoomSide.right, publicRooms, doorSize); // ��� ����� ���� � ������ ������� - ����� ���������
                newRoomData = new SRoomAfterData();
                newRoomData.doorRightSize = doorSize;
                if (!SpawnRoom(corridorAfterInfo.endblock_left.transform.position, nextRoom, RoomUtils.DirectionType.fromRightToLeft, farFromSpawnVert, farFromSpawnHor, newRoomData))
                {
                    return false;
                }
                break;

            // ������ ������� - ���������� ������� ������� (30 �����)
            case RoomUtils.DirectionType.noDirection:
                spawned_corridor = Instantiate(corridor, position, new Quaternion(0, 0, 0, 0)); // ���������� ������� 1 �� �������� � ������ �����������

                corridorAfterInfo = RoomUtils.CreateCorridor(spawned_corridor, RoomUtils.DirectionType.fromBotToTop, doorSize, 30, 30, roomTilesData);

                bad_corridors.Add(spawned_corridor);
                spawned_corridors.Add(spawned_corridor);
                bad_corridors_count++;

                spawnblock = spawned_corridor.transform.Find("endblock_bot").gameObject;
                spawnblock_pos = spawnblock.transform.position;
                position = new Vector3(spawned_corridor.transform.position.x - spawnblock.transform.localPosition.x, spawned_corridor.transform.position.y + spawnblock.transform.localPosition.y, 0);
                spawned_corridor.transform.position = position;

                nextRoom = RoomUtils.SelectOneRoomUltimate(RoomUtils.RoomSide.bot, publicRooms, doorSize); // ��� ����� ���� � ������ ������� - ����� ���������
                newRoomData = new SRoomAfterData();
                newRoomData.doorBotSize = doorSize;
                if (!SpawnRoom(corridorAfterInfo.endblock_top.transform.position, nextRoom, RoomUtils.DirectionType.fromBotToTop, farFromSpawnVert, farFromSpawnHor, newRoomData))
                {
                    return false;
                }
                break;
        }

                
        for (int i = 0; i < spawned_corridors.Count - 1; i++) // �����, �������, ��� 2 �����, 1 ���� �����, �� ������ ��������
        {
            if (AllowToSpawn(spawned_corridor, spawned_corridors[i], 1) == false)
            {
                spawned_corridors.Remove(spawned_corridor);
                Destroy(spawned_corridor);
                return false;
            }
        }
        for (int i = 0; i < spawned_rooms.Count; i++) // �����, �������, ��� 2 �����, 1 ���� �����, �� ������ ��������
        {
            if (AllowToSpawn(spawned_corridor, spawned_rooms[i], 1) == false)
            {
                spawned_corridors.Remove(spawned_corridor);
                Destroy(spawned_corridor);
                return false;
            }
        }
        return true;
    }


    public bool AllowToSpawn(GameObject spawnedRoom, GameObject targetRoom, int mode = 0)
    {
        if ((spawnedRoom == null) || (targetRoom.name == null))
        {
            Debug.Log("�����");
            return true;
        }
        int count = 0;
        if (mode == 1)
        {
            count = 3;
        }
        Vector3 LB0T = targetRoom.transform.Find("limit_block_0").position;
        Vector3 LB1T = targetRoom.transform.Find("limit_block_1").position;
        Vector3 LB2T = targetRoom.transform.Find("limit_block_2").position;
        Vector3 LB3T = targetRoom.transform.Find("limit_block_3").position;

        Vector3 LB0S = spawnedRoom.transform.Find("limit_block_0").position;
        Vector3 LB1S = spawnedRoom.transform.Find("limit_block_1").position;
        Vector3 LB2S = spawnedRoom.transform.Find("limit_block_2").position;
        Vector3 LB3S = spawnedRoom.transform.Find("limit_block_3").position;

        if ((LB0S.x >= LB1T.x) | (LB1S.x <= LB0T.x))
        {
            count += 2;
            //Debug.Log("1 ���� ����");
        }
        if ((LB0S.y <= LB3T.y) | (LB3S.y >= LB0T.y))
        {
            count += 2;
            //Debug.Log("2 ���� ����");
        }
        //if (limitBlock0_spawned == limitBlock0_target && limitBlock1_spawned == limitBlock1_target && limitBlock2_spawned == limitBlock2_target && limitBlock3_spawned == limitBlock3_target) {
        //    
        //    return false;
        //}
        if ((LB0S.x == LB0T.x && LB1S.x == LB1T.x))
        {
            count += 1;
        }
        else
        {
            count += 1;
        }
        if ((LB0S.y == LB0T.y && LB2S.y == LB2T.y))
        {
            count += 1;
        }
        else
        {
            count += 1;
        }
        if (spawnedRoom.transform.position == targetRoom.transform.position)
        {
            count = count;
        }
        if (count >= 3)
        {
            List<GameObject> techRoom = new List<GameObject>();
            List<GameObject> techCorridor = new List<GameObject>();
            techRoom.Add(spawnedRoom);
            techCorridor.Add(targetRoom);
            if (RoomsCollision2(techRoom, techCorridor) == true)
            {
                //Debug.Log("������");
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            Debug.Log($"������ ������, ������� �������: {spawnedRoom.transform.position} ������ �������: {targetRoom.transform.position}");
            return false;
        }
    }

    public void DisableAllColliders(List<GameObject> rooms, List<GameObject> corridors)
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            try
            {
                GameObject fullroom = rooms[i].transform.Find("FullRoom").gameObject;
                fullroom.gameObject.SetActive(false);
            }
            catch
            {
            }
        }
        for (int i = 0; i < corridors.Count; i++)
        {
            try
            {
                GameObject fullcorridor = corridors[i].transform.Find("FullRoom").gameObject;
                fullcorridor.gameObject.SetActive(false);
            }
            catch
            {
            }
        }

    }

    void AddCorridors()
    {

    }
    

    void SpawnEscapeBlockZone(int escapeBlocksCount = 1)
    {
        // Сколько попыток есть у генератора на то, чтобы найти комнату с центром
        int attemps = 50;
        List<GameObject> copyRooms = new List<GameObject>(spawned_rooms);
        
        for (int i = 0; i < escapeBlocksCount; i++)
        {
            copyRooms.Shuffle();
            GameObject targetRoom = null;

            foreach (GameObject room in copyRooms)
            {
                RoomInfo roomInfo  = room.GetComponent<RoomInfo>();
                if (roomInfo.centreBlock != null && roomInfo.sideRoom == true && roomInfo.roomType == RoomUtils.RoomTypes.defaultRoom)
                {
                    targetRoom = room;
                    copyRooms.Remove(room);
                    break;
                }
            }
            spawnedEscapeBlocks.Add(Instantiate(data.escapeBlock, targetRoom.GetComponent<RoomInfo>().centreBlock.transform.position, Quaternion.identity));
        }
        

    }


    GameObject GetRandomRoom(List<GameObject> commonRoomList, List<GameObject> rareRoomList = null, List<GameObject> mythicRoomList = null, List<GameObject> legendaryRoomList = null)
    {
        float randFloat = Random.Range(0f, 1f);
        GameObject returnedRoom = null;
        if (rareRoomChance == 0)
        {
            rareRoomList = null;
        }
        if (mythicRoomChance == 0)
        {
            mythicRoomList = null;
        }
        if (legendaryRoomChance == 0)
        {
            legendaryRoomList = null;
        } // �������������, ����� ����� �� �������� ������ ������� ���� �� ���� ������ = 0

        if ((randFloat <= legendaryRoomChance) && (legendaryRoomList != null) && (legendaryRoomList.Count != 0))
        {
            returnedRoom = legendaryRoomList[Random.Range(0, legendaryRoomList.Count)];
            Debug.Log($"������ ����������� ������� � ������ {randFloat}");
            return returnedRoom;
        }
        if ((randFloat <= mythicRoomChance) && (mythicRoomList != null) && (mythicRoomList.Count != 0))
        {
            returnedRoom = mythicRoomList[Random.Range(0, mythicRoomList.Count)];
            return returnedRoom;
        }
        if ((randFloat <= rareRoomChance) && (rareRoomList != null) && (rareRoomList.Count != 0))
        {
            returnedRoom = rareRoomList[Random.Range(0, rareRoomList.Count)];
            return returnedRoom;
        }
        returnedRoom = commonRoomList[Random.Range(0, commonRoomList.Count)];
        return returnedRoom;
    }

    List<GameObject> SpawnEnemyCamps()
    {
        return null;
    }

    bool SpawnEnemyCampsFull(List<GameObject> roomList, int roomCount)
    {
        List<GameObject> localRoomList = new List<GameObject>();
        localRoomList.AddRange(roomList);
        List<GameObject> targetRooms = RoomUtils.SelectRoomPool(localRoomList, roomCount, false);
        RoomUtils.SelectCampTypes(targetRooms, data.mediumChance, data.hardChance, data.insaneChance);
        foreach (GameObject room in targetRooms)
        {
            room.GetComponent<RoomInfo>().canSpawn = false; // � ������� �� ����� ���������� �������� �������
            int spawnersCount = 0;
            switch (room.GetComponent<RoomInfo>().campDifficult)
            {
                case RoomUtils.CampDifficults.easy:
                    spawnersCount = data.EasyRoomSpawnersCount;
                    RoomUtils.SpawnSingleChest(room, data.classicChest);
                    break;
                case RoomUtils.CampDifficults.medium:
                    spawnersCount = data.MediumRoomSpawnersCount;
                    RoomUtils.SpawnSingleChest(room, data.classicChest, 2);
                    break;
                case RoomUtils.CampDifficults.hard:
                    spawnersCount = data.HardRoomSpawnersCount;
                    RoomUtils.SpawnSingleChest(room, data.classicChest, 3);
                    break;
                case RoomUtils.CampDifficults.insane:
                    spawnersCount = data.InsaneRoomSpawnersCount;
                    RoomUtils.SpawnSingleChest(room, data.classicChest, 4);
                    break;
            }
            List<GameObject> localSpawners = RoomUtils.GetRandomSpawners(room, spawnersCount);
            RoomUtils.SpawnEnemyOnSpawners(localSpawners, room.GetComponent<RoomInfo>().campFilling, data);
        }
        Debug.Log("����� ������ ������ �������.");
        return true;
    }
    
    
    
    private void Finish()
    {
        Debug.Log("Закончил работу над комнатами");
        DisableAllColliders(spawned_rooms, spawned_corridors);
        GameObject.Find("A*").GetComponent<AstarPath>().Scan();
        Debug.Log(spawned_rooms);
        //GetComponent<ChestSpawning>().enabled = true;
        spawned_rooms.RemoveAt(0);
        foreach (GameObject room in spawned_rooms)
        {
            room.GetComponent<RoomInfo>().AfterFullSpawn();
            room.transform.Find("FullRoom").gameObject.SetActive(true);
        }

        // ������ ����� - � ���������
        //SpawnEnemyCampsFull(spawned_rooms, enemyCampsCount);

        List<GameObject> sideRooms = new List<GameObject>();

        foreach (GameObject room in spawned_rooms)
        {
            if (RoomUtils.CheckSideRoom(room))
            {
                room.GetComponent<RoomInfo>().sideRoom = true;
                sideRooms.Add(room);
            }
        }

        // Создаем магазины
        int shopCountFormula = Mathf.Max((int)(spawned_rooms.Count / 20), 1); 
        List<GameObject> shopRooms = RoomUtils.SelectRoomPool(sideRooms, shopCountFormula);
        foreach (GameObject room in shopRooms)
        {
            room.GetComponent<RoomInfo>().roomType = RoomUtils.RoomTypes.shop;
            RoomUtils.SpawnSingleChest(room, data.classicShopCenter, 3);
        }

        // ����� ���� ��������� - �������� ����������� ������� �� ���, ������� ��������
        foreach (GameObject room in spawned_rooms)
        {
            if (room.GetComponent<RoomInfo>().roomType == RoomUtils.RoomTypes.unknown)
            {
                room.GetComponent<RoomInfo>().roomType = RoomUtils.RoomTypes.defaultRoom;
            }
        }
        RoomUtils.FullChestSpawning(spawned_rooms, RoomUtils.ChestTypes.classicChest, (int)(spawned_rooms.Count / 4), data);
        RoomUtils.FullChestSpawning(spawned_rooms, RoomUtils.ChestTypes.classicShopCenter, (int)(spawned_rooms.Count / 7), data);

        int escapeBlocksCountFormula = Mathf.Max((int)(spawned_rooms.Count / 20), 1);
        SpawnEscapeBlockZone(escapeBlocksCountFormula);
        gameObject.SetActive(false);

    }
}
