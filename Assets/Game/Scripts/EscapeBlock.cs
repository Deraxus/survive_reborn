using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeBlock : MonoBehaviour
{
    bool canEscape = false;
    GameObject timer;
    void Start()
    {
        timer = GameObject.Find("Timer");   
    }

    // Update is called once per frame
    void Update()
    {
        canEscape = WaveManager.Instance.canEscape;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player") {
            FinishGame();
        }
    }

    void FinishGame() {
        if (canEscape)
        {
            SceneManager.LoadScene(1);
        }
    }
}
