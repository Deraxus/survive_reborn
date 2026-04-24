using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.U2D;

public class BulletLightLogic : MonoBehaviour
{
    public float fadeTime = 3;
    public Light2D parentLight;

    float currentTime;
    float startTime;
    float startLightValue = 1;
    float startInnerValue;
    float startOuterValue;
    Light2D light;
    void Start()
    {
        light = GetComponent<Light2D>();
        startLightValue = light.intensity;
        startInnerValue = light.pointLightInnerRadius;
        startOuterValue = light.pointLightOuterRadius;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = Time.time - startTime;
        light.intensity = Mathf.Lerp(startLightValue, 0, currentTime / fadeTime);
        //light.pointLightInnerRadius = Mathf.Lerp(startInnerValue, 0, currentTime / fadeTime);
        //light.pointLightOuterRadius = Mathf.Lerp(startOuterValue, 0, currentTime / fadeTime);
        if (light.intensity == 0) {
            //Destroy(gameObject);
        }
        Debug.Log(currentTime);
    }
}
