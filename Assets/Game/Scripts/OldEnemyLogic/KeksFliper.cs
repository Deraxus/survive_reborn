using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class KeksFliper : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    int count = 0;
    public int kekspos = 0;
    public GameObject mainEnemy;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        count++;
        if (count >= 30 && mainEnemy.gameObject.GetComponent<MonsterMove>().mode == 0)
        {
            count = 0;
            Vector3 difference = player.transform.position - transform.position;
            float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            if (rotZ < 22.5 && rotZ > 0)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0);
                kekspos = 0;
            }
            else if (rotZ > 22.5 && rotZ < 45)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0);
                kekspos = 0;
            }
            else if (rotZ > 45 && rotZ < 67.5)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 90);
                kekspos = 90;
            }
            else if (rotZ > 67.5 && rotZ < 90)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 90);
                kekspos = 90;
            }
            else if (rotZ > 90 && rotZ < 112.5)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 90);
                kekspos = 90;
            }
            
            else if (rotZ > 112.5 && rotZ < 135)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 90);
                kekspos = 90;
            }
            else if (rotZ > 135 && rotZ < 157.5)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 180);
                kekspos = 180;
            }
            
            else if (rotZ > 157.5 && rotZ < 180)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 180);
                kekspos = 180;
            }
            else if (rotZ > 180 && rotZ < 202.5)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 180);
                kekspos = 180;
            }
            else if (rotZ < -157.5 && rotZ > -179.999999)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 180);
                kekspos = 180;
            }
            
            else if (rotZ < -135 && rotZ > -157.5)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 180);
                kekspos = 180;
            }
            else if (rotZ < -112.5 && rotZ > -135)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, -90);
                kekspos = 270;
            }
            else if (rotZ < -90 && rotZ > -112.5)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, -90);
                kekspos = 270;
            }
            else if (rotZ < -67.5 && rotZ > -90)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, -90);
                kekspos = 270;
            }
            
            else if (rotZ < -45 && rotZ > -67.5)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, -90);
                kekspos = 270;
            }
            else if (rotZ < -22.5 && rotZ > -45)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0);
                kekspos = 0;
            }
            else if (rotZ > -22.5 && rotZ < 0)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0);
                kekspos = 0;
            }
        }
    }
}
