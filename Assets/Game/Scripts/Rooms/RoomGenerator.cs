using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Pathfinding;
public class RoomGenerator : MonoBehaviour
{
    // Здесь все тайлы для построения комнат
    public SRoomTilesData roomTilesData;

    public bool mainStartRoom = true;
    public SRoomInfoGenerator data;

    [Tooltip("Начальная комната - всегда фиксированная, с одним выходом")]
    public GameObject startRoom; // Начальная комната - всегда фиксированная и с одним выходом

    [Header("Варианты комнат")]
    [Tooltip("Список доступных для спауна обычных комнат")]
    public List<GameObject> publicRooms = new List<GameObject>();

    [Tooltip("Список доступных для спауна редких комнат")]
    public List<GameObject> publicRareRooms = new List<GameObject>();

    [Tooltip("Список доступных для спауна эпических комнат")]
    public List<GameObject> publicMythicRooms = new List<GameObject>();

    [Tooltip("Список доступных для спауна легендарных комнат")]
    public List<GameObject> publicLegendaryRooms = new List<GameObject>(); // Какие комнаты будут спауниться

    [Header("Шансы на спаун комнат")]
    public float rareRoomChance = 0.2f;
    public float mythicRoomChance = 0.5f;
    public float legendaryRoomChance = 0.01f; // Шансы на спаун каждой категории комнат от 0 до 100

    [Header("Варианты коридоров")]
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

    [Tooltip("Могут ли появиться удлиненные коридоры")]
    public bool moreCorridors = false;

    private int bad_corridors_count;
    public float setka_size = 4;
    public int[] limits = new int[4];
    public List<GameObject> rooms = new List<GameObject>();
    public int count;
    public int finishCount;

    public List<GameObject> spawned_rooms = new List<GameObject>();
    [HideInInspector] public List<GameObject> bad_corridors = new List<GameObject>();
    [HideInInspector] public List<GameObject> spawned_corridors = new List<GameObject>();
    private GameObject spawned_room, last_room, spawned_corridor;
    private List<Vector2> quests = new List<Vector2>();

    private int vert, hor;
    private bool isFinished = false;

    public int roomCounter;

    // ТЕСТОВАЯ ЗОНА, ПЕРЕМЕННЫЕ НИЖЕ НЕ НУЖНЫ

    // Start is called before the first frame update
    void Start()
    {
        SpawnRoom(new Vector2(0.5f, 0.5f), startRoom, 4, 0, 0);
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
            Debug.Log("Переделываю, увидел столкновение");
            hor = 0;
            vert = 0;
            roomCounter = 0;
            SpawnRoom(new Vector2(0.5f, 0.5f), startRoom, 4, 0, 0);
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
            Debug.Log("С меня хватит");
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

    bool SpawnRoom(Vector3 position, GameObject room, int spawnType, int farFromSpawnVer, int farFromSpawnHor)
    {
        Vector3[] vectors;
        vectors = new Vector3[4];
        Vector2 spawnblock_pos;
        GameObject blocksNot0, endblock0, spawnblock0;
        GameObject blocksNot1, endblock1, spawnblock1;
        GameObject blocksNot2, endblock2, spawnblock2;
        GameObject blocksNot3, endblock3, spawnblock3;

        while (true)
        {
            switch (spawnType)
            {
                case 0:
                    try
                    {
                        GameObject endblock = room.transform.Find("endblock_bot").gameObject;
                    }
                    catch
                    {
                        //room = publicRooms[Random.Range(0, publicRooms.Count)];
                        room = GetRandomRoom(publicRooms, publicRareRooms, publicMythicRooms, publicLegendaryRooms);
                        continue;
                    }
                    break;
                case 1:
                    try
                    {
                        GameObject endblock = room.transform.Find("endblock_left").gameObject;
                    }
                    catch
                    {
                        room = GetRandomRoom(publicRooms, publicRareRooms, publicMythicRooms, publicLegendaryRooms);
                        continue;
                    }
                    break;
                case 2:
                    try
                    {
                        GameObject endblock = room.transform.Find("endblock_top").gameObject;
                    }
                    catch
                    {
                        room = GetRandomRoom(publicRooms, publicRareRooms, publicMythicRooms, publicLegendaryRooms);
                        continue;

                    }
                    break;
                case 3:
                    try
                    {
                        GameObject endblock = room.transform.Find("endblock_right").gameObject;
                    }
                    catch
                    {
                        room = GetRandomRoom(publicRooms, publicRareRooms, publicMythicRooms, publicLegendaryRooms);
                        continue;
                    }
                    break;
            }
            break;
        }

        switch (spawnType)
        {
            case 0:
                vert += 1;
                spawnblock0 = room.transform.Find("endblock_bot").gameObject;
                spawnblock_pos = spawnblock0.transform.position;
                position = new Vector3(position.x, position.y + 1, vectors[1].z);
                break;
            case 1:
                hor += 1;
                spawnblock1 = room.transform.Find("endblock_left").gameObject;
                spawnblock_pos = spawnblock1.transform.position;
                position = new Vector3(position.x - spawnblock_pos.x + 1 / setka_size, position.y - spawnblock_pos.y, vectors[1].z);
                break;

            case 2:
                vert += 1;
                spawnblock2 = room.transform.Find("endblock_top").gameObject;
                spawnblock_pos = spawnblock2.transform.position;
                position = new Vector3(position.x - spawnblock_pos.x, position.y - spawnblock_pos.y - 1 / setka_size, vectors[1].z);
                break;
            case 3:
                hor += 1;
                spawnblock3 = room.transform.Find("endblock_right").gameObject;
                spawnblock_pos = spawnblock3.transform.position;
                position = new Vector3(position.x - spawnblock_pos.x - 1 / setka_size, position.y - spawnblock_pos.y, vectors[1].z);
                break;
            case 4:
                break;
        }
        /*switch (spawnType)
        {
            case 0:
                vert += 1;
                spawnblock0 = room.transform.Find("endblock_top").gameObject;
                spawnblock_pos = spawnblock0.transform.position;
                position = new Vector3(position.x, position.y + 1 / setka_size, vectors[1].z);
                break;
            case 1:
                spawnblock1 = room.transform.Find("endblock_left").gameObject;
                spawnblock_pos = spawnblock1.transform.position;
                position = new Vector3(position.x - spawnblock_pos.x, position.y - spawnblock_pos.y, vectors[1].z);
                hor += 1;
                break;

            case 2:
                spawnblock2 = room.transform.Find("endblock_top").gameObject;
                spawnblock_pos = spawnblock2.transform.position;
                position = new Vector3(position.x, position.y - spawnblock_pos.y - 1 / setka_size, vectors[1].z);
                break;
            case 3:
                spawnblock3 = room.transform.Find("endblock_right").gameObject;
                spawnblock_pos = spawnblock3.transform.position;
                position = new Vector3(position.x - spawnblock_pos.x, position.y - spawnblock_pos.y, vectors[1].z);
                hor += 1;
                break;
            case 4:

                break;
        }*/
        int vertLocal = vert, horLocal = hor;// МЕСТО X
        if (Mathf.Abs(vert) > farFromSpawn || Mathf.Abs(hor) > farFromSpawn)
        {
            Debug.Log(position + " - Не смог заспаунить, лимит, думаю что она уже " + vert + " " + hor);
            if (spawnType % 2 == 0)
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
        //int vertLocal = vert, horLocal = hor; // Если будут баги, переставить на место X
        GameObject gold_room = spawned_room;
        spawned_room = Instantiate(room, position, new Quaternion(0, 0, 0, 0)); // Заспаунить комнату 1 по заданным в начале координатам

        for (int i = 0; i < spawned_rooms.Count; i++) // ДЕБАГ, комнаты, где 2 брака, 1 брак чинит, на второй забивает
        {
            if ((AllowToSpawn(spawned_room, spawned_rooms[i]) == false) || (roomCounter >= roomLimit)) // Убрать мод на 0 при баге
            {
                Destroy(spawned_room);
                Debug.Log($"Хорошая комната - {gold_room.transform.position}, не смог заспаунить - {spawned_room.transform.position}. SpawnType - {spawnType}");
                spawned_room = gold_room;
                if (spawnType % 2 == 0)
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
        for (int i = 0; i < spawned_corridors.Count - 1; i++) // ДЕБАГ, комнаты, где 2 брака, 1 брак чинит, на второй забивает
        {
            if (AllowToSpawn(spawned_room, spawned_corridors[i]) == false)
            {
                Destroy(spawned_room);
                spawned_room = gold_room;
                Debug.Log($"Хорошая комната - {gold_room.transform.position}, не смог заспаунить - {spawned_room.transform.position}. SpawnType - {spawnType}");
                if (spawnType % 2 == 0)
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
        /*for (int i = 0; i < spawned_corridors.Count; i++) // ЕСЛИ ЧТО УДАЛИТЬ ЦИКЛ ЭТОТ
        {
            if (AllowToSpawn(spawned_room, spawned_corridors[i]) == false)
            {
                Destroy(spawned_room);
                Debug.Log($"Хорошая комната - {gold_room.transform.position}, не смог заспаунить - {spawned_room.transform.position}. SpawnType - {spawnType}");
                spawned_room = gold_room;
                for (int g = 0; g < bad_corridors.Count; g++)
                {
                    Destroy(bad_corridors[g]);
                }
                return false;
            }
        }*/

        gold_room = spawned_room;
        bad_corridors.Clear();
        spawned_rooms.Add(spawned_room);
        int[] roomTypes = new int[4];
        for (int i = 0; i <= 3; i = i + 1)
        {
            int randRoom = 4;
            if (room == startRoom)
            {
                Debug.Log("рум");
                if (!mainStartRoom)
                {
                    randRoom = Random.Range(0, 4);
                }
                else
                {
                    randRoom = 0;
                }
                i = randRoom;
            }
            switch (i)
            {
                case 0:
                    try
                    {
                        endblock0 = spawned_room.transform.Find("endblock_top").gameObject;
                    }
                    catch
                    {
                        break;
                    }
                    blocksNot0 = spawned_room.transform.Find("Grid").Find("blocks?_top").gameObject; // Получаем ссылку на спавнблок в новом объекте
                    spawnblock0 = spawned_room.transform.Find("endblock_top").gameObject;
                    if ((((Random.Range(0f, 1f)) <= roomSpawnChance) || (spawnType == 2)) || (randRoom == 0))
                    {
                        blocksNot0.gameObject.SetActive(false);
                        endblock0.gameObject.SetActive(true);
                        spawnblock0.gameObject.SetActive(true);
                        if (spawnType != 2)
                        {
                            vectors[0] = endblock0.transform.position;
                        }
                        roomTypes[0] = 0;
                    }
                    break;
                case 1:
                    try
                    {
                        endblock1 = spawned_room.transform.Find("endblock_right").gameObject;
                    }
                    catch
                    {
                        break;
                    }
                    blocksNot1 = spawned_room.transform.Find("Grid").Find("blocks?_right").gameObject; // Получаем ссылку на спавнблок в новом объекте
                    spawnblock1 = spawned_room.transform.Find("endblock_right").gameObject;
                    if (((Random.Range(0f, 1f) <= roomSpawnChance) || (spawnType == 3)) || (randRoom == 1))
                    {
                        blocksNot1.gameObject.SetActive(false);
                        endblock1.gameObject.SetActive(true);
                        spawnblock1.gameObject.SetActive(true);
                        if (spawnType != 3)
                        {
                            vectors[1] = endblock1.transform.position;
                        }
                        roomTypes[1] = 1;
                    }
                    break;
                case 2:
                    try
                    {
                        endblock2 = spawned_room.transform.Find("endblock_bot").gameObject;
                    }
                    catch
                    {
                        break;
                    }
                    blocksNot2 = spawned_room.transform.Find("Grid").Find("blocks?_bot").gameObject; // Получаем ссылку на спавнблок в новом объекте
                    spawnblock2 = spawned_room.transform.Find("endblock_bot").gameObject;
                    if ((((Random.Range(0f, 1f)) <= roomSpawnChance) || (spawnType == 0)) || (randRoom == 2))
                    {
                        blocksNot2.gameObject.SetActive(false);
                        endblock2.gameObject.SetActive(true);
                        spawnblock2.gameObject.SetActive(true);
                        if (spawnType != 0)
                        {
                            vectors[2] = endblock2.transform.position;
                        }
                        roomTypes[2] = 2;
                    }
                    break;
                case 3:
                    try
                    {
                        endblock3 = spawned_room.transform.Find("endblock_left").gameObject;
                    }
                    catch
                    {
                        break;
                    }
                    blocksNot3 = spawned_room.transform.Find("Grid").Find("blocks?_left").gameObject; // Получаем ссылку на спавнблок в новом объекте
                    spawnblock3 = spawned_room.transform.Find("endblock_left").gameObject;
                    if ((((Random.Range(0f, 1f)) <= roomSpawnChance) || (spawnType == 1)) || (randRoom == 3))
                    {
                        blocksNot3.gameObject.SetActive(false);
                        endblock3.gameObject.SetActive(true);
                        spawnblock3.gameObject.SetActive(true);
                        if (spawnType != 1)
                        {
                            vectors[3] = endblock3.transform.position;
                        }
                        roomTypes[3] = 3;
                    }
                    break;

            }
            if (randRoom != 4)
            {
                break;
            }
        }
        roomCounter += 1;

        // Блок закрытия комнаты, если улетели слишком далеко
        if (vertLocal >= farFromSpawn || horLocal >= farFromSpawn)
        {
            for (int i = 0; i <= 3; i = i + 1)
            {
                switch (i)
                {
                    case 0:
                        if (spawnType != 2)
                        {
                            blocksNot0 = spawned_room.transform.Find("Grid").Find("blocks?_top").gameObject; // Получаем ссылку на спавнблок в новом объекте
                            //endblock0 = spawned_room.transform.Find("endblock_top").gameObject;
                            //spawnblock0 = spawned_room.transform.Find("endblock_top").gameObject;
                            blocksNot0.gameObject.SetActive(true);
                        }
                        break;
                    case 1:
                        if (spawnType != 3)
                        {
                            blocksNot1 = spawned_room.transform.Find("Grid").Find("blocks?_right").gameObject; // Получаем ссылку на спавнблок в новом объекте
                            //endblock1 = spawned_room.transform.Find("endblock_right").gameObject;
                            //spawnblock1 = spawned_room.transform.Find("endblock_right").gameObject;
                            blocksNot1.gameObject.SetActive(true);
                        }
                        break;
                    case 2:
                        if (spawnType != 0)
                        {
                            blocksNot2 = spawned_room.transform.Find("Grid").Find("blocks?_bot").gameObject; // Получаем ссылку на спавнблок в новом объекте
                            //endblock2 = spawned_room.transform.Find("endblock_bot").gameObject;
                            //spawnblock2 = spawned_room.transform.Find("endblock_bot").gameObject;
                            blocksNot2.gameObject.SetActive(true);
                        }
                        break;
                    case 3:
                        if (spawnType != 1)
                        {
                            blocksNot3 = spawned_room.transform.Find("Grid").Find("blocks?_left").gameObject; // Получаем ссылку на спавнблок в новом объекте
                            //endblock3 = spawned_room.transform.Find("endblock_left").gameObject;
                            //spawnblock3 = spawned_room.transform.Find("endblock_left").gameObject;
                            blocksNot3.gameObject.SetActive(true);
                        }
                        break;
                }
            }
            return true;
        }
        last_room = spawned_room;
        for (int i = 0; i <= vectors.Length - 1; i++)
        {
            vert = vertLocal;
            hor = horLocal;
            Vector3 vr_vector = new Vector3(0f, 0f, 0f);
            if (vectors[i] != vr_vector)
            {
                if (roomCounter <= roomLimit)
                {
                    if (i % 2 == 0)
                    {
                        vert = vertLocal;
                        if (SpawnCorridor(vectors[i], publicCorridorsVertical[Random.Range(0, publicCorridorsVertical.Count)], i, vert, hor, moreCorridors) == false)
                        {
                            switch (i)
                            {
                                case 0:
                                    endblock0 = gold_room.transform.Find("Grid").Find("blocks?_top").gameObject;
                                    endblock0.gameObject.SetActive(true);
                                    Debug.Log($"{endblock0.transform.position} Сверху");
                                    break;
                                case 1:
                                    endblock1 = gold_room.transform.Find("Grid").Find("blocks?_right").gameObject;
                                    endblock1.gameObject.SetActive(true);
                                    Debug.Log("Справа");
                                    Debug.Log(endblock1.transform.position);
                                    break;
                                case 2:
                                    endblock2 = gold_room.transform.Find("Grid").Find("blocks?_bot").gameObject;
                                    endblock2.gameObject.SetActive(true);
                                    Debug.Log("Снизу");
                                    Debug.Log(endblock2.transform.position);
                                    break;
                                case 3:
                                    endblock3 = gold_room.transform.Find("Grid").Find("blocks?_left").gameObject;
                                    endblock3.gameObject.SetActive(true);
                                    Debug.Log("Слева");
                                    Debug.Log(endblock3.transform.position);
                                    break;
                            }
                            Debug.Log(gold_room.transform.position + $" жопа {i}");
                        };
                    }
                    else if (i % 2 == 1)
                    {
                        hor = horLocal;
                        if (SpawnCorridor(vectors[i], publicCorridorsHorizontal[Random.Range(0, publicCorridorsHorizontal.Count)], i, vert, hor, moreCorridors) == false)
                        {
                            // Блок закрывашек
                            Debug.Log("ПРОВЕРКА!"); // ДЕБАГ2
                            switch (i)
                            {
                                case 0:
                                    endblock0 = gold_room.transform.Find("Grid").Find("blocks?_top").gameObject;
                                    endblock0.gameObject.SetActive(true);
                                    Debug.Log($"{endblock0.transform.position} Сверху");
                                    break;
                                case 1:
                                    endblock1 = gold_room.transform.Find("Grid").Find("blocks?_right").gameObject;
                                    endblock1.gameObject.SetActive(true);
                                    Debug.Log("Справа");
                                    Debug.Log(endblock1.transform.position);
                                    break;
                                case 2:
                                    endblock2 = gold_room.transform.Find("Grid").Find("blocks?_bot").gameObject;
                                    endblock2.gameObject.SetActive(true);
                                    Debug.Log("Снизу");
                                    Debug.Log(endblock2.transform.position);
                                    break;
                                case 3:
                                    endblock3 = gold_room.transform.Find("Grid").Find("blocks?_left").gameObject;
                                    endblock3.gameObject.SetActive(true);
                                    Debug.Log("Слева");
                                    Debug.Log(endblock3.transform.position);
                                    break;
                            }
                            Debug.Log(spawned_room.transform.position + $" брак {i}"); // Скорее всего проблема в том, что генератор делает 2 безуспешных спавна коридоров
                        };
                    }
                    Debug.Log("Спауню комнату в точке" + i);
                }
                else
                {
                    Debug.Log("Закончил работу!");
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
        return true;

    }


    public bool SpawnCorridor(Vector3 position, GameObject corridor, int spawnType, int farFromSpawnVert = 0, int farFromSpawnHor = 0, bool moreCorridors = false)
    // mode = 0 - рандомные коридоры, mode = 1 - фиксированные коридоры
    {

        int vertLocal = farFromSpawnVert, horLocal = farFromSpawnHor;
        if (vert > farFromSpawn || hor > farFromSpawn)
        {
            Debug.Log(position + " - Не смог заспаунить коридор, лимит, думаю что она уже " + vertLocal + " " + horLocal);
            return false;
        }

        Vector3[] vectors;
        vectors = new Vector3[4];
        Vector2 spawnblock_pos;
        GameObject blocksNot, endblock, spawnblock, closeblock;
        switch (spawnType)
        {
            case 0:
                spawnblock = corridor.transform.Find("endblock_top").gameObject;
                spawnblock_pos = spawnblock.transform.position;
                position = new Vector3(position.x, position.y + 1 / setka_size, vectors[1].z);
                break;
            case 1:
                spawnblock = corridor.transform.Find("endblock_left").gameObject;
                spawnblock_pos = spawnblock.transform.position;
                position = new Vector3(position.x - spawnblock_pos.x + 1 / setka_size, position.y - spawnblock_pos.y, vectors[1].z);
                break;
            case 2:
                spawnblock = corridor.transform.Find("endblock_top").gameObject;
                spawnblock_pos = spawnblock.transform.position;
                position = new Vector3(position.x - spawnblock_pos.x, position.y - spawnblock_pos.y - 1 / setka_size, vectors[1].z);
                break;
            case 3:
                spawnblock = corridor.transform.Find("endblock_right").gameObject;
                spawnblock_pos = spawnblock.transform.position;
                position = new Vector3(position.x - spawnblock_pos.x - 1 / setka_size, position.y - spawnblock_pos.y, vectors[1].z);
                break;
        }

        GameObject newcorridor;
        if (spawnType % 2 == 0)
        {
            newcorridor = publicCorridorsVertical[Random.Range(0, publicCorridorsVertical.Count)];
        }
        else
        {
            newcorridor = publicCorridorsHorizontal[Random.Range(0, publicCorridorsHorizontal.Count)];
        }
        switch (spawnType)
        {
            case 0:
                spawned_corridor = Instantiate(corridor, position, new Quaternion(0, 0, 0, 0)); // Заспаунить комнату 1 по заданным в начале координатам
                bad_corridors.Add(spawned_corridor);
                spawned_corridors.Add(spawned_corridor);
                bad_corridors_count++;
                endblock = spawned_corridor.transform.Find("endblock_top").gameObject;
                spawnblock = spawned_corridor.transform.Find("endblock_top").gameObject;
                if (((Random.Range(0, 2) == 1) || (spawnType == 2)) && (moreCorridors == true))
                {
                    Debug.Log("спауню еще коридор");
                    if (SpawnCorridor(endblock.transform.position, newcorridor, 0, vert, hor) == false)
                    {
                        return false;
                    };
                }
                else
                {
                    Debug.Log("спауню комнату");
                    if (SpawnRoom(endblock.transform.position, GetRandomRoom(publicRooms, publicRareRooms, publicMythicRooms, publicLegendaryRooms), 0, vert, hor) == false)
                    {
                        return false;
                    };
                }
                break;
            case 1:
                spawned_corridor = Instantiate(corridor, position, new Quaternion(0, 0, 0, 0)); // Заспаунить комнату 1 по заданным в начале координатам
                bad_corridors.Add(spawned_corridor);
                spawned_corridors.Add(spawned_corridor);
                bad_corridors_count++;
                endblock = spawned_corridor.transform.Find("endblock_right").gameObject;
                spawnblock = spawned_corridor.transform.Find("endblock_right").gameObject;
                if (((Random.Range(0, 2) == 1) || (spawnType == 3)) && (moreCorridors == true))
                {
                    Debug.Log("спауню еще коридор");
                    if (SpawnCorridor(endblock.transform.position, newcorridor, 1, vert, hor) == false)
                    {
                        return false;
                    };
                }
                else
                {
                    Debug.Log("спауню комнату");
                    if (SpawnRoom(endblock.transform.position, GetRandomRoom(publicRooms, publicRareRooms, publicMythicRooms, publicLegendaryRooms), 1, vert, hor) == false)
                    {
                        return false;
                    };
                }
                break;
            case 2:
                spawned_corridor = Instantiate(corridor, position, new Quaternion(0, 0, 0, 0)); // Заспаунить комнату 1 по заданным в начале координатам
                bad_corridors.Add(spawned_corridor);
                spawned_corridors.Add(spawned_corridor);
                bad_corridors_count++;
                endblock = spawned_corridor.transform.Find("endblock_bot").gameObject;
                spawnblock = spawned_corridor.transform.Find("endblock_bot").gameObject;
                if (((Random.Range(0, 2) == 1) || (spawnType == 0)) && (moreCorridors == true))
                {
                    Debug.Log("спауню еще коридор");
                    if (SpawnCorridor(endblock.transform.position, newcorridor, 2, vert, hor) == false)
                    {
                        return false;
                    };
                }
                else
                {
                    Debug.Log("спауню комнату");
                    if (SpawnRoom(endblock.transform.position, GetRandomRoom(publicRooms, publicRareRooms, publicMythicRooms, publicLegendaryRooms), 2, vert, hor) == false)
                    {
                        return false;
                    };
                }
                break;
            case 3:
                spawned_corridor = Instantiate(corridor, position, new Quaternion(0, 0, 0, 0)); // Заспаунить комнату 1 по заданным в начале координатам
                bad_corridors.Add(spawned_corridor);
                spawned_corridors.Add(spawned_corridor);
                bad_corridors_count++;
                endblock = spawned_corridor.transform.Find("endblock_left").gameObject;
                spawnblock = spawned_corridor.transform.Find("endblock_left").gameObject;
                if (((Random.Range(0, 2) == 1) || (spawnType == 1)) && (moreCorridors == true))
                {
                    Debug.Log("спауню еще коридор");
                    if (SpawnCorridor(endblock.transform.position, corridor, 3, vert, hor) == false)
                    {
                        return false;
                    };
                }
                else
                {
                    Debug.Log("спауню комнату");
                    if (SpawnRoom(endblock.transform.position, GetRandomRoom(publicRooms, publicRareRooms, publicMythicRooms, publicLegendaryRooms), 3, vert, hor) == false)
                    {
                        return false;
                    };
                }
                break;
        }
        for (int i = 0; i < spawned_corridors.Count - 1; i++) // ДЕБАГ, комнаты, где 2 брака, 1 брак чинит, на второй забивает
        {
            if (AllowToSpawn(spawned_corridor, spawned_corridors[i], 1) == false)
            {
                spawned_corridors.Remove(spawned_corridor);
                Destroy(spawned_corridor);
                return false;
            }
        }
        for (int i = 0; i < spawned_rooms.Count; i++) // ДЕБАГ, комнаты, где 2 брака, 1 брак чинит, на второй забивает
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


    bool AllowToSpawn(GameObject spawnedRoom, GameObject targetRoom, int mode = 0)
    {
        if ((spawnedRoom == null) || (targetRoom.name == null))
        {
            Debug.Log("Попал");
            return true;
        }
        int count = 0;
        if (mode == 1)
        {
            count = 3;
        }
        Vector3 LB0T = targetRoom.transform.Find("Grid").Find("limit_block_0").position;
        Vector3 LB1T = targetRoom.transform.Find("Grid").Find("limit_block_1").position;
        Vector3 LB2T = targetRoom.transform.Find("Grid").Find("limit_block_2").position;
        Vector3 LB3T = targetRoom.transform.Find("Grid").Find("limit_block_3").position;

        Vector3 LB0S = spawnedRoom.transform.Find("Grid").Find("limit_block_0").position;
        Vector3 LB1S = spawnedRoom.transform.Find("Grid").Find("limit_block_1").position;
        Vector3 LB2S = spawnedRoom.transform.Find("Grid").Find("limit_block_2").position;
        Vector3 LB3S = spawnedRoom.transform.Find("Grid").Find("limit_block_3").position;

        if ((LB0S.x >= LB1T.x) | (LB1S.x <= LB0T.x))
        {
            count += 2;
            //Debug.Log("1 кейс норм");
        }
        if ((LB0S.y <= LB3T.y) | (LB3S.y >= LB0T.y))
        {
            count += 2;
            //Debug.Log("2 кейс норм");
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
                //Debug.Log("спауню");
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            Debug.Log($"Ошибка спауна, хорошая комната: {spawnedRoom.transform.position} Плохая комната: {targetRoom.transform.position}");
            return false;
        }
    }

    void DisableAllColliders(List<GameObject> rooms, List<GameObject> corridors)
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
        } // Перестраховка, чтобы точно не выдавать редкие комнаты если их шанс спауна = 0

        if ((randFloat <= legendaryRoomChance) && (legendaryRoomList != null) && (legendaryRoomList.Count != 0))
        {
            returnedRoom = legendaryRoomList[Random.Range(0, legendaryRoomList.Count)];
            Debug.Log($"Спауню легендарную комнату с шансом {randFloat}");
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

    /*List<GameObject> SelectRoomPool(List<GameObject> roomList, int roomSelectCount, bool selectAgain = false) {
        roomSelectCount -= 1;
        // Не знаю почему, но метод выдает на 1 комнату больше чем положено, поэтому пока колхозное решение
        List<GameObject> localRoomList = new List<GameObject>(roomList);
        Debug.Log($"Выборка перед махинациями - {localRoomList.Count}");
        for (int i = localRoomList.Count - 1; i >= 0; i--) {
            if (localRoomList[i].GetComponent<RoomInfo>().roomType != RoomUtils.RoomTypes.unknownRoom) { 
                Debug.Log($"Тип комнаты известен! Айди {i}");
                localRoomList.RemoveAt(i);
            }
        }
        Debug.Log($"Начинаю работу с комнатами, текущая выборка - {localRoomList.Count}");

        List<GameObject> returnedRooms = new List<GameObject>();
        GameObject selectedRoom;
        for (int i = roomSelectCount - 1; i >= 0; i--) {
            Debug.Log($"После удаления объекта длина массива стала {localRoomList.Count}");
            selectedRoom = localRoomList[Random.Range(0, localRoomList.Count)];
            returnedRooms.Add(selectedRoom);
            if (selectAgain == false) {
                localRoomList.Remove(selectedRoom);
            }
        }
        return returnedRooms;
    }
    // Перенесено в утилити, позже можно удалить
    */

    bool SpawnEnemyCampsFull(List<GameObject> roomList, int roomCount)
    {
        List<GameObject> localRoomList = new List<GameObject>();
        localRoomList.AddRange(roomList);
        List<GameObject> targetRooms = RoomUtils.SelectRoomPool(localRoomList, roomCount, false);
        RoomUtils.SelectCampTypes(targetRooms, data.mediumChance, data.hardChance, data.insaneChance);
        foreach (GameObject room in targetRooms)
        {
            room.GetComponent<RoomInfo>().canSpawn = false; // В лагерях не могут спауниться рандомно монстры
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
        Debug.Log("Спаун врагов прошел успешно.");
        return true;
    }
    void Finish()
    {
        Debug.Log("Вызвал финиш!");
        DisableAllColliders(spawned_rooms, spawned_corridors);
        GameObject.Find("A*").GetComponent<AstarPath>().Scan();
        Debug.Log(spawned_rooms);
        //GetComponent<ChestSpawning>().enabled = true;
        spawned_rooms.RemoveAt(0);
        foreach (GameObject room in spawned_rooms)
        {
            room.transform.Find("FullRoom").gameObject.SetActive(true);
        }
        Debug.Log("Удаляю первую комнату");
        SpawnEnemyCampsFull(spawned_rooms, enemyCampsCount);
        RoomUtils.FullChestSpawning(spawned_rooms, RoomUtils.ChestTypes.classicChest, 5, data);
        RoomUtils.FullChestSpawning(spawned_rooms, RoomUtils.ChestTypes.classicShopCenter, 3, data);

        gameObject.SetActive(false);

    }
}
