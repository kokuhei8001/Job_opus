using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData
{
    //基本情報
    public Vector2Int Pos;
    public MapStatus Status;
}

public class RoomData : MapData
{
    //Roomの情報
    public int roomNum;
    public int Width;
    public int Height;
    public int RoadCount = 0; //通路がつながっている数
    public Vector2Int[] RoadPos = new Vector2Int[10]; //通路の開始位置 //メモリがもったいない
}

public class RoadData : MapData
{   
    //Raodの情報
    int StartRoomNum;
    int EndRoomNum;
    Vector2Int StartPos;
    Vector2Int EndPos;
}

public class GameManager : MonoBehaviour {

    public MapData[,] Map;
    public RoomData[,] roomData;
    public RoadData[,] roadData;

    private void Start()
    {
    }
}
