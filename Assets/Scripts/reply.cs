using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class reply : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void change() {
        SceneManager.LoadScene(0);
        return;
    }

    public void exit()
    {
        Application.Quit();
        return;
    }
}
