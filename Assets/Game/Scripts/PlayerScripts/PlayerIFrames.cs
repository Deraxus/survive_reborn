using System.Collections;
using UnityEngine;

public class PlayerIFrames : MonoBehaviour
{
    public float iframesDuration = 3f;
    void Start()
    {
        EventManager.Instance.OnPlayerDamagedEvent += OnPlayerDamaged;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnPlayerDamaged(GameObject enemy, float damage = 0)
    {
        
    }
}
