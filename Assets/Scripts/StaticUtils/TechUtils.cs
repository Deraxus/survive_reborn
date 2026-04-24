using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class TechUtils
{
    public static List<GameObject> GetAllChilds(GameObject parent, bool includingDeepChildren = false, bool onlyActive = true)
    {
        List<GameObject> children = new List<GameObject>();

        if (includingDeepChildren)
        {
            foreach (Transform child in parent.GetComponentsInChildren<Transform>(includeInactive: true))
            {
                if (onlyActive)
                {
                    if (child != parent.transform && child.gameObject.activeInHierarchy)
                        children.Add(child.gameObject);
                }
                else
                {
                    if (child != parent.transform)
                        children.Add(child.gameObject);
                }
            }
        }

        else
        {
            foreach (Transform child in parent.transform)
            {
                if (onlyActive && child.gameObject.activeInHierarchy)
                {
                    children.Add(child.gameObject);
                }
                else if (!onlyActive)
                {
                    children.Add(child.gameObject);
                }
            }
        }

        return children;
    }

    public static void GetFullTargetComponent(GameObject objGetScript, GameObject objSetScript, MonoBehaviour targetScript)
    {
        if (objGetScript == null || objSetScript == null || targetScript == null)
        {
            Debug.LogError("Utils.GetFullTargetComponent: один из аргументов null!");
            return;
        }

        Type scriptType = targetScript.GetType();

        // Получаем компонент с исходного объекта
        Component sourceComponent = objGetScript.GetComponent(scriptType);
        if (sourceComponent == null)
        {
            Debug.LogError("Utils.GetFullTargetComponent: компонент не найден на objGetScript");
            return;
        }

        // Добавляем компонент к целевому объекту
        Component copiedComponent = objSetScript.AddComponent(scriptType);

        // Копируем все поля
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        foreach (FieldInfo field in scriptType.GetFields(flags))
        {
            if (field.IsDefined(typeof(ObsoleteAttribute), true)) continue;
            field.SetValue(copiedComponent, field.GetValue(sourceComponent));
        }

        // Копируем свойства (если они доступны для записи)
        foreach (PropertyInfo property in scriptType.GetProperties(flags))
        {
            if (!property.CanWrite || !property.CanRead) continue;
            if (property.Name == "name") continue; // исключаем стандартные Unity-свойства

            try
            {
                property.SetValue(copiedComponent, property.GetValue(sourceComponent));
            }
            catch { /* некоторые свойства нельзя скопировать — игнорируем */ }
        }
    }

    public static void StartDropLoot(Loot lootScript)
    {

    }

    public static void SpawnRaicast(GameObject fromObject, Vector2 rayDirection)
    {
        RaycastHit2D hit = Physics2D.Raycast(fromObject.transform.position, rayDirection);
    }
}
