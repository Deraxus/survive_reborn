using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fullLaserLogic : MonoBehaviour
{
    public GameObject startLaser;
    public GameObject midLaser;
    public GameObject endLaser;

    public bool oneMidSpriteLaser = true;

    public float laserDistance;
    public GameObject player;
    void Start()
    {
        player = MainManager.Instance.mainPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PrepareLaser(Vector2 startLaserPos)
    {
        transform.position = startLaserPos;
        Vector3 difference = MainManager.Instance.mainPlayer.transform.position - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
    }
}
