using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UIElements;

public class ChestSpawning : MonoBehaviour
{
    public float chestSpawnChance = 0.1f;


    public List<GameObject> chestList = new List<GameObject>();
    public List<GameObject> spawnedRooms;
    private List<GameObject> lootSpawnersList;
    // Start is called before the first frame update
    void OnEnable()
    {
        spawnedRooms = GameObject.Find("RoomGenerator").gameObject.GetComponent<RoomGenerator>().spawned_rooms;
        FullChestSpawn(spawnedRooms.Count);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FullChestSpawnOld() {
        for (int i = 0; i <= spawnedRooms.Count - 1; i++)
        {
            lootSpawnersList = spawnedRooms[i].GetComponent<RoomInfo>().lootSpawners;
            for (int j = 0; j <= lootSpawnersList.Count - 1; j++) {
                int randint = Random.Range(0, 101);
                if (randint <= chestSpawnChance * 100) {
                    SpawnChest(chestList[0], lootSpawnersList[j]);
                }
            }
        }
    }

    void FullChestSpawn(int roomCount) {
        int ChestCount = 0;
        ChestCount = (roomCount / 3) + 1; // Формула вычисления количества сундуков
        for (int i = 0;i <= ChestCount;i++) {
            GameObject randomChest = chestList[0]; // В будущем изменить, пока 1 сундук
            GameObject randomRoom = GetRandomRoom(spawnedRooms);
            GameObject randomSpawner = GetRandomChestSpawner(randomRoom);
            SpawnChest(randomChest, randomSpawner);
            Destroy(randomSpawner);

        }
    }

    GameObject GetRandomChestSpawner(GameObject room) {
        lootSpawnersList = room.GetComponent<RoomInfo>().lootSpawners;
        int randInt = Random.Range(0, lootSpawnersList.Count);
        return lootSpawnersList[randInt];
    }

    GameObject GetRandomRoom(List<GameObject> spawnedRooms) { 
        int randIndex = Random.Range(0, spawnedRooms.Count);
        return spawnedRooms[randIndex];
    }
    void SpawnChest(GameObject chest, GameObject spawner) {
        Instantiate(chest, spawner.transform.position, chest.GetComponent<Transform>().rotation);
    }
}
