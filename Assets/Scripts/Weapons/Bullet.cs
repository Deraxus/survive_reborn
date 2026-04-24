using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject bulletLine;
    public GameObject bulletLight;
    public GameObject weapon;
    public GameObject wherespawn;
    public float kf = 1;

    public Sprite bulletSprite;

    public SWeaponStats data;

    public Vector2 direction;
    
    public bool isBounced = false;
    public int bouceLimit = 5;

    public float startBullet_damage, bullet_damage = 1;
    public float startBullet_speed, bullet_speed;

    public int bounceCount = 0;
    public float bulletLiveTime;
    public float randRecoilKf, randRecoilKf2 = 0f;


    [HideInInspector] public int faceKf = 1;
    // ������� ������ �������� - � ����������� ���� ������� ���� ��� ������
    private Vector2 shootVector;
    private Transform playerTransform;
    private float timer;

    private void Awake()
    {
        playerTransform = MainManager.Instance.transform;
    }
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bulletLine = GameObject.FindGameObjectWithTag("bulletLine");
        rb.linearVelocity = shootVector;
        bulletLiveTime = data.bulletLiveTime;

        startBullet_speed = weapon.GetComponent<Weapon>().data.bullet_speed;
        startBullet_damage = weapon.GetComponent<Weapon>().data.bullet_damage;
        GetComponent<SpriteRenderer>().sprite = weapon.GetComponent<Weapon>().data.bulletSprite;

        StatsUpdate();

        shootVector = transform.right * bullet_speed * kf * faceKf;
        shootVector.y += randRecoilKf;
        shootVector.x += randRecoilKf2;
        shootVector = shootVector.normalized * (bullet_speed * kf * playerTransform.localScale.x);
        rb.linearVelocity = shootVector;
    }

    void FixedUpdate()
    {
        StatsUpdate();
        timer += Time.fixedDeltaTime;
        if (timer >= bulletLiveTime)
        {
            GameObject.Destroy(transform.parent.gameObject);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ����� ����� - � ���������
        //SpawnBulletLight(bulletLight, transform.position);
        switch (collision.collider.tag) {
            case "block":
                if (isBounced == false)
                {
                    GameObject.Destroy(transform.parent.gameObject);
                    //gameObject.SetActive(false);
                }
                else if (isBounced == true) 
                {
                    BounceBullet(collision);
                }
                break;
            case "chest":
                GameObject.Destroy(transform.parent.gameObject); break;
            case "Enemy":
                DamageEnemy(collision.gameObject, bullet_damage);
                break;
            case "box":
                DamageBox(collision.gameObject, bullet_damage);
                break;
        }
    }

    void BounceBullet(Collision2D collision) {
        bounceCount++;
        var firstContact = collision.contacts[0];
        Vector2 newVelocity = Vector2.Reflect(shootVector, firstContact.normal);
        newVelocity = newVelocity.normalized;
        shootVector = newVelocity * bullet_speed * kf * Time.fixedDeltaTime;
        rb.linearVelocity = shootVector;
        //transform.right = transform.right * -1;
        if (bounceCount >= bouceLimit) {
            isBounced = false;
        }
    }

    void StatsUpdate() {
        GameObject player = Player.Instance.gameObject;
        bullet_damage = startBullet_damage * player.GetComponent<Player>().damageKf;
        bulletLiveTime = data.bulletLiveTime * MainItemManager.Instance.GetModify(ItemManager.ModifyTypes.BulletLTKf);
        bullet_speed = startBullet_speed;
    }
    private void DamageEnemy(GameObject enemy, float damage) {
        Utils.DamageEnemy(enemy, damage);
        GameObject.Destroy(transform.parent.gameObject);
        GameObject.Destroy(transform.parent.gameObject);
    }
    private void DamageBox(GameObject box, float damage)
    {
        box.gameObject.GetComponent<BoxLogic>().HP -= damage;
        GameObject.Destroy(transform.parent.gameObject);
    }

    private void SpawnBulletLight(GameObject lightObj, Vector2 pos) {
        Quaternion qtr = transform.rotation;
        //Instantiate(lightObj, pos, qtr).GetComponentInChildren<BulletLightLogic>().parentLight = GetComponentInChildren<Light2D>();
        GameObject SpawnedLight = Instantiate(lightObj, pos, qtr);
        SpawnedLight.GetComponentInChildren<BulletLightLogic>().enabled = true;
        SpawnedLight.GetComponentInChildren<Light2D>().overlapOperation = Light2D.OverlapOperation.Additive;
        //SpawnedLight.GetComponentInChildren<Light2D>().intensity = SpawnedLight.GetComponentInChildren<Light2D>().intensity / 2;
    }
}
