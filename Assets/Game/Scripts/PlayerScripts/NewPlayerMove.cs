using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerMove : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 5.0f;
    Vector2 vector;
    public Animator animator;

    private Transform bodySprite;

    [Tooltip("Тут лежит ссылка на объект, со всем визуальным отображением")]
    public GameObject visualsObject;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (visualsObject == null )
        {
            visualsObject = transform.Find("Visuals").gameObject;
            bodySprite = visualsObject.transform.Find("bodySprite").GetComponent<Transform>();
        }
    }

    // Update is called once per fram

    private void FixedUpdate()
    {
        speed = GetComponent<Player>().speed * GetComponent<Player>().speedKf;
        vector.x = Input.GetAxisRaw("Horizontal");
        vector.y = Input.GetAxisRaw("Vertical");
        if (Mathf.Abs(vector.x) > 0 || Mathf.Abs(vector.y) > 0)
        {
            bodySprite.GetComponent<Animator>().SetInteger("horizontalmove", 1);
            Debug.Log("движ");
        }
        else
        {
            bodySprite.GetComponent<Animator>().SetInteger("horizontalmove", 0);
        }
        rb.MovePosition(rb.position + vector * speed * Time.fixedDeltaTime);
    }
}
