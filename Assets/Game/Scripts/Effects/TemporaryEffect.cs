using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryEffect : Effect
{
    [Tooltip("Время действия эффекта в секундах")]
    public int effectDuration = 5;

    [Tooltip("Иконка эффекта сверху на экране (как в майнкрафте, пока в разработке)")]
    public Sprite effectIcon;

    private void Awake()
    {
        GameObject.Find("MainManager").GetComponent<EventManager>().OnEveryMicSecEvent += PerSecEvent;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PerSecEvent() { 
    }
}
