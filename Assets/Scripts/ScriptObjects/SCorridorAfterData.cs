using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCorridorAfterData : ScriptableObject
{
    public RoomUtils.DirectionType corridorType;

    public GameObject endblock_top = null;
    public GameObject endblock_right = null;
    public GameObject endblock_bot = null;
    public GameObject endblock_left = null;

    public int doorTopSize;
    public int doorRightSize;
    public int doorBotSize;
    public int doorLeftSize;
}
