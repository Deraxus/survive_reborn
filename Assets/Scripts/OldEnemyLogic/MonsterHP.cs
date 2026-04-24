using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHP : MonoBehaviour
{
    // Start is called before the first frame update
    public int hp;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hp == 0)
        {
            gameObject.SetActive(false);
        }
    }
}
