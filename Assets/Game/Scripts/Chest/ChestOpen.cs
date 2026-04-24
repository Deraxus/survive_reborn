using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ChestOpen : MonoBehaviour
{
    public GameObject Chest;
    private Animator anim;
    public GameObject Chest_sss;
    private int LootNumber;
    private bool isOpened;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "chest")
        {
            collision.gameObject.GetComponent<ChestLogic>().canBeOpen = true;
            //OpenChest(collision.gameObject);
            //anim.SetTrigger("Tog");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "chest")
        {
            collision.gameObject.GetComponent<ChestLogic>().canBeOpen = false;
            //OpenChest(collision.gameObject);
            //anim.SetTrigger("Tog");
        }
    }


    public void OpenChest(GameObject chest) {
        anim = chest.gameObject.GetComponent<Animator>();
        anim.SetTrigger("Tog");
        chest.GetComponent<ChestLogic>().LootDrop();
    }
}
