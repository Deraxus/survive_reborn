using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameBlockLogic : MonoBehaviour
{
    public static bool gameStarted = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject == MainManager.Instance.mainPlayer)
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        if (!gameStarted)
        {
            gameStarted = true;
            MainManager.Instance.waveManager.GetComponent<WaveManager>().enabled = true;
            foreach (GameObject startGameBlock in GameObject.FindGameObjectsWithTag("startGameBlock"))
            {
                Destroy(startGameBlock);
            }
            Utils.StartNewMessage("Игра началась!");
        }    
    }

}
