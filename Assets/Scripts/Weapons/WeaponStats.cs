using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    [SerializeField] public SWeaponStats data;


    public float bullet_damage;
    public int patrons;

    public float reload_time;
    public float rate;
    public float bullet_speed;
    // Start is called before the first frame update
    void Awake()
    {
        bullet_damage = data.bullet_damage;
        patrons = data.patrons;
        reload_time = data.reload_time;
        rate = data.rate;
        bullet_speed = data.bullet_speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
