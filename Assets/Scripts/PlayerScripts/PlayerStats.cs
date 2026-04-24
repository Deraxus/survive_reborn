using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] SPlayerStats data;

    public float HP;
    public float speed;
    
    void Start()
    {
        HP = data.HP;
        speed = data.speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
