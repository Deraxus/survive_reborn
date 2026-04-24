using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEngine.RuleTile.TilingRuleOutput;
public static class Utils
{
    private static bool techBool;
    public enum ItemTypes {passive, weapon, consumables, building };
    public enum RareTypes {common, rare, mythic, legendary};

    public enum ColorTags { blue, green, red, yellow};

    public static RareTypes GetRandomRareType(float rareChance = 0.2f, float mythicChance = 0.025f, float legendaryChance = 0.005f) {
        float randFloat = UnityEngine.Random.Range(0f, 1f);
        if (randFloat < legendaryChance)
        {
            return RareTypes.legendary;
        }
        if (randFloat < mythicChance)
        {
            return RareTypes.mythic;
        }
        if (randFloat < rareChance)
        {
            return RareTypes.rare;
        }
        return RareTypes.common;
    }

    public static Sprite SelectRandomSprite(List<Sprite> spriteList) {
        return spriteList[UnityEngine.Random.Range(0, spriteList.Count)];
    }

    public static Vector3 GetNearbyCell(GameObject target) {
        Vector3 returnedVector = new Vector3();
        returnedVector.x = Mathf.Round(target.transform.position.x) - 0.5f;
        returnedVector.y = Mathf.Round(target.transform.position.y) - 0.5f;
        return returnedVector;
    }

    public static GameObject SpawnTarget(GameObject target, Vector3 whereSpawn) {
        return GameObject.Instantiate(target, whereSpawn, Quaternion.identity);
    }

    /*public static bool DetectBlockAvailable(Vector3 position, GameObject detectBlock) {
        GameObject.Find("Player").GetComponent<Player>().StartCoroutine(CDetectBlockAvailable(position, detectBlock, CDetectBlockAvailableTech));
        int attemps = 0;
        while (attemps <= 1000) {
            attemps++;
        }
        if (techBool)
        {
            return true;
        }
        else {
            return false;
        }
    }*/
    public static IEnumerator CDetectBlockAvailable(Vector3 position, GameObject detectBlock, System.Action<bool> callback) {
        GameObject localDetectBlock = GameObject.Instantiate(detectBlock, position, Quaternion.identity);
        int attemps = 0;
        yield return new WaitForSeconds(0.05f);
        callback(!localDetectBlock.GetComponentInChildren<DetectBlockLogic>().isTouched);
    }

    public static List<int> GetListRandomNumbers(int min, int max, int numbersCount) {
        if (numbersCount > (min + max)) {
            return null;
        }
        List<int> intList = new List<int>();
        while (intList.Count != numbersCount) { 
            int targetInt = UnityEngine.Random.Range(min, max);
            if (!intList.Contains(targetInt)) { 
                intList.Add(targetInt);
            }
        }
        return intList;
    }

    public static void StartNewMessage(string message, string description = null)
    {
        UIManager localUI = UIManager.Instance;
        localUI.StartNewMessage(message, description);
    }

    /*static void CDetectBlockAvailableTech(bool callbackBool) {
        if (callbackBool)
        {
            techBool = true;
        }
        else {
            techBool = false;
        }
    }*/
    private static CoroutineManager localCoroutineManager;
    public static void InitializeCoroutine(CoroutineManager coroutineManager)
    {
        localCoroutineManager = coroutineManager;
    }
    public static void StartGlobalCoroutine(IEnumerator coroutine)
    {
        localCoroutineManager.StartGlobalCoroutine(coroutine);
    }

    public static void TeleportEntity(float x, float y, GameObject entity)
    {
        entity.GetComponent<Rigidbody2D>().position = new Vector2(x, y);
    }

    public static void DamageEnemy(GameObject enemy, float damage)
    {
        EventManager.Instance.OnEnemyDamage(enemy, damage);
        enemy.GetComponent<EnemyLogic>().HP -= damage;
    }
}
