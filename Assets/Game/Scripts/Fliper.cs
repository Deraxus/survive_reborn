using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fliper : MonoBehaviour
{
    public bool playerFlipped = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.localEulerAngles.z > 90 || transform.localEulerAngles.z < -90) && !playerFlipped)
        {
            GetComponent<PistolRotater>().offset = 180;
            MainManager.Instance.mainPlayer.transform.localScale = new Vector2(-1 * MainManager.Instance.mainPlayer.transform.localScale.x, MainManager.Instance.mainPlayer.transform.localScale.y);
            playerFlipped = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

    }

}
