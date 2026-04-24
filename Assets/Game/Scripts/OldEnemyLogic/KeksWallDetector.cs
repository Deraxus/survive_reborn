using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class KeksWallDetector : MonoBehaviour
{
    public GameObject mainEnemy;
    int count;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        count++;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "block" && count >= 10)
        {
            count = 0;
            mainEnemy.gameObject.GetComponent<MonsterMove>().mode = 1;
            Debug.Log("Стою в блоке");
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "block")
        {
            mainEnemy.gameObject.GetComponent<MonsterMove>().mode = 0;
        }
    }
}
