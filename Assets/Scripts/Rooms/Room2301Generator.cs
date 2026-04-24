using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Room2301Generator : MonoBehaviour
{
    public List<GameObject> publicRooms = new List<GameObject>();
    public List<GameObject> publicCorridorsVertical = new List<GameObject>();
    public List<GameObject> publicCorridorsHorizontal = new List<GameObject>();
    public GameObject spawned_room, last_room, spawned_corridor;
    public List<GameObject> spawned_rooms = new List<GameObject>();
    public List<GameObject> bad_corridors = new List<GameObject>();
    public List<GameObject> spawned_corridors = new List<GameObject>();
    public int roomLimit = 50, farFromSpawn = 5;
    public int roomLimitMin = 20;
    private int bad_corridors_count;
    public float setka_size = 4;
    int vert, hor;
    public int roomCounter;
    public int[] limits = new int[4];
    private List<Vector2> quests = new List<Vector2>();
    public List<GameObject> rooms = new List<GameObject>();
    public int count;
    // Start is called before the first frame update
    void Start()
    {
        SpawnRoom(new Vector2(46, 50), publicRooms[0], 4, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(RoomsCollision(spawned_rooms, spawned_corridors));
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
            SpawnRoom(new Vector2(46, 50), publicRooms[0], 4, 0, 0);
        }
        else {
            Debug.Log(roomCounter + " каунтер");
        }
        count += 1;
        if (count > 10000) {
            gameObject.GetComponent<RoomGenerator>().enabled = false;
        }
    }

    bool RoomsCollision2(List<GameObject> rooms, List<GameObject> corridors)
    {
        for (int i = 0; i <= corridors.Count - 1; i++)
        {
            try
            {
                GameObject fullCorridor = corridors[i].transform.Find("FullRoom").gameObject;
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
                GameObject fullRoom = rooms[i].transform.Find("FullRoom").gameObject;
                if (fullRoom.GetComponent<RoomCollision>().tog == 0)
                {
                    return false;
                }
                else
                {
                    Debug.Log("tog " + fullRoom.GetComponent<RoomCollision>().tog);
                    Debug.Log("проблем нет");
                }
            }
            catch { }
        }
        for (int i = 0; i <= corridors.Count - 1; i++)
        {
            try
            {
                GameObject fullCorridor = corridors[i].transform.Find("FullRoom").gameObject;
                if (fullCorridor.GetComponent<RoomCollision>().tog == 0)
                {
                    return false;
                }
                else
                {
                    Debug.Log("проблем нет");
                }
            }
            catch
            {

            }
        }
        return true;

    }

    bool RoomsCollision(List<GameObject> rooms, List<GameObject> corridors) {
        for (int i = 0; i <= rooms.Count - 1; i++) {
            try
            {
                GameObject fullRoom = rooms[i].transform.Find("FullRoom").gameObject;
                fullRoom.gameObject.SetActive(true);
            }
            catch { 
            }
        }
        return true;

    }

    bool SpawnRoom(Vector3 position, GameObject room, int spawnType, int farFromSpawnVer, int farFromSpawnHor)
    {
        int dodelivaem = 0;
        Vector3[] vectors;
        vectors = new Vector3[4];
        Vector2 spawnblock_pos;
        GameObject blocksNot0, endblock0, spawnblock0;
        GameObject blocksNot1, endblock1, spawnblock1;
        GameObject blocksNot2, endblock2, spawnblock2;
        GameObject blocksNot3, endblock3, spawnblock3;
        Debug.Log(vert + " " + hor);
        switch (spawnType)
        {
            case 0:
                vert += 1;
                spawnblock0 = room.transform.Find("endblock_top").gameObject;
                spawnblock_pos = spawnblock0.transform.position;
                position = new Vector3(position.x, position.y + 1 / setka_size, vectors[1].z);
                break;
            case 1:
                hor += 1;
                spawnblock1 = room.transform.Find("endblock_left").gameObject;
                spawnblock_pos = spawnblock1.transform.position;
                position = new Vector3(position.x - spawnblock_pos.x, position.y - spawnblock_pos.y, vectors[1].z);
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
        int vertLocal = vert, horLocal = hor;
        if (Mathf.Abs(vert) > farFromSpawn || Mathf.Abs(hor) > farFromSpawn)
        {
            Debug.Log(position + " - Не смог заспаунить, лимит, думаю что она уже " + vert + " " + hor);
            return false;
        }
        GameObject gold_room = spawned_room;
        spawned_room = Instantiate(room, position, new Quaternion(0, 0, 0, 0)); // Заспаунить комнату 1 по заданным в начале координатам
        for(int i = 0; i < spawned_rooms.Count; i++) // ДЕБАГ, комнаты, где 2 брака, 1 брак чинит, на второй забивает
        {
            if (AllowToSpawn(spawned_room, spawned_rooms[i]) == false)
            {
                Debug.Log(spawned_rooms[i].transform.position);
                Destroy(spawned_room);
                Debug.Log($"Хорошая комната - {gold_room.transform.position}, не смог заспаунить - {spawned_room.transform.position}. SpawnType - {spawnType}");
                spawned_room = gold_room;
                for (int g = 0; g < bad_corridors.Count; g++) {
                    Destroy(bad_corridors[g]);
                }
                Debug.Log($"");
                return false;
            }
        }

        gold_room = spawned_room;
        bad_corridors.Clear();
        spawned_rooms.Add(spawned_room);
        int[] roomTypes = new int[4];
        for (int i = 0; i <= 3; i = i + 1)
        {
            switch (i)
            {
                case 0:
                    blocksNot0 = spawned_room.transform.Find("Grid").Find("blocks?_top").gameObject; // Получаем ссылку на спавнблок в новом объекте
                    endblock0 = spawned_room.transform.Find("endblock_top").gameObject;
                    spawnblock0 = spawned_room.transform.Find("endblock_top").gameObject;
                    if (((Random.Range(0, 2) == 0) || (spawnType == 2)))
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
                    blocksNot1 = spawned_room.transform.Find("Grid").Find("blocks?_right").gameObject; // Получаем ссылку на спавнблок в новом объекте
                    endblock1 = spawned_room.transform.Find("endblock_right").gameObject;
                    spawnblock1 = spawned_room.transform.Find("endblock_right").gameObject;
                    if (((Random.Range(0, 2) == 0) || (spawnType == 3)))
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
                    blocksNot2 = spawned_room.transform.Find("Grid").Find("blocks?_bot").gameObject; // Получаем ссылку на спавнблок в новом объекте
                    endblock2 = spawned_room.transform.Find("endblock_bot").gameObject;
                    spawnblock2 = spawned_room.transform.Find("endblock_bot").gameObject;
                    if (((Random.Range(0, 2) == 0) || (spawnType == 0)))
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
                    blocksNot3 = spawned_room.transform.Find("Grid").Find("blocks?_left").gameObject; // Получаем ссылку на спавнблок в новом объекте
                    endblock3 = spawned_room.transform.Find("endblock_left").gameObject;
                    spawnblock3 = spawned_room.transform.Find("endblock_left").gameObject;
                    if (((Random.Range(0, 2) == 0) || (spawnType == 1)))
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
        }
        roomCounter += 1;
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
                            endblock0 = spawned_room.transform.Find("endblock_top").gameObject;
                            spawnblock0 = spawned_room.transform.Find("endblock_top").gameObject;
                            blocksNot0.gameObject.SetActive(true);
                        }
                        break;
                    case 1:
                        if (spawnType != 3)
                        {
                            blocksNot1 = spawned_room.transform.Find("Grid").Find("blocks?_right").gameObject; // Получаем ссылку на спавнблок в новом объекте
                            endblock1 = spawned_room.transform.Find("endblock_right").gameObject;
                            spawnblock1 = spawned_room.transform.Find("endblock_right").gameObject;
                            blocksNot1.gameObject.SetActive(true);
                        }
                        break;
                    case 2:
                        if (spawnType != 0)
                        {
                            blocksNot2 = spawned_room.transform.Find("Grid").Find("blocks?_bot").gameObject; // Получаем ссылку на спавнблок в новом объекте
                            endblock2 = spawned_room.transform.Find("endblock_bot").gameObject;
                            spawnblock2 = spawned_room.transform.Find("endblock_bot").gameObject;
                            blocksNot2.gameObject.SetActive(true);
                        }
                        break;
                    case 3:
                        if (spawnType != 1)
                        {
                            blocksNot3 = spawned_room.transform.Find("Grid").Find("blocks?_left").gameObject; // Получаем ссылку на спавнблок в новом объекте
                            endblock3 = spawned_room.transform.Find("endblock_left").gameObject;
                            spawnblock3 = spawned_room.transform.Find("endblock_left").gameObject;
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
            Vector3 vr_vector = new Vector3(0f, 0f, 0f);
            if (vectors[i] != vr_vector)
            {
                if (roomCounter <= roomLimit)
                {
                    if (i % 2 == 0)
                    {
                        vert = vertLocal;
                        if (SpawnCorridor(vectors[i], publicCorridorsVertical[Random.Range(0, publicCorridorsVertical.Count)], i, vert, hor) == false) {
                            Debug.Log("");
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
                        if (SpawnCorridor(vectors[i], publicCorridorsHorizontal[Random.Range(0, publicCorridorsHorizontal.Count)], i, vert, hor) == false) {
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
        for (int i = 0; i <= vectors.Length - 1; i++) {
            if (vectors[i] == vr_vector2)
            {
                lim += 1;
            }
        }
        return true;

    }


    bool SpawnCorridor(Vector3 position, GameObject corridor, int spawnType, int farFromSpawnVert = 0, int farFromSpawnHor = 0)
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
                spawnblock = corridor.transform.Find("endblock_right").gameObject;
                spawnblock_pos = spawnblock.transform.position;
                position = new Vector3(position.x + 1/setka_size, position.y, vectors[1].z);
                break;
            case 2:
                spawnblock = corridor.transform.Find("endblock_top").gameObject;
                spawnblock_pos = spawnblock.transform.position;
                position = new Vector3(position.x, position.y - spawnblock_pos.y - 1 / setka_size, vectors[1].z);
                break;
            case 3:
                spawnblock = corridor.transform.Find("endblock_right").gameObject;
                spawnblock_pos = spawnblock.transform.position;
                position = new Vector3(position.x - spawnblock_pos.x - 1/setka_size, position.y, vectors[1].z);
                break;
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
                if ((Random.Range(0, 2) == 1) || (spawnType == 2))
                {
                    Debug.Log("спауню еще коридор");
                    if (SpawnCorridor(endblock.transform.position, corridor, 0, vert, hor) == false)
                    {
                        return false;
                    };
                }
                else
                {
                    Debug.Log("спауню комнату");
                    if (SpawnRoom(endblock.transform.position, publicRooms[Random.Range(0, publicRooms.Count)], 0, vert, hor) == false) {
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
                if ((Random.Range(0, 2) == 1) || (spawnType == 3))
                {
                    Debug.Log("спауню еще коридор");
                    if (SpawnCorridor(endblock.transform.position, corridor, 1, vert, hor) == false) {
                        return false;
                    };
                }
                else
                {
                    Debug.Log("спауню комнату");
                    if (SpawnRoom(endblock.transform.position, publicRooms[Random.Range(0, publicRooms.Count)], 1, vert, hor) == false) {
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
                if ((Random.Range(0, 2) == 1) || (spawnType == 0))
                {
                    Debug.Log("спауню еще коридор");
                    if (SpawnCorridor(endblock.transform.position, corridor, 2, vert, hor) == false)
                    {
                        return false;
                    };
                }
                else
                {
                    Debug.Log("спауню комнату");
                    if (SpawnRoom(endblock.transform.position, publicRooms[Random.Range(0, publicRooms.Count)], 2, vert, hor) == false){
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
                if ((Random.Range(0, 2) == 1) || (spawnType == 1))
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
                    if (SpawnRoom(endblock.transform.position, publicRooms[Random.Range(0, publicRooms.Count)], 3, vert, hor) == false) {
                        return false;
                    };
                }
                break;
        }
        return true;
    }


    bool AllowToSpawn(GameObject spawnedRoom, GameObject targetRoom)
    {
        int count = 0;
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
            Debug.Log("1 кейс норм");
        }
        if ((LB0S.y <= LB3T.y) | (LB3S.y >= LB0T.y))
        {
            count += 2;
            Debug.Log("2 кейс норм");
        }
        /*if (limitBlock0_spawned == limitBlock0_target && limitBlock1_spawned == limitBlock1_target && limitBlock2_spawned == limitBlock2_target && limitBlock3_spawned == limitBlock3_target) {
            
            return false;
        }*/
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
        else {
            count += 1;
        }
        if (spawnedRoom.transform.position == targetRoom.transform.position)
        {
            count = count;
        }
        if (count >= 3)
        {
            Debug.Log("спауню");
            return true;
        }
        else {
            Debug.Log(spawnedRoom.transform.position + " " + targetRoom.transform.position);
            return false; }
    }

}
