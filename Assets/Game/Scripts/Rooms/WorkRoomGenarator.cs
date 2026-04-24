using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomGenerator_work : MonoBehaviour
{
    public List<GameObject> publicRooms = new List<GameObject>();
    private GameObject spawned_room;
    public List<GameObject> spawned_rooms = new List<GameObject>();
    public GameObject publicRoom, publicRoom2, publicCorridorVertical, publicCorridorHorizontal;
    public int roomLimit = 50, farFromSpawn = 5;
    public float setka_size = 4;
    int vert, hor;
    private int roomCounter;
    public int[] limits = new int[4];
    private List<Vector2> quests = new List<Vector2>();
    public List<GameObject> rooms = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        SpawnRoom(new Vector2(46, 50), publicRooms[0], 4, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnRoom(Vector3 position, GameObject room, int spawnType, int farFromSpawnVer, int farFromSpawnHor)
    {
        int dodelivaem = 0;
        Vector3[] vectors;
        vectors = new Vector3[4];
        Vector2 spawnblock_pos;
        GameObject blocksNot, endblock, spawnblock;
        Debug.Log(vert + " " + hor);
        switch (spawnType)
        {
            case 0:
                vert += 1;

                break;
            case 1:
                spawnblock = room.transform.Find("Grid").Find("spawnblock_left").gameObject;
                spawnblock_pos = spawnblock.transform.position;
                position = new Vector3(position.x - spawnblock_pos.x, position.y - spawnblock_pos.y, vectors[1].z);
                hor += 1;
                break;

            case 2:
                spawnblock = room.transform.Find("Grid").Find("spawnblock_top").gameObject;
                spawnblock_pos = spawnblock.transform.position;
                position = new Vector3(position.x - spawnblock_pos.x, position.y - spawnblock_pos.y, vectors[1].z);
                vert += 1;

                break;
            case 3:
                spawnblock = room.transform.Find("Grid").Find("spawnblock_right").gameObject;
                spawnblock_pos = spawnblock.transform.position;
                position = new Vector3(position.x - spawnblock_pos.x, position.y - spawnblock_pos.y, vectors[1].z);
                hor += 1;
                break;
            case 4:

                break;
        }
        int vertLocal = vert, horLocal = hor;
        if (Mathf.Abs(vert) > farFromSpawn || Mathf.Abs(hor) > farFromSpawn)
        {
            Debug.Log(position + " - Не смог заспаунить, лимит, думаю что она уже " + vert + " " + hor);
            return;
        }
        spawned_room = Instantiate(room, position, new Quaternion(0, 0, 0, 0)); // Заспаунить комнату 1 по заданным в начале координатам
        for(int i = 0; i < spawned_rooms.Count; i++)
        {
            if (AllowToSpawn(spawned_room, spawned_rooms[i]) == false)
            {
                Debug.Log("НЕЛЬЗЯ!!!");
                Debug.Log(spawned_rooms[i].transform.position);
                Destroy(spawned_room);
                dodelivaem = 1;
                return;
            }
        }
        Debug.Log(spawned_room.transform.Find("FullRoom").gameObject.GetComponent<RoomCollision>().tog);
        spawned_rooms.Add(spawned_room);
        int[] roomTypes = new int[4];
        for (int i = 0; i <= 3; i = i + 1)
        {
            switch (i)
            {
                case 0:
                    blocksNot = spawned_room.transform.Find("Grid").Find("blocks?_top").gameObject; // Получаем ссылку на спавнблок в новом объекте
                    endblock = spawned_room.transform.Find("Grid").Find("endblock_top").gameObject;
                    spawnblock = spawned_room.transform.Find("Grid").Find("spawnblock_top").gameObject;
                    if (((Random.Range(0, 2) == 0) || (spawnType == 2)))
                    {
                        blocksNot.gameObject.SetActive(false);
                        endblock.gameObject.SetActive(true);
                        spawnblock.gameObject.SetActive(true);
                        if (spawnType != 2)
                        {
                            vectors[0] = endblock.transform.position;
                        }
                        roomTypes[0] = 0;
                    }
                    break;
                case 1:
                    blocksNot = spawned_room.transform.Find("Grid").Find("blocks?_right").gameObject; // Получаем ссылку на спавнблок в новом объекте
                    endblock = spawned_room.transform.Find("Grid").Find("endblock_right").gameObject;
                    spawnblock = spawned_room.transform.Find("Grid").Find("spawnblock_right").gameObject;
                    if (((Random.Range(0, 2) == 0) || (spawnType == 3)))
                    {
                        blocksNot.gameObject.SetActive(false);
                        endblock.gameObject.SetActive(true);
                        spawnblock.gameObject.SetActive(true);
                        if (spawnType != 3)
                        {
                            vectors[1] = endblock.transform.position;
                        }
                        roomTypes[1] = 1;
                    }
                    break;
                case 2:
                    blocksNot = spawned_room.transform.Find("Grid").Find("blocks?_bot").gameObject; // Получаем ссылку на спавнблок в новом объекте
                    endblock = spawned_room.transform.Find("Grid").Find("endblock_bot").gameObject;
                    spawnblock = spawned_room.transform.Find("Grid").Find("spawnblock_bot").gameObject;
                    if (((Random.Range(0, 2) == 0) || (spawnType == 0)))
                    {
                        blocksNot.gameObject.SetActive(false);
                        endblock.gameObject.SetActive(true);
                        spawnblock.gameObject.SetActive(true);
                        if (spawnType != 0)
                        {
                            vectors[2] = endblock.transform.position;
                        }
                        roomTypes[2] = 2;
                    }
                    break;
                case 3:
                    blocksNot = spawned_room.transform.Find("Grid").Find("blocks?_left").gameObject; // Получаем ссылку на спавнблок в новом объекте
                    endblock = spawned_room.transform.Find("Grid").Find("endblock_left").gameObject;
                    spawnblock = spawned_room.transform.Find("Grid").Find("spawnblock_left").gameObject;
                    if (((Random.Range(0, 2) == 0) || (spawnType == 1)))
                    {
                        blocksNot.gameObject.SetActive(false);
                        endblock.gameObject.SetActive(true);
                        spawnblock.gameObject.SetActive(true);
                        if (spawnType != 1)
                        {
                            vectors[3] = endblock.transform.position;
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
                            blocksNot = spawned_room.transform.Find("Grid").Find("blocks?_top").gameObject; // Получаем ссылку на спавнблок в новом объекте
                            endblock = spawned_room.transform.Find("Grid").Find("endblock_top").gameObject;
                            spawnblock = spawned_room.transform.Find("Grid").Find("spawnblock_top").gameObject;
                            blocksNot.gameObject.SetActive(true);
                        }
                        break;
                    case 1:
                        if (spawnType != 3)
                        {
                            blocksNot = spawned_room.transform.Find("Grid").Find("blocks?_right").gameObject; // Получаем ссылку на спавнблок в новом объекте
                            endblock = spawned_room.transform.Find("Grid").Find("endblock_right").gameObject;
                            spawnblock = spawned_room.transform.Find("Grid").Find("spawnblock_right").gameObject;
                            blocksNot.gameObject.SetActive(true);
                        }
                        break;
                    case 2:
                        if (spawnType != 0)
                        {
                            blocksNot = spawned_room.transform.Find("Grid").Find("blocks?_bot").gameObject; // Получаем ссылку на спавнблок в новом объекте
                            endblock = spawned_room.transform.Find("Grid").Find("endblock_bot").gameObject;
                            spawnblock = spawned_room.transform.Find("Grid").Find("spawnblock_bot").gameObject;
                            blocksNot.gameObject.SetActive(true);
                        }
                        break;
                    case 3:
                        if (spawnType != 1)
                        {
                            blocksNot = spawned_room.transform.Find("Grid").Find("blocks?_left").gameObject; // Получаем ссылку на спавнблок в новом объекте
                            endblock = spawned_room.transform.Find("Grid").Find("endblock_left").gameObject;
                            spawnblock = spawned_room.transform.Find("Grid").Find("spawnblock_left").gameObject;
                            blocksNot.gameObject.SetActive(true);
                        }
                        break;
                }
            }
            return;
        }
        for (int i = 0; i <= vectors.Length - 1; i++)
        {
            Vector3 vr_vector = new Vector3(0f, 0f, 0f);
            if (vectors[i] != vr_vector)
            {
                if (roomCounter <= roomLimit)
                {
                    if (i % 2 == 0 && vertLocal <= farFromSpawn)
                    {
                        vert = vertLocal;
                        SpawnCorridor(vectors[i], publicCorridorVertical, i, vert, hor);
                    }
                    else if (i % 2 == 1 && horLocal <= farFromSpawn)
                    {
                        hor = horLocal;
                        SpawnCorridor(vectors[i], publicCorridorHorizontal, i, vert, hor);
                    }
                    Debug.Log("Спауню комнату в точке" + i);
                }
                else
                {
                    Debug.Log("Закончил работу!");
                }
            }
        }
        return;

    }

    void SpawnCorridor(Vector3 position, GameObject corridor, int spawnType, int farFromSpawnVert = 0, int farFromSpawnHor = 0)
    {

        int vertLocal = farFromSpawnVert, horLocal = farFromSpawnHor;
        if (vert > farFromSpawn || hor > farFromSpawn)
        {
            Debug.Log(position + " - Не смог заспаунить коридор, лимит, думаю что она уже " + vertLocal + " " + horLocal);
            return;
        }
        Vector3[] vectors;
        vectors = new Vector3[4];
        Vector2 spawnblock_pos;
        GameObject blocksNot, endblock, spawnblock;
        switch (spawnType)
        {
            case 0:
                spawnblock = corridor.transform.Find("Grid").Find("spawnblock_top").gameObject;
                spawnblock_pos = spawnblock.transform.position;
                position = new Vector3(position.x, position.y + 0.25f, vectors[1].z);
                break;
            case 1:
                spawnblock = corridor.transform.Find("Grid").Find("spawnblock_right").gameObject;
                spawnblock_pos = spawnblock.transform.position;
                position = new Vector3(position.x + 0.25f, position.y, vectors[1].z);
                break;
            case 2:
                spawnblock = corridor.transform.Find("Grid").Find("spawnblock_top").gameObject;
                spawnblock_pos = spawnblock.transform.position;
                position = new Vector3(position.x, position.y - spawnblock_pos.y - 0.25f, vectors[1].z);
                break;
            case 3:
                spawnblock = corridor.transform.Find("Grid").Find("spawnblock_right").gameObject;
                spawnblock_pos = spawnblock.transform.position;
                position = new Vector3(position.x - spawnblock_pos.x - 0.25f, position.y, vectors[1].z);
                break;
        }
        switch (spawnType)
        {
            case 0:
                spawned_room = Instantiate(corridor, position, new Quaternion(0, 0, 0, 0)); // Заспаунить комнату 1 по заданным в начале координатам
                endblock = spawned_room.transform.Find("Grid").Find("endblock_top").gameObject;
                spawnblock = spawned_room.transform.Find("Grid").Find("spawnblock_top").gameObject;
                if ((Random.Range(0, 2) == 1) || (spawnType == 2))
                {
                    Debug.Log("спауню еще коридор");
                    SpawnCorridor(endblock.transform.position, corridor, 0, vert, hor);
                }
                else
                {
                    Debug.Log("спауню комнату");
                    SpawnRoom(endblock.transform.position, publicRooms[Random.Range(0, publicRooms.Count)], 0, vert, hor);
                }
                break;
            case 1:
                spawned_room = Instantiate(corridor, position, new Quaternion(0, 0, 0, 0)); // Заспаунить комнату 1 по заданным в начале координатам
                endblock = spawned_room.transform.Find("Grid").Find("endblock_right").gameObject;
                spawnblock = spawned_room.transform.Find("Grid").Find("spawnblock_right").gameObject;
                if ((Random.Range(0, 2) == 1) || (spawnType == 3))
                {
                    Debug.Log("спауню еще коридор");
                    SpawnCorridor(endblock.transform.position, corridor, 1, vert, hor);
                }
                else
                {
                    Debug.Log("спауню комнату");
                    SpawnRoom(endblock.transform.position, publicRooms[Random.Range(0, publicRooms.Count)], 1, vert, hor);
                }
                break;
            case 2:
                spawned_room = Instantiate(corridor, position, new Quaternion(0, 0, 0, 0)); // Заспаунить комнату 1 по заданным в начале координатам
                endblock = spawned_room.transform.Find("Grid").Find("endblock_bot").gameObject;
                spawnblock = spawned_room.transform.Find("Grid").Find("spawnblock_bot").gameObject;
                if ((Random.Range(0, 2) == 1) || (spawnType == 0))
                {
                    Debug.Log("спауню еще коридор");
                    SpawnCorridor(endblock.transform.position, corridor, 2, vert, hor);
                }
                else
                {
                    Debug.Log("спауню комнату");
                    SpawnRoom(endblock.transform.position, publicRooms[Random.Range(0, publicRooms.Count)], 2, vert, hor);
                }
                break;
            case 3:
                spawned_room = Instantiate(corridor, position, new Quaternion(0, 0, 0, 0)); // Заспаунить комнату 1 по заданным в начале координатам
                endblock = spawned_room.transform.Find("Grid").Find("endblock_left").gameObject;
                spawnblock = spawned_room.transform.Find("Grid").Find("spawnblock_left").gameObject;
                if ((Random.Range(0, 2) == 1) || (spawnType == 1))
                {
                    Debug.Log("спауню еще коридор");
                    SpawnCorridor(endblock.transform.position, corridor, 3, vert, hor);
                }
                else
                {
                    Debug.Log("спауню комнату");
                    SpawnRoom(endblock.transform.position, publicRooms[Random.Range(0, publicRooms.Count)], 3, vert, hor);
                }
                break;
        }
        return;
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
