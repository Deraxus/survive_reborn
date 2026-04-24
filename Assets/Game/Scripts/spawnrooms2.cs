using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnrooms2 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject start_room; // комната для спауна 1
    public GameObject start_room2; // комната для спауна 2
    private Vector2 vector;
    private Quaternion rotation;
    public Vector3 pos;
    public GameObject spawned_room;
    public GameObject spawned_room2;
    GameObject PrefabObj;
    GameObject spawn;
    GameObject end;
    public Vector3 endpos;
    void Start()
    {
        vector = new Vector2(23, 55);
        rotation = new Quaternion(0, 0, 0, 0);
    }

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            spawned_room = Instantiate(start_room, vector, rotation); // Заспаунить комнату 1 по заданным в начале координатам
            spawn = spawned_room.transform.Find("Grid").Find("spawnblock1").gameObject; // Получаем ссылку на спавнблок в новом объекте
            end = spawned_room.transform.Find("Grid").Find("endblock3").gameObject; // Получаем ссылку на эндблок в новом объекте
            endpos = end.transform.position; // Получаем позицию эндблока
            endpos = new Vector3(endpos.x, endpos.y + 0.25f, endpos.z); // смещаем на 0.25 вверх
            vector = endpos; // задаем новые координаты для следующего спавна
            // Вопрос открытый - сможет ли код работать в глобальных координатах?
            Debug.Log(endpos.ToString());
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            spawned_room2 = Instantiate(start_room2, vector, rotation);
            spawn = spawned_room2.transform.Find("Grid").Find("spawnblock2").gameObject;
            end = spawned_room2.transform.Find("Grid").Find("endblock4").gameObject;
            endpos = end.transform.position;
            endpos = new Vector3(endpos.x, endpos.y, endpos.z);
            vector = endpos;
            Debug.Log(endpos.ToString());
        }
    }
}
