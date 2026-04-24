using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RainbowBullet", menuName = "Weapon Features/Rainbow Bullet")]
public class RainbowBulletFeature : WeaponFeature
{
    public override void OnShoot(GameObject bullet)
    {
        base.OnShoot(bullet);
        
        float hue = Random.Range(0f, 1f);        // Оттенок от 0 до 1
        float saturation = 0.8f;                  // Яркость цвета
        float value = 1f;                         // Максимальная яркость

        Color rainbowColor = Color.HSVToRGB(hue, saturation, value);

        bullet.GetComponentInChildren<SpriteRenderer>().color = rainbowColor;
    }
}
