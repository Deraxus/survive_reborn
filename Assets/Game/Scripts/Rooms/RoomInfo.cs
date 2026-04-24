using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInfo : MonoBehaviour
{
    public RoomUtils.RoomTypes roomType = RoomUtils.RoomTypes.defaultRoom;
    public RoomUtils.CampDifficults campDifficult;
    public RoomUtils.CampFillings campFilling;
    public RoomUtils.RoomSizes roomSize;

    public Utils.RareTypes roomRare = Utils.RareTypes.common;

    [Tooltip("≈сли галочка стоит - спаунеры врагов будут там же, где и сундуки")]
    public bool ChestSpawnersAreEnemySpawners;

    public int spawnersCount;
    public List<GameObject> enemySpawners = new List<GameObject>();
    public List<GameObject> lootSpawners = new List<GameObject>();

    public Utils.ColorTags lootSpawnersTag = Utils.ColorTags.green;
    public Utils.ColorTags enemySpawnersTag = Utils.ColorTags.red;
    public int spawnTimer = 5;

    public Vector2 side0;
    public Vector2 side1;
    public Vector2 side2;
    public Vector2 side3;

    public Vector2 topCorridorLeftPoint;
    public Vector2 topCorridorRightPoint;

    public Vector2 rightCorridorTopPoint;
    public Vector2 rightCorridorBotPoint;

    public Vector2 botCorridorLeftPoint;
    public Vector2 botCorridorRightPoint;

    public Vector2 leftCorridorTopPoint;
    public Vector2 leftCorridorBotPoint;

    public bool canSpawn = false;

    // явл€етс€ ли комната боковой. Ќужна дл€ заполнени€ уровн€
    public bool sideRoom = false;

    // Update is called once per frame
    void Update()
    {
        
    }

    void FillAllSpawners()
    {
        List<GameObject> allSpawners = new List<GameObject>();
        List<GameObject> redSpawners = new List<GameObject>();
        List<GameObject> greenSpawners = new List<GameObject>();
        List<GameObject> blueSpawners = new List<GameObject>();
        List<GameObject> yellowSpawners = new List<GameObject>();
        allSpawners = TechUtils.GetAllChilds(transform.Find("RoomSpawners").gameObject);
        Debug.Log($"228 {allSpawners.Count}");
        foreach (GameObject spawner in allSpawners)
        {
            Debug.Log($"229{spawner.name.Split("_")[0]}");
            switch (spawner.name.Split("_")[0])
            {
                case "red":
                    redSpawners.Add(spawner);
                    break;
                case "green":
                    greenSpawners.Add(spawner);
                    break;
                case "blue":
                    blueSpawners.Add(spawner);
                    break;
                case "yellow":
                    yellowSpawners.Add(spawner);
                    break;
            }
        }

        foreach (GameObject spawner in allSpawners)
        {
            switch (lootSpawnersTag)
            {
                case Utils.ColorTags.red:
                    if (redSpawners.Contains(spawner))
                    {
                        lootSpawners.Add(spawner);
                    }
                    break;
                case Utils.ColorTags.green:
                    if (greenSpawners.Contains(spawner))
                    {
                        Debug.Log($"230 {spawner.name} {gameObject.name}");
                        lootSpawners.Add(spawner);
                    }
                    break;
                case Utils.ColorTags.blue:
                    if (blueSpawners.Contains(spawner))
                    {
                        lootSpawners.Add(spawner);
                    }
                    break;
                case Utils.ColorTags.yellow:
                    if (yellowSpawners.Contains(spawner))
                    {
                        lootSpawners.Add(spawner);
                    }
                    break;
            }

            switch (enemySpawnersTag)
            {
                case Utils.ColorTags.red:
                    if (redSpawners.Contains(spawner))
                    {
                        enemySpawners.Add(spawner);
                    }
                    break;
                case Utils.ColorTags.green:
                    if (greenSpawners.Contains(spawner))
                    {
                        enemySpawners.Add(spawner);
                    }
                    break;
                case Utils.ColorTags.blue:
                    if (blueSpawners.Contains(spawner))
                    {
                        enemySpawners.Add(spawner);
                    }
                    break;
                case Utils.ColorTags.yellow:
                    if (yellowSpawners.Contains(spawner))
                    {
                        enemySpawners.Add(spawner);
                    }
                    break;
            }
        }
    }

    // ћетод вызываетс€ после полного построени€ комнаты
    public void AfterFullSpawn()
    {
        FillAllSpawners();
        Debug.Log("методы");
    }
}
