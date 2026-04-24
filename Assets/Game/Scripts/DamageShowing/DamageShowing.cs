using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class DamageShowing : MonoBehaviour
{
    public float forceCount = 5;
    public float damageCount = 0;
    public float counterLifeTime = 1f;
    public float delayFading = 1;
    private float timer, timer2;
    public int prevDir = 1;

    [HideInInspector] public Canvas canvas;
    private Rigidbody2D rb;
    void Start()
    {
        forceCount = 3.5f + Mathf.Pow(damageCount * 0.2f, 1/1.5f);
        //counterLifeTime = 0.5f + counterLifeTime * damageCount * 0.05f;
        delayFading = delayFading + delayFading * Mathf.Pow(damageCount, 1/1.5f) * 0.15f;
        canvas = GetComponentInParent<Canvas>();
        canvas.worldCamera = GameObject.Find("MainManager").GetComponent<MainManager>().mainCamera;
        rb = GetComponent<Rigidbody2D>();

        rb.AddForce(new Vector2(prevDir, forceCount), ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timer2 >= delayFading)
        {
            timer += Time.fixedDeltaTime;
            GetComponent<TMP_Text>().color = Color.Lerp(new Color(GetComponent<TMP_Text>().color.r, GetComponent<TMP_Text>().color.g, GetComponent<TMP_Text>().color.b, 1), new Color(GetComponent<TMP_Text>().color.r, GetComponent<TMP_Text>().color.g, GetComponent<TMP_Text>().color.b, 0), timer / counterLifeTime);

        }

        if ((timer / counterLifeTime) >= 1)
        {
            Destroy(transform.parent.transform.parent.gameObject);
        }
        timer2 += Time.fixedDeltaTime;
    }
}
