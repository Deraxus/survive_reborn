using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class testRoomScript : MonoBehaviour
{
    public GameObject room;
    public GameObject corridor;
    public GameObject corridor2;

    public SRoomAfterData roomAfterInfo;

    public SRoomTilesData roomTilesData;
    void Start()
    {
        SRoomAfterData localRoomData = new SRoomAfterData();
        localRoomData.doorBotSize = 0;
        localRoomData.roomDirectionFromTo = RoomUtils.DirectionType.fromBotToTop;
        roomAfterInfo = RoomUtils.FillRoomSide(room, RoomUtils.RoomSide.bot, roomTilesData, localRoomData);
        //RoomUtils.CreateCorridor(corridor, RoomUtils.DirectionType.fromBotToTop, roomAfterInfo.doorTopSize, 6, 10, roomTilesData);
        //RoomUtils.CreateCorridor(corridor2, RoomUtils.CorridorType.fromLeftToRight, roomAfterInfo.doorRightSize, 6, 40, roomTilesData);
        //RoomUtils.CreateCorridor(corridor2, RoomUtils.DirectionType.fromLeftToRight, 12, 6, 40, roomTilesData);
        //GameObject.Find("RoomGenerator").GetComponent<RoomGenerator>().SpawnCorridor(roomAfterInfo.endblock_top.transform.localPosition, corridor, 0);
        Debug.Log("финиш");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
