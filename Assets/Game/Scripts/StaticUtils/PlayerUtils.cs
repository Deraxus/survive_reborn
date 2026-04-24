using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Различные методы для работы с игроком
// Любые действия с игроком лучше выносить в отдельные методы, так как в будущем логика будет расширяться
// Пример - хил персонажа. В будущем появится анимация хила
public static class PlayerUtils
{
    public static void HealPlayer(int hpRegenCount ,GameObject player = null)
    {
        GameObject localPlayer;
        if (player != null)
        {
            localPlayer = player;
        }
        else
        {
            localPlayer = MainManager.Instance.mainPlayer;
        }

        int newPlayerHP = (int)localPlayer.GetComponent<Player>().HP + hpRegenCount;
        if (newPlayerHP > localPlayer.GetComponent<Player>().MaxHP)
        {
            localPlayer.GetComponent<Player>().HP = localPlayer.GetComponent<Player>().MaxHP;
        }
        else
        {
            localPlayer.GetComponent<Player>().HP = newPlayerHP;
        }
    }

    public static void DamagePlayer(float damage)
    {
        MainManager.Instance.mainPlayer.GetComponent<Player>().HP -= damage;
    }
    public static void GiveMoney(int amount, GameObject player = null)
    {
        GameObject localPlayer;
        if (player != null)
        {
            localPlayer = player;
        }
        else
        {
            localPlayer = GameObject.Find("MainManager").GetComponent<MainManager>().mainPlayer;
        }

        localPlayer.GetComponent<Player>().coins += amount;
    }
}
