using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastShot : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject wherespawn;
    public GameObject bullet;
    public float timer;

    private float rate;
    public int patrons = 10;
    public float reload_time;

    private GameObject Stats;

    private int old_patrons;
    private float bullet_damage;
    private bool tog = true;
    void Start()
    {
        /*Stats = GameObject.Find("Stats");
        rate = GameObject.Find("Stats").GetComponent<WeaponStats>().rate;
        patrons = GameObject.Find("Stats").GetComponent<WeaponStats>().patrons;
        reload_time = GameObject.Find("Stats").GetComponent<WeaponStats>().reload_time;
        old_patrons = GameObject.Find("Stats").GetComponent<WeaponStats>().patrons;
        Debug.Log("СТАРТ");*/
        rate = GetComponentInParent<WeaponStats>().rate;
        patrons = GetComponentInParent<WeaponStats>().patrons;
        reload_time = GetComponentInParent<WeaponStats>().reload_time;
        old_patrons = GetComponentInParent<WeaponStats>().patrons;
        bullet_damage = GetComponentInParent<WeaponStats>().bullet_damage;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (Input.GetMouseButton(0) && timer >= Mathf.Pow(rate, -1) && patrons > 0)
        {
            timer = 0f;
            Shot();
        }
        if ((patrons <= 0 && tog == true) || (Input.GetKey("r") && tog == true))
        {
            tog = false;
            Debug.Log("Начинаю перезарядку");
            StartCoroutine(Reload());
        }

    }

    void ShotOld()
    {
        patrons -= 1;
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        Debug.Log("Спавн");
        //Instantiate(bullet, wherespawn.transform.position, Quaternion.Euler(0f, 0f, rotZ)).GetComponent<bullet>().enabled = true;
        Instantiate(bullet, wherespawn.transform.position, Quaternion.Euler(0f, 0f, rotZ));
    }

    IEnumerator Reload()
    {
        Debug.Log("перезарядка");
        patrons = 0;
        GameObject.Find("Player").gameObject.GetComponent<MainInventory>().canSwap = false;
        yield return new WaitForSecondsRealtime(reload_time);
        patrons = old_patrons;
        tog = true;
        GameObject.Find("Player").gameObject.GetComponent<MainInventory>().canSwap = true;
    }

    void Shot()
    {
        patrons -= 1;
        Ray2D ray = new Ray2D(transform.position, transform.right * 100);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right * 100);
        if (hit.transform.tag == "Enemy") {
            Debug.Log("Попал во врага");
            DamageEnemy(hit.transform.gameObject, bullet_damage);
        }
        Debug.DrawRay(transform.position, transform.right * 100, Color.red);
    }
    private void DamageEnemy(GameObject enemy, float damage)
    {
        enemy.gameObject.GetComponent<EnemyLogic>().HP -= damage;
    }
}
