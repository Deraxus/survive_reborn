using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SRoomAfterData : ScriptableObject
{
    public GameObject room;

    // ¬ кусочках (1 блок = 2 кусочка)
    public int minimumDoorSizeVert = 8;
    public int minimumDoorSizeHor = 10;

    public float roomSpawnChance = 0.5f;

    //  оэфициент, на который умножаетс€ верхний потолок при определении размера коридора.
    // „ем меньше коэфициент - тем меньше будут коридоры.
    public float roomSizeKoef = 0.7f;

    public GameObject endblock_top;
    public GameObject endblock_right;
    public GameObject endblock_bot;
    public GameObject endblock_left;

    public int doorTopSize;
    public int doorRightSize;
    public int doorBotSize;
    public int doorLeftSize;

    public bool roomHasCorridorTop = true;
    public bool roomHasCorridorRight = true;
    public bool roomHasCorridorBot = true;
    public bool roomHasCorridorLeft = true;

    public RoomUtils.DirectionType roomDirectionFromTo = RoomUtils.DirectionType.noDirection;

    // -1 «начит фиксированного размера нет - лепим любой рандомный
    public int fixedDoorSize = -1;
    // 0 - верх
    // 1 - право
    // 2 - вниз
    // 3 - влево
    // ≈сли галочка стоит - значит в этой стороне можно смело строить следующие комнаты (коридоры)
    public bool[] readyToBuildRoomsHere = new bool[4];
}
