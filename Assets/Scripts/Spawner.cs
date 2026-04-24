using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject monster;
    private float time;
    public GameObject timer;
    private float localtimer;
    public float spawnertime;
    // Start is called before the first frame update
    void Start()
    {
        localtimer = 0;
        spawnertime = Random.Range(0.1f, 0.6f);
    }

    // Update is called once per frame

    private float generator() {
        return Random.Range(0.1f, 0.6f); ;
    }
    void Update()
    {
        time = WaveManager.Instance.timer;
        localtimer += Time.deltaTime;
        switch (time)
        {
            case < 20:
                if (localtimer > spawnertime * 10)
                {
                    localtimer = 0;
                    Instantiate(monster, transform.position, monster.GetComponent<Transform>().rotation);
                    spawnertime = generator();
                    Debug.Log("спаун глаза");
                }
                break;
                Debug.Log("работает");
            case < 60:
                if (localtimer > spawnertime * 25)
                {
                    localtimer = 0;
                    Instantiate(monster, transform.position, monster.GetComponent<Transform>().rotation);
                    spawnertime = generator();
                    Debug.Log("спаун глаза");
                }
                break;
            case < 100:
                if (localtimer > spawnertime * 40)
                {
                    localtimer = 0;
                    Instantiate(monster, transform.position, monster.GetComponent<Transform>().rotation);
                    spawnertime = generator();
                    Debug.Log("спаун глаза");
                }
                break;
            case < 180:
                if (localtimer > spawnertime * 50) {
                    localtimer = 0;
                    Instantiate(monster, transform.position, monster.GetComponent<Transform>().rotation);
                    spawnertime = generator();
                    Debug.Log("спаун глаза");
                }
                break;
        }
    }
}
