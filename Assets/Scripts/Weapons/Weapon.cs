using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public GameObject mainManager;
    public SWeaponStats data;

    
    public WeaponFeature[] weaponFeatures;

    public GameObject wherespawn;
    public GameObject bullet;
    private float timer;

    [HideInInspector] public GameObject hand1;

    private Vector2 position;

    private float startRate, rate;
    private Sprite panelSprite;
    private int startPatrons = 1, patrons = 1;
    private float startReload_time, reload_time;

    private GameObject Stats;

    private int old_patrons;

    private bool tog = true;

    private TMP_Text patronsText;
    private Image patronsPanelSprite;
    private Slider reloadBarSlider;

    public int faceKf = 1;
    public float offset;



    [HideInInspector] public float newAngel;
    [HideInInspector] public float tech = 1;
    [HideInInspector] public int duration;

    [HideInInspector] public float rotZ;
    float seconds;
    float techSeconds;
    bool reverse = false;

    private bool isKnockbacking = false;
    [HideInInspector] public float oldRotZ = 0;

    [HideInInspector] public float xWeapon;

    void Start()
    {
        /*Stats = GameObject.Find("Stats");
        rate = GameObject.Find("Stats").GetComponent<WeaponStats>().rate;
        patrons = GameObject.Find("Stats").GetComponent<WeaponStats>().patrons;
        reload_time = GameObject.Find("Stats").GetComponent<WeaponStats>().reload_time;
        old_patrons = GameObject.Find("Stats").GetComponent<WeaponStats>().patrons;
        Debug.Log("�����");*/
        reloadBarSlider = UI.Instance.reloadBarSlider.GetComponent<Slider>();
        startRate = data.rate;
        patrons = data.patrons;
        startReload_time = data.reload_time;
        old_patrons = data.patrons;
        panelSprite = data.panelSprite;
        xWeapon = data.xWeapon;

        patronsText = GameObject.Find("UIObject").GetComponent<UI>().patrons;
        patronsPanelSprite = GameObject.Find("UIObject").GetComponent<UI>().patronsPanelSprite;

        mainManager = GameObject.Find("MainManager");

        loadData();
        StatsUpdate();
        foreach (WeaponFeature feature in weaponFeatures)
        {
            feature.OnWeaponTaking();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.deltaTime;
        if (Input.GetMouseButton(0) && timer > Mathf.Pow(rate, -1) && patrons > 0)
        {
            timer = 0f;
            StatsUpdate();
            Shoot();
        }
        if ((patrons <= 0 && tog == true) || ((Input.GetKey("r") && tog == true) && (patrons < data.patrons)))
        {
            tog = false;
            StartCoroutine(Reload());
        }
        patronsText.text = $"{patrons} / {old_patrons}";
        if (patronsPanelSprite.sprite != panelSprite) {
            patronsPanelSprite.sprite = panelSprite;
        }

        if (isKnockbacking) {
            if (seconds >= (Mathf.Pow(rate, -1) / 4)) // если тут делим на 4, то снизу - на два, если тут на 6, то снизу - на 3
            {
                reverse = true;
            }
            if (techSeconds >= Mathf.Pow(rate, -1) / 2) // тут число должно быть примерно 2 раза меньше чем выше
            {
                isKnockbacking = false;
                techSeconds = 0;
                reverse = false;
                seconds = 0;
                rotZ = 0;
                return;
            }
            if (reverse)
            {
                seconds -= Time.deltaTime;
            }
            else
            {
                seconds += Time.deltaTime;
            }
            //newAngel = Mathf.Lerp(0, Mathf.Sqrt(seconds), seconds / 2);
            rotZ = Mathf.Atan2(Mathf.Pow(seconds, (1 / (1.25f))), xWeapon) * Mathf.Rad2Deg;

            techSeconds += Time.deltaTime;

        }

        // Строчки, которые отвечают за поворот руки персонажа к курсору
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - hand1.transform.position;
        float rotZ_mouse = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        if (faceKf == -1)
        {
            rotZ *= -1;
        }
        hand1.transform.rotation = Quaternion.Euler(0f, 0f, rotZ_mouse + rotZ + offset);

        // РЕШИТЬ БАГ С КАМЕРОЙ!!! Там изи
        // Также решить баг, что пуля вылетает не там где надо
        if ((difference.x < 0) && faceKf == 1)
        {
            offset = 180;
            MainManager.Instance.mainPlayer.transform.Find("Visuals").localScale = new Vector2(-1 * MainManager.Instance.mainPlayer.transform.Find("Visuals").localScale.x, MainManager.Instance.mainPlayer.transform.Find("Visuals").transform.localScale.y);
            faceKf = -1;
        }
        else if ((difference.x > 0) && faceKf == -1)
        {
            offset = 0;
            MainManager.Instance.mainPlayer.transform.Find("Visuals").transform.localScale = new Vector2(-1 * MainManager.Instance.mainPlayer.transform.Find("Visuals").localScale.x, MainManager.Instance.mainPlayer.transform.Find("Visuals").transform.localScale.y);
            faceKf = 1;
        }

        if (tog == false && reloadBarSlider != null)
        {
            if (reloadBarSlider.gameObject.activeInHierarchy == false)
            {
                reloadBarSlider.gameObject.SetActive(true);
            }
            reloadBarSlider.maxValue = reload_time;
            reloadBarSlider.value += Time.fixedDeltaTime;
        }
    }

    void Shoot()
    {
        patrons -= 1;
        //Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        //float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        //Debug.Log("�����");
        //Instantiate(bullet, wherespawn.transform.position, Quaternion.Euler(0f, 0f, rotZ)).GetComponent<bullet>().enabled = true;
        //GameObject spawnedBullet = Instantiate(bullet, wherespawn.transform.position, Quaternion.Euler(0f, 0f, rotZ));
        GameObject spawnedBullet = Instantiate(bullet, wherespawn.transform.position, transform.rotation);
        Bullet localBulletScript = spawnedBullet.GetComponentInChildren<Bullet>();

        localBulletScript.faceKf = faceKf;
        localBulletScript.data = data;
        localBulletScript.weapon = gameObject;
        float randRecoilKf = Random.Range(data.weaponRecoil * -1 / MainItemManager.Instance.GetModify(ItemManager.ModifyTypes.RecoilKf), data.weaponRecoil / MainItemManager.Instance.GetModify(ItemManager.ModifyTypes.RecoilKf));
        float randRecoilKf2 = Random.Range(data.weaponRecoil * -1 / MainItemManager.Instance.GetModify(ItemManager.ModifyTypes.RecoilKf), data.weaponRecoil / MainItemManager.Instance.GetModify(ItemManager.ModifyTypes.RecoilKf)); ;
        localBulletScript.randRecoilKf = randRecoilKf;
        localBulletScript.randRecoilKf2 = randRecoilKf2;
        localBulletScript.direction = position;

        localBulletScript.weapon = gameObject;
        oldRotZ = (float)hand1.transform.rotation.eulerAngles.z;
        isKnockbacking = true;
        techSeconds = 0;
        reverse = false;
        seconds = 0;
        
        EventManager.Instance.OnShoot(spawnedBullet);
    }

    public void RateUp(float kf) {
        rate = rate * kf;
    }

    public void RateDown(float kf) {
        rate = rate / kf;
    }

    void StatsUpdate() {
        GameObject player = GameObject.Find("Player");
        rate = startRate * player.GetComponent<Player>().rateKf;
        reload_time = startReload_time;
    }

    IEnumerator Reload()
    {
        Debug.Log("�����������");
        patrons = 0;
        GameObject.Find("Player").gameObject.GetComponent<MainInventory>().canSwap = false;
        
        EventManager.Instance.OnReload(bullet);
        
        yield return new WaitForSecondsRealtime(reload_time);
        patrons = old_patrons;
        tog = true;
        GameObject.Find("Player").gameObject.GetComponent<MainInventory>().canSwap = true;
        if (reloadBarSlider != null)
        {
            reloadBarSlider.gameObject.SetActive(false);
            reloadBarSlider.value = 0;
        }
    }

    public virtual void loadData()
    {
        
    }
}
