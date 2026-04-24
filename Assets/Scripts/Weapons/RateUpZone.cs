using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RateUpZone : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject player;

    public GameObject publicRostok;
    public float publicKoef = 1f;
    public int stade = 1;
    void Start()
    {
        player = GameObject.Find("Player");
        publicRostok.GetComponent<Rostok>().OnRostokGrave += OnRostokGrave;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.transform.name == "Player") {
            UpRate(publicKoef);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.transform.name == "Player")
        {
            DownRate(publicKoef);
        }
    }

    private void UpRate(float koef) {
        player.GetComponent<Player>().rateKf += publicKoef;
    }

    private void DownRate(float koef)
    {
        player.GetComponent<Player>().rateKf -= publicKoef;
    }

    void OnRostokGrave(GameObject rostok) {
        if (rostok == publicRostok) {
            stade += 1;
            transform.localScale = new Vector3(transform.localScale.x + 1, transform.localScale.y + 1);
        }
    }
}
