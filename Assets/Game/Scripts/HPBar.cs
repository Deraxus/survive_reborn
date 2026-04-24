using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    private float HP;
    private float maxHP;
    private GameObject player;

    public Image image;
    void Start()
    {
        player = GameObject.Find("Player");
        HP = player.GetComponent<Player>().HP;
        maxHP = player.GetComponent<Player>().MaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        HP = player.GetComponent<Player>().HP;
        maxHP = player.GetComponent<Player>().MaxHP;
        image.fillAmount = HP / maxHP;
    }
}
