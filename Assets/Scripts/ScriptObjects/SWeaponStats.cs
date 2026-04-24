using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu()]
public class SWeaponStats : ScriptableObject
{
    [SerializeField] public float bullet_damage = 5;
    [SerializeField] public int patrons = 10;

    [SerializeField] public Sprite bulletSprite;

    [SerializeField] public float reload_time = 3.0f;
    [SerializeField] public float rate = 5.0f; // ��������� � �������
    [SerializeField] public float bullet_speed = 1f;
    [SerializeField] public float bulletLiveTime = 1f;
    [SerializeField] public float weaponRecoil = 0f;
    [SerializeField] public int weaponCount = 1;

    [Tooltip("Отдача оружия, то насколько оно подпрыгивает при стрельбе. Чем меньше - тем сильнее отдача")]
    [SerializeField] public float xWeapon = 0.5f;

    [SerializeField] public Sprite panelSprite;
    public List<AudioClip> shotSounds;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
