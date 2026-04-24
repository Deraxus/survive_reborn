using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class KeksSecondFliper : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public GameObject keks;
    public GameObject rotator;
    int count = 0;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(keks.gameObject.GetComponent<KeksFliper>().kekspos);
        switch (keks.gameObject.GetComponent<KeksFliper>().kekspos) { 
            case 0:
                if (keks.gameObject.GetComponent<KeksSecondWallDetector>().rotation == 0)
                {
                    transform.rotation = Quaternion.Euler(0f, 0f, 90);
                }
                else if (keks.gameObject.GetComponent<KeksSecondWallDetector>().rotation == 2)
                {
                    transform.rotation = Quaternion.Euler(0f, 0f, -90);
                }
                break;
            case 45:
                if (keks.gameObject.GetComponent<KeksSecondWallDetector>().rotation == 0)
                {
                    transform.rotation = Quaternion.Euler(0f, 0f, 90);
                }
                else if (keks.gameObject.GetComponent<KeksSecondWallDetector>().rotation == 2)
                {
                    transform.rotation = Quaternion.Euler(0f, 0f, -90);
                }
                break;
            case 90:
                transform.rotation = Quaternion.Euler(0f, 0f, 0);
                break;
            case 135:
                transform.rotation = Quaternion.Euler(0f, 0f, 0);
                break;
            case 180:
                transform.rotation = Quaternion.Euler(0f, 0f, 90);
                break;
            case 225:
                transform.rotation = Quaternion.Euler(0f, 0f, 90);
                break;
            case 270:
                transform.rotation = Quaternion.Euler(0f, 0f, 0);
                break;
            case 315:
                transform.rotation = Quaternion.Euler(0f, 0f, 0);
                break;
        }

    }
}
