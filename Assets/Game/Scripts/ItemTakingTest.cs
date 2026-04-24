using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NUnit.Framework.Constraints;
using System.Reflection;

public class ItemTakingTest : MonoBehaviour
{

    // Update is called once per frame

    public void OnPassiveItemTaking(GameObject item)
    {
        GameObject ItemManager = GameObject.Find("ItemManager");
        string scriptName = item.name.Replace("_sprite", "") + "Item";
        Debug.Log("Буду искать скрипт: " + scriptName);

        foreach (MonoBehaviour component in ItemManager.GetComponents<MonoBehaviour>())
        {
            if (component.GetType().Name == scriptName)
            {
                Component newComponent = ItemManager.AddComponent(component.GetType());

                FieldInfo[] fieldNew = component.GetType().GetFields();
                FieldInfo[] fieldOld = newComponent.GetType().GetFields();

                PropertyInfo[] propertyNew = component.GetType().GetProperties();
                PropertyInfo[] propertyOld = newComponent.GetType().GetProperties();
                for (int i = 0; i < newComponent.GetType().GetFields().Length; i++)
                {
                    fieldNew[i].SetValue(newComponent, fieldOld[i].GetValue(component));
                }
                for (int i = 0; i < newComponent.GetType().GetProperties().Length; i++)
                {
                    propertyNew[i].SetValue(newComponent, propertyOld[i].GetValue(component));
                }
                Debug.Log($"Добавил {component.GetType()}");
                break;
            }
        }
    }
    public void DestroyGameObject(GameObject obj)
    {
        GameObject.Destroy(obj);
    }
    // Ближайшая задача - упростить процесс подбора предметов, сделать так чтобы несколько универсальных скриптов за это отвечало
    // Бомба почти готова, нужно избавить ее от скрипта ExplosionsSprite
}
