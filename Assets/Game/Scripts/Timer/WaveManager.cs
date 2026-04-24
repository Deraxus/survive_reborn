using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class WaveManager : MonoBehaviour
{
    [Header("Основные настройки")]

    public bool canSpawnEnemy = true;

    [Tooltip("Сколько минут будет длиться забег")]
    public int minutesDifficult = 5;

    [Tooltip("Переодичность появления монстров в секундах на первой сложности")]
    public float EnemySpawnPeriod = 5;

        [Tooltip("Сколько максимум может быть врагов на карте. Если их слишком много - спаун временно прекращается чтобы не душить игрока")]
    public int maxEnemyCount = 20;

    [Header("Варианты врагов, которые могут появиться в забеге")]
    public List<GameObject> publicCommonEnemyes;
    public List<GameObject> publicRareEnemyes;
    public List<GameObject> publicMythicEnemyes;
    public List<GameObject> publicLegendaryEnemyes;
    public List<GameObject> publicBosses;

    [Header("Технические поля")]
    [Tooltip("ѕеременна€ текста, которая отображает время до победы")]
    public TMP_Text timerText;

    [Tooltip("Переменная, которая отвечает на вопрос можно ли сейчас закончить игру и победить")]
    public bool canEscape = false;

    private int microCount = 0;
    private int microCount2 = 0;

    public List<GameObject> spawnedEnemyes;

    private List<GameObject> spawnedRooms;
    public float timer;
    float timerReverse = 0;
    private bool isOneMinuteLeft = false;
    private float StartEnemySpawnPeriod;
    public int difficult = 1; // 1 - начало, самый лайт, 2 - сложнее, 3 - т€желее, 4 - хард, 5 - максимум

    // Активируется, когда игрок выйдет со стартового коридора
    public bool gameStarted = false;

    public static WaveManager Instance;

    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        EventManager.Instance.OnEverySecEvent += TestFunc;

        timer = minutesDifficult * 60;
        StartEnemySpawnPeriod = EnemySpawnPeriod;
        //text.text = timer.ToString();
        spawnedRooms = RoomGeneratorNew.Instance.gameObject.GetComponent<RoomGeneratorNew>().spawned_rooms;

        if (publicLegendaryEnemyes.Count == 0) {
            publicLegendaryEnemyes.AddRange(publicCommonEnemyes);
        }
        if (publicMythicEnemyes.Count == 0)
        {
            publicMythicEnemyes.AddRange(publicCommonEnemyes);
        }
        if (publicRareEnemyes.Count == 0)
        {
            publicRareEnemyes.AddRange(publicCommonEnemyes);
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameStarted)
        {

        }
        switch (difficult)
        {
            case 1:
                EnemySpawnPeriod = StartEnemySpawnPeriod * 1f;
                break;
            case 2:
                EnemySpawnPeriod = StartEnemySpawnPeriod * 0.85f;
                break;
            case 3:
                EnemySpawnPeriod = StartEnemySpawnPeriod * 0.70f;
                break;
            case 4:
                EnemySpawnPeriod = StartEnemySpawnPeriod * 0.50f;
                break;
            case 5:
                EnemySpawnPeriod = StartEnemySpawnPeriod * 0.25f;
                break;
        }

        if (timer >= 0)
        {
            timer -= Time.fixedDeltaTime;
        }

        timerReverse += Time.fixedDeltaTime;
        //float round_timer = Mathf.Round(timer);
        //text.text = round_timer.ToString();
        if ((timer <= 0) && (canEscape == false)) {
            Utils.StartNewMessage("<color=white>Зачистка завершена!", "Теперь вы можете сбежать из подземелья, для этого найдите лестницу в одной из комнат");
            canEscape = true;

        }

        if (timer < (minutesDifficult * 60 * 0.8f) && (difficult == 1))
        {
            //Utils.StartNewMessage("Смена сложности!");
            difficult = 2;
        }
        else if (timer < (minutesDifficult * 60 * 0.6f) && (difficult == 2))
        {
            //Utils.StartNewMessage("Смена сложности!");
            difficult = 3;
        }
        else if (timer < (minutesDifficult * 60 * 0.4f) && (difficult == 3))
        {
            //Utils.StartNewMessage("Смена сложности!");
            difficult = 4;
        }
        else if (timer < (minutesDifficult * 60 * 0.2f) && (difficult == 4))
        {
            //Utils.StartNewMessage("Смена сложности!");
            difficult = 5;
        }


        if (timerReverse >= 0.1f) {
            OneMicSecond();
            timerReverse = 0;
        }

    }

    bool SpawnEnemy(GameObject room, GameObject enemy) {
        int randCount = room.gameObject.GetComponent<RoomInfo>().enemySpawners.Count;
        randCount = Random.Range(0, randCount);
        GameObject spawner = RoomUtils.GetRandomSpawners(room, 1)[0];
        Vector3 pos = spawner.gameObject.transform.position;
        Instantiate(enemy, pos, new Quaternion(0, 0, 0, 0));
        return true;
    }

    GameObject GetRandomRoom(List<GameObject> rooms) {
        int len = rooms.Count;
        int randCount = Random.Range(0, len);
        return rooms[randCount];
    }
    GameObject GetRandomEnemy(List<GameObject> enemyes)
    {
        int len = enemyes.Count;
        int randCount = Random.Range(0, len);
        return enemyes[randCount];
    }

    void FullSpawnEnemy(int difficult = 1) {
        GameObject targetRoom = RoomUtils.GetRandomRoom(spawnedRooms);
        int attemps = 0;
        while (attemps <= 1000) {
            targetRoom = RoomUtils.GetRandomRoom(spawnedRooms);
            if (targetRoom.GetComponent<RoomInfo>().canSpawn) {
                break;
            }
            attemps++;
        }
        GameObject targetSpawner = RoomUtils.GetRandomSpawners(targetRoom, 1)[0];
        GameObject targetEnemy = null;
        switch (Utils.GetRandomRareType()) {
            case Utils.RareTypes.legendary:
                targetEnemy = publicLegendaryEnemyes[Random.Range(0, publicLegendaryEnemyes.Count)];
                break;
            case Utils.RareTypes.mythic:
                targetEnemy = publicMythicEnemyes[Random.Range(0, publicMythicEnemyes.Count)];
                break;
            case Utils.RareTypes.rare:
                targetEnemy = publicRareEnemyes[Random.Range(0, publicRareEnemyes.Count)];
                break;
            case Utils.RareTypes.common:
                targetEnemy = publicCommonEnemyes[Random.Range(0, publicCommonEnemyes.Count)];
                break;

        }
        if (canSpawnEnemy)
        {
            GameObject spawnedEnemy = Utils.SpawnTarget(targetEnemy, targetSpawner.transform.position);
            WaveManager.Instance.spawnedEnemyes.Add(spawnedEnemy);
            spawnedEnemy.GetComponentInChildren<EnemyLogic>().enemySpawnType = EnemyLogic.enemySpawnTypes.wild;
        }
    }

    void OneMicSecond() {
        microCount += 1;
        microCount2 += 1;
        if ((microCount / 10) >= EnemySpawnPeriod)
        {
            Debug.Log("ЅјЌјЌ");
            if (spawnedEnemyes.Count < maxEnemyCount)
            {
                FullSpawnEnemy();
            }
            microCount = 0;
        }

        if (timer <= 60 && !isOneMinuteLeft)
        {
            UIManager.Instance.StartNewMessage("<color=white>До открытия люка осталась 1 минута!");
            isOneMinuteLeft = true;
        }
    }

    void TestFunc() {
        Debug.Log("ЋќЎј–ј!");
    }
}
