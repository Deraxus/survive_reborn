using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedBoots_item : ItemLogic, IStatModifier
{
    public float speedKf = 0.2f;
    
    private float oldSpeedKf;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnItemTaking()
    {
        //oldSpeedKf = player.GetComponent<Player>().speedKf * speedKf;
        MainItemManager.Instance.AddModify(MainItemManager.ModifyTypes.SpeedKf, speedKf);
        Debug.Log("111");
        //player.GetComponent<Player>().speedKf += speedKf;
        //player.GetComponent<Player>().speed += player.GetComponent<Player>().speedKf * speedKf;
    }

    public void OnItemLoosing()
    {
        //GameObject player = GetComponent<MainItemManager>().player;
        //player.GetComponent<Player>().speedKf -= oldSpeedKf;
        MainItemManager.Instance.RemoveModify(MainItemManager.ModifyTypes.SpeedKf, speedKf);
        //player.GetComponent<Player>().speed += player.GetComponent<Player>().speedKf * speedKf;
    }


}
