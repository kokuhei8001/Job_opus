using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum MapData
{
    Room ,
    Wall ,
    Road ,
    Grond
}

class ROOM
{
    public int room_num;        //番号(配列番号と一緒)
    public Vector2Int startPos; //開始地点
    public Vector2Int endPos;   //終了地点
    public Vector2Int[] RoadPos;//つながっている全ての通路の座標
}

//public static TEnum ConvertToEnum<TEnum>(int number)
//{
//    return(TEnum) Enum.ToObject(typeof(TEnum), number);
//}

public class MapCreate : MonoBehaviour {

    [SerializeField] private GameObject Player;      //プレイヤーキャラクター本体
    [SerializeField] private GameObject WallObject;  //壁のオブジェクト
    [SerializeField] private GameObject GroundObject;//地面のオブジェクト
    [SerializeField] private float CrackLength;      //オブジェクト同士の隙間距離

    private GameObject Parent;//大量のオブジェが生産されるのでこの階層下で生成させる

    //Mapのデータ
    MapData[,] Map;

    //Mapの大きさ
    int MapWidth = 50;
    int MapHeight = 50;

    int roomsize;          //部屋の数
    int RoomCountMin = 10; 
    int RoomCountMax = 15;
    ROOM[] room;

    int roomMinHeight = 5; //縦のふり幅
    int roomMaxHeight = 10;

    int roomMinWidth = 5;  //横のふり幅
    int roomMaxWidth = 10;

    //道の集合地点を増やしたいならこれを増やす
    int meetPointCount = 1;

    void Start()
    {
        //たくさんクローンオブジェクトが生成されてヒエラルキーが見にくくなるので親オブジェクトを作っておく
        Parent = new GameObject("Map");
        Parent.transform.position = new Vector3(0, 0, 0);

        //ダンジョンを生成
        ResetMapData();
        CreateSpaceData();
        CreateDangeon();
    }

    //Mapデータのリセット
    private void ResetMapData()
    {
        Map = new MapData[MapHeight, MapWidth];
        for (int i = 0; i < MapHeight; i++){
            for (int k = 0; k < MapWidth; k++){
                Map[i, k] = MapData.Wall;
            }
        }
    }

    //部屋のスペースを作る
    private void CreateSpaceData()
    {
        int roomCount = Random.Range(RoomCountMin, RoomCountMax); //部屋の数を決める

        room = new ROOM[roomCount];

        int[] meetPointsX = new int[meetPointCount];
        int[] meetPointsY = new int[meetPointCount];
        for (int i = 0; i < meetPointsX.Length; i++)
        {
            meetPointsX[i] = Random.Range(MapWidth / 4, MapWidth * 3 / 4);
            meetPointsY[i] = Random.Range(MapHeight / 4, MapHeight * 3 / 4);

            Map[meetPointsY[i], meetPointsX[i]] = MapData.Road;
        }

        for (int i = 0; i < roomCount; i++)
        {
            int roomHeight = Random.Range(roomMinHeight, roomMaxHeight);
            int roomWidth = Random.Range(roomMinWidth, roomMaxWidth);
            int roomPointX = Random.Range(2, MapWidth - roomMaxWidth - 2);
            int roomPointY = Random.Range(2, MapWidth - roomMaxWidth - 2);

            int roadStartPointX = Random.Range(roomPointX, roomPointX + roomWidth);
            int roadStartPointY = Random.Range(roomPointY, roomPointY + roomHeight);

            bool isRoad = CreateRoomData(roomHeight, roomWidth, roomPointX, roomPointY); //部屋に通路を引くかどうか判断する

            if (isRoad == false)//他の部屋と重なっていなかったら通路を作る
            {
                CreateRoadData(roadStartPointX, roadStartPointY, meetPointsX[Random.Range(0, 0)], meetPointsY[Random.Range(0, 0)]);
            }
        }
    }

    //Mapに部屋を作っていく
    private bool CreateRoomData(int roomHeight, int roomWidth, int roomPointX, int roomPointY)
    {
        bool isRoad = false;
        for (int i = 0; i < roomHeight; i++){
            for (int k = 0; k < roomWidth; k++){
                if (Map[roomPointY + i, roomPointX + k] == MapData.Road){ //重なっている部屋があったら出入り口の道を作らないようにする
                    isRoad = true;
                }
                else{
                    Map[roomPointY + i, roomPointX + k] = MapData.Road;   //部屋のステータスを代入する
                }
            }
        }
        return isRoad;
    }

    //Mapに通路を作っていく
    private void CreateRoadData(int roadStartPointX, int roadStartPointY, int meetPointX, int meetPointY)
    {
        bool isRight;
        if (roadStartPointX > meetPointX)
        {
            isRight = true;
        }
        else
        {
            isRight = false;
        }
        bool isUnder;
        if (roadStartPointY > meetPointY)
        {
            isUnder = false;
        }
        else
        {
            isUnder = true;
        }

        if (Random.Range(0, 2) == 0)
        {

            while (roadStartPointX != meetPointX)
            {
                Map[roadStartPointY, roadStartPointX] = MapData.Road;
                if (isRight == true)
                {
                    roadStartPointX--;
                }
                else
                {
                    roadStartPointX++;
                }
            }

            while (roadStartPointY != meetPointY)
            {
                Map[roadStartPointY, roadStartPointX] = MapData.Road;
                if (isUnder == true)
                {
                    roadStartPointY++;
                }
                else
                {
                    roadStartPointY--;
                }
            }
        }
        else
        {

            while (roadStartPointY != meetPointY)
            {
                Map[roadStartPointY, roadStartPointX] = MapData.Road;
                if (isUnder == true)
                {
                    roadStartPointY++;
                }
                else
                {
                    roadStartPointY--;
                }
            }

            while (roadStartPointX != meetPointX)
            {

                Map[roadStartPointY, roadStartPointX] = MapData.Road;
                if (isRight == true)
                {
                    roadStartPointX--;
                }
                else
                {
                    roadStartPointX++;
                }

            }

        }
    }

    //ダンジョンをオブジェクトに起こして生成する
    private void CreateDangeon()
    {
        for (int i = 0; i < MapHeight; i++){
            for (int k = 0; k < MapWidth; k++){
                if (Map[i, k] == MapData.Wall)
                {
                    Instantiate(WallObject, new Vector3(k * CrackLength, 0, i * CrackLength), Quaternion.identity, Parent.transform);
                    Instantiate(WallObject, new Vector3(k * CrackLength, 1, i * CrackLength), Quaternion.identity, Parent.transform); //力技で高さをいじってる
                }
                Instantiate(GroundObject, new Vector3(k * CrackLength, -1, i * CrackLength), Quaternion.identity, Parent.transform);
            }
        }
    }
}