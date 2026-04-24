using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rostok : MonoBehaviour
{
    // Start is called before the first frame update
    public int growPeriod = 30;
    public float kfPerGrow = 0f;
    public float startKfUp = 1f;
    public int growCount = 5;
    public int stade = 1;

    private bool moveOn = true;

    public GameObject rateZone;
    public delegate void RostokGraveDelegate(GameObject rostok); 
    public event RostokGraveDelegate OnRostokGrave;

    void Start()
    {
        RostokLogic();
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((stade < growCount) && (moveOn == true)) {
            StartCoroutine(RostokGrow());
        }
    }

    void GoToPos() { 
    }

    public void RostokLogic() { 
    }

    IEnumerator RostokGrow() {
        moveOn = false;
        yield return new WaitForSeconds(growPeriod);
        OnRostokGrave?.Invoke(gameObject);
        Debug.Log("ÏÓÑÒÈË ÊÎÐÍÈ");
        stade += 1;
        moveOn = true;

    }

}
