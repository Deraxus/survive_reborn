using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TestKnockbackScript : MonoBehaviour
{
    public float newAngel;
    public float tech = 1;
    public int duration;
    public float rotZ;
    float seconds;
    bool reverse = false;

    [Tooltip("Сила отдачи. Чем больше число - тем слабее отдача")]
    public float xWeapon;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.G))
        {
            if (seconds >= 0.25f) {
                reverse = true;
            }
            if (seconds <= 0)
            {
                reverse = false;
            }
            if (reverse)
            {
                seconds -= Time.deltaTime;
            }
            else {
                seconds += Time.deltaTime;
            }
            tech += 0.001f;
            //newAngel = Mathf.Lerp(0, Mathf.Sqrt(seconds), seconds / 2);
            rotZ = Mathf.Atan2(Mathf.Pow(seconds, (1/(1.5f))), xWeapon) * Mathf.Rad2Deg;
            gameObject.transform.rotation = Quaternion.Euler(0f,0f, rotZ);
        }
    }
}
