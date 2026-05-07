using System.Collections;
using UnityEngine;

public class PlayerIFrames : MonoBehaviour
{
    public GameObject visualPartBody;
    public float iframesDuration = 3f;
    
    public bool iframesNow = false;
    public float timer = 0f;
    void Start()
    {
        EventManager.Instance.OnPlayerDamagedEvent += OnPlayerDamaged;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
    }

    void OnPlayerDamaged(GameObject enemy, float damage = 0)
    {
        print($"damage by {enemy.name}");
        if (!iframesNow)
        {
            StartCoroutine(StartIFrames(enemy, damage));   
        }
    }
    
    IEnumerator StartIFrames(GameObject enemy, float damage = 0)
    {
        SpriteRenderer[] spriteRenderers;
        spriteRenderers = visualPartBody.GetComponentsInChildren<SpriteRenderer>();
        Player.Instance.GetDamage((int)damage);
        timer = 0f;
        iframesNow = true;
        while (timer < iframesDuration)
        {
            foreach (SpriteRenderer localSr in spriteRenderers)
            {
                localSr.enabled = !localSr.enabled;
            }
            yield return new WaitForSeconds(0.25f);
        }
        foreach (SpriteRenderer localSr in spriteRenderers)
        {
            localSr.enabled = true;
        }
        iframesNow = false;
    }
}
