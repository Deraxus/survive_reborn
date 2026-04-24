using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player Instance;
    [Header("Основные настройки")]
    public string Name;
    public float HP = 100;
    [HideInInspector] public float MaxHP = 100;
    public float speed = 5.0f;
    public int coins = 0;

    [Header("Настройки мутации")]
    public float mutantScale = 0;

    [Tooltip("Множитель урона - урон будет умножаться на эту величину")]
    public float damageKf = 1f;

    [Tooltip("Множитель скорострельности")]
    public float rateKf = 1f;

    [Tooltip("Множитель скорости передвижения")]
    public float speedKf = 1f;

    // Когда эта переменная доходит до единицы - игроку дается 1 хп и обнуляется
    public float healthCup = 0;

    float startDamageKf;
    float startRateKf;
    float startSpeedKf;

    public Animator animator;

    private Rigidbody2D rb;
    private Vector2 vector;

    public void Awake()
    {
        Instance = this;
        MaxHP = HP;
    }

    private void Start()
    {
        startDamageKf = damageKf;
        startRateKf = rateKf;
        startSpeedKf = speedKf;
    }
    void Update()
    {
        if (HP <= 0)
        {
            Death();
        }
    }

    public void GetDamage(int damage) {
        HP -= damage;
    }

    public void Death() {
        SceneManager.LoadScene(1);
    }

    public void AfterGettingHpCup()
    {
        if (healthCup >= 1)
        {
            healthCup = 0;
            MaxHP += 1;
            HP += 1;
        }
    }

}
