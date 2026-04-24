using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu()]
public class SRoomTilesData : ScriptableObject
{
    [SerializeField] public GameObject startGameBlock;

    [SerializeField] public TileBase side0Block;
    [SerializeField] public TileBase side1Block;
    [SerializeField] public TileBase side2Block;
    [SerializeField] public TileBase side3Block;

    [SerializeField] public TileBase topWallFirst;
    [SerializeField] public TileBase topWallSecond;

    [SerializeField] public TileBase rightWallFirst;
    [SerializeField] public TileBase rightWallSecond;

    [SerializeField] public TileBase bottomWallFirst;
    [SerializeField] public TileBase bottomWallSecond;

    [SerializeField] public TileBase leftWallFirst;
    [SerializeField] public TileBase leftWallSecond;

    [SerializeField] public TileBase classicPol0; // Верхний левый угол пола
    [SerializeField] public TileBase classicPol1; // Верхний правый угол пола
    [SerializeField] public TileBase classicPol2; // Нижний правый угол пола
    [SerializeField] public TileBase classicPol3; // Нижний левый угол пола

    [SerializeField] public TileBase perPolStartFirst0;
    [SerializeField] public TileBase perPolStartFirst1;
    [SerializeField] public TileBase perPolStartFirst2;
    [SerializeField] public TileBase perPolStartFirst3;
    [SerializeField] public TileBase perPolStartFirst4;
    [SerializeField] public TileBase perPolStartFirst5;

    [SerializeField] public TileBase perPolStartSecond0;
    [SerializeField] public TileBase perPolStartSecond1;
    [SerializeField] public TileBase perPolStartSecond2;
    [SerializeField] public TileBase perPolStartSecond3;
    [SerializeField] public TileBase perPolStartSecond4;
    [SerializeField] public TileBase perPolStartSecond5;


    [SerializeField] public TileBase perPolMidFirst0;
    [SerializeField] public TileBase perPolMidFirst1;
    [SerializeField] public TileBase perPolMidFirst2;
    [SerializeField] public TileBase perPolMidFirst3;
    [SerializeField] public TileBase perPolMidFirst4;
    [SerializeField] public TileBase perPolMidFirst5;

    [SerializeField] public TileBase perPolMidSecond0;
    [SerializeField] public TileBase perPolMidSecond1;
    [SerializeField] public TileBase perPolMidSecond2;
    [SerializeField] public TileBase perPolMidSecond3;
    [SerializeField] public TileBase perPolMidSecond4;
    [SerializeField] public TileBase perPolMidSecond5;
}
