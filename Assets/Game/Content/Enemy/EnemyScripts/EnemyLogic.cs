using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyLogic : MonoBehaviour
{
    public string debugVar;

    public enum enemyStates { idle, patrol, returning, attack };
    public enum enemySpawnTypes { wild, camping }

    [SerializeField] private SEnemyStats data;

    // Подгружается из дата-файла, но можно поменять в инспекторе
    [Header("Характеристики врага")]
    public float HP = 0;
    public float damage = 0;
    public float speed = 0;
    public bool statShaking = true;
    private AIPath aipath;
    [HideInInspector()]
    public float maxHP = 0;

    [Space]
    public bool canAttack = true;
    public enemySpawnTypes enemySpawnType;

    [Tooltip("Коэфицент, который влияет на разброс характеристик каждого конкретного моба, чем меньше - тем больше разброс")]
    public int koef = 2;

    [Tooltip("Объект, отвечающий за зону видимости врага")]
    public GameObject detectObject;

    [Tooltip("Объект, к которому будет возвращаться враг если он в режиме лагеря")]
    public GameObject homeObject;

    public IEnemyState currentEnemyState;

    [Header("Списки атак врага")]
    //public List<EnemyBaseAttack> meleeEnemyAttacks; - неактуально, заполнять через AttackState
    //public List<EnemyBaseAttack> otherEmenyAttacks; - неактуально, заполнять через AttackState

    public AttackState attackState;
    public PatrolState patrolState;
    public ReturningState returningState;

    [Space]
    public bool IsDead = false;

    [HideInInspector]
    private GameObject player;
    private Rigidbody2D rb;
    [HideInInspector] public bool IsBlock = false;
    [HideInInspector] public bool isAttack = false;
    [HideInInspector] public bool isMoving = true;

    public delegate void EventHandler(GameObject enemy);
    public delegate void ChangeStatePatrol(GameObject enemy);
    public delegate void ChangeStateReturning(GameObject enemy);
    public delegate void ChangeStateAttack(GameObject enemy);

    public event EventHandler OnEnemyDied;
    public event ChangeStatePatrol OnChangingPatrol;
    public event ChangeStateReturning OnChangingReturning;
    public event ChangeStateAttack OnChangingAttack;

    private GameObject Player;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = MainManager.Instance.mainPlayer;


        if (HP == 0) 
        {
            HP = data.HP;
        }
        if (damage == 0) 
        {
            damage = data.Damage;
        }
        if (speed == 0) 
        {
            speed = data.speed;
        }
        aipath = GetComponent<AIPath>();
        GetComponent<AIPath>().canSearch = true;
        GetComponent<AIPath>().repathRate = 0.5f;

        maxHP = HP;
    }

    private void Start()
    {
        if (statShaking)
        {
            HP = UnityEngine.Random.Range((HP - (HP / koef)), (HP + (HP / koef)));
            speed = UnityEngine.Random.Range((speed - (speed / koef)), (speed + (speed / koef)));
        }
        GetComponent<AIPath>().maxSpeed = speed;
        Player = GameObject.Find("Player");
        if (enemySpawnType == enemySpawnTypes.wild)
        {
            ChangeState(attackState);
        }
        else if (enemySpawnType == enemySpawnTypes.camping)
        {
            ChangeState(patrolState);
        }
        if (aipath.velocity.magnitude < 0.3f) {
            aipath.SearchPath();  // Принудительное пересчитывание пути
        }
    }

    int direction = 0;
    int old_direction = 0;
    int rand_stay = 0;
    Vector2 vector;
    // Update is called once per frame
    void FixedUpdate()
    {
        float playerX = player.transform.position.x;
        float playerY = player.transform.position.y;
        if ((HP <= 0) && (IsDead == false))
        {
            EnemyDeath();
        }
        
    }

    private void Update()
    {
        currentEnemyState?.StateUpdate();
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "player1")
        {
            isMoving = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        try
        {
            if (collision.gameObject.name == "FullRoom")
            {
                collision.GetComponentInParent<RoomStats>().EnemyCount += 1;
            }
        }
        catch
        {

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        try
        {
            if (collision.gameObject.name == "FullRoom")
            {
                //collision.gameObject.transform.parent.gameObject.transform.Find("Stats").GetComponent<RoomStats>().EnemyCount -= 1;
                collision.GetComponentInParent<RoomStats>().EnemyCount -= 1;
            }
        }
        catch {
            
        }
    }

    void EnemyDeath() {
        IsDead = true;
        Transform pos = transform;
        int randInt = UnityEngine.Random.Range(GetComponent<EnemyLoot>().minCoinCount, GetComponent<EnemyLoot>().maxCoinCount);
        int randInt2 = UnityEngine.Random.Range(0, 101);
        GetComponent<EnemyLoot>().LootDrop();
        EnemyUtils.DisableFull(gameObject);
        foreach (EnemyBaseAttack script in attackState.enemyMeleeAttackList)
        {
            script.StopAllCoroutines();
        }
        foreach (EnemyBaseAttack script in attackState.enemySpecialAttackList)
        {
            script.StopAllCoroutines();
        }
        StartCoroutine(EnemyUtils.FullEnemyDeath(gameObject, 2.2f));
        if (OnEnemyDied != null) {
            OnEnemyDied(gameObject);
        }
    }

    public IEnumerator DamageEntity(GameObject damageCloud, float damage, GameObject pos)
    {
        if (canAttack)
        {
            gameObject.GetComponent<Animator>().SetTrigger("IsAttacking");
            isMoving = false;
            isAttack = true;
            //GetComponent<AIPath>().canMove = false;
            yield return new WaitForSeconds(0.5f);
            Quaternion smth = new Quaternion(0, 0, 0, 0);
            Vector3 position = pos.transform.position;
            Instantiate(damageCloud, position, smth).GetComponentInChildren<DamageCloudLogic>().damage = damage;
            yield return new WaitForSeconds(0.35f);
            isAttack = false;
            isMoving = true;
            GetComponent<AIPath>().canMove = true;
        }
    }

    public void OnChangingAttackVoid(GameObject enemy) {
        if (OnChangingAttack != null) { 
            OnChangingAttack(enemy);
        }

        if (enemySpawnType == enemySpawnTypes.camping) {
            //detectObject.transform.localScale = new Vector2(detectObject.transform.localScale.x * 2, detectObject.transform.localScale.y * 2);
            detectObject.transform.localScale = detectObject.GetComponent<DetectPlayer>().startScale * 2;
        }
        //currentState = enemyStates.attack;
        //patrolLogic.enabled = false;
        //GetComponent<EnemyReturning>().enabled = false;
        //attackLogic.enabled = true;

        //GetComponent<AIPath>().enabled = true;
    }

    // Метод смены фазы. Выходит из текущей фазы, и заходит в новую фазу.
    public void ChangeState(IEnemyState enemyState)
    {
        currentEnemyState?.Exit();
        currentEnemyState = enemyState;
        currentEnemyState.Enter();
    }
}
