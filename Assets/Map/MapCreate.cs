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

public class ROOM
{
    public int Room_num;    //番号(配列番号と一緒)
    public Vector2Int Pos;  //開始地点
    public Vector2Int Size; //部屋のサイズ
    //public Vector2Int[] RoadPos;//つながっている全ての通路の座標
}


public class MapCreate : MonoBehaviour {

    //Intからenumへ
    public static TEnum ConvertToEnum<TEnum>(int number)
    {
        return (TEnum)System.Enum.ToObject(typeof(TEnum), number);
    }

    [SerializeField] private GameObject Player = null;      //プレイヤーキャラクター本体
    [SerializeField] private GameObject Enemy = null;       //エネミーキャラクター本体
    [SerializeField] private GameObject Goal = null;        //ゴールのオブジェクト
    [SerializeField] private GameObject WallObject = null;  //壁のオブジェクト
    [SerializeField] private GameObject GroundObject = null;//地面のオブジェクト
    [SerializeField] private float CrackLength = 0.0f;      //オブジェクト同士の隙間距離
    [SerializeField] private int EnemySize = 1;             //エネミーの数

    private GameObject Parent;//大量のオブジェが生産されるのでこの階層下で生成させる

    //Mapのデータ
    private MapData[,] Map;
    private ROOM[] room;

    //Mapの大きさ
    private int MapWidth = 50;
    private int MapHeight = 50;

    private int roomCount;          //部屋の数
    private int RoomCountMin = 10; 
    private int RoomCountMax = 15;

    private int roomMinHeight = 5; //縦のふり幅
    private int roomMaxHeight = 10;
     
    private int roomMinWidth = 5;  //横のふり幅
    private int roomMaxWidth = 10;
    
    //道の集合地点を増やしたいならこれを増やす
    private int meetPointCount = 1;

    //Playerがポップする部屋の番号s
    private int PlayerPopRoom = 0;
    

    
    void Start()
    {
        //たくさんクローンオブジェクトが生成されてヒエラルキーが見にくくなるので親オブジェクトを作っておく
        Parent = new GameObject("Map");
        Parent.transform.position = new Vector3(0, 0, 0);

        //ダンジョンを生成
        ResetMapData();
        CreateSpaceData();
        CreateDangeon();
        PlayerPop();
        for (int i = 0; i < EnemySize; i++)
        {
            EnemyPop();
        }
        GorlPop();
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
        roomCount = Random.Range(RoomCountMin, RoomCountMax); //部屋の数を決める

        room = new ROOM[roomCount];//Mapデータの配列

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
            bool isRoad;
            int roomHeight;
            int roomWidth;
            int roomPointX;
            int roomPointY;

            int roadStartPointX;
            int roadStartPointY;
            
            roomHeight = Random.Range(roomMinHeight, roomMaxHeight);    //高さ
            roomWidth  = Random.Range(roomMinWidth, roomMaxWidth);      //横幅
            roomPointX = Random.Range(2, MapWidth - roomMaxWidth - 2);  //部屋のｘ位置
            roomPointY = Random.Range(2, MapWidth - roomMaxWidth - 2);  //部屋のｙ位置

            roadStartPointX = Random.Range(roomPointX, roomPointX + roomWidth);  //部屋に通路を繋ぐ位置
            roadStartPointY = Random.Range(roomPointY, roomPointY + roomHeight);

            isRoad = CreateRoomData(roomHeight, roomWidth, roomPointX, roomPointY); //部屋に通路を引くかどうか判断する
            

            //部屋のデータ
            room[i] = new ROOM();
            room[i].Room_num = i;
            room[i].Pos = new Vector2Int(roomPointX, roomPointY);
            room[i].Size = new Vector2Int(roomWidth, roomHeight);
            //room[i].RoadPos[0] = new Vector2Int(roadStartPointX, roadStartPointY);
            //ここまで

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
        bool isRight; //左右を調べる
        if (roadStartPointX > meetPointX){
            isRight = true;
        } else {
            isRight = false;
        }

        bool isUnder; //上下を調べる
        if (roadStartPointY > meetPointY){
            isUnder = false;
        } else {
            isUnder = true;
        }

        //通路を引くプログラム
        //全部同じパターンだと単調になってしますから上から延ばすか横から延ばすかの2パターン用意する
        if (Random.Range(0, 2) == 0){
            while (roadStartPointX != meetPointX)
            {
                Map[roadStartPointY, roadStartPointX] = MapData.Road;
                if (isRight == true){
                    roadStartPointX--;
                } else {
                    roadStartPointX++;
                }
            }
            while (roadStartPointY != meetPointY)
            {
                Map[roadStartPointY, roadStartPointX] = MapData.Road;
                if (isUnder == true){
                    roadStartPointY++;
                } else {
                    roadStartPointY--;
                }
            }
        }
        else
        {
            while (roadStartPointY != meetPointY)
            {
                Map[roadStartPointY, roadStartPointX] = MapData.Road;
                if (isUnder == true){
                    roadStartPointY++;
                }else{
                    roadStartPointY--;
                }
            }
            while (roadStartPointX != meetPointX)
            {
                Map[roadStartPointY, roadStartPointX] = MapData.Road;
                if (isRight == true){
                    roadStartPointX--;
                }else{
                    roadStartPointX++;
                }
            }
        }//if()else
    }//void CreatRoadData

    //ダンジョンをオブジェクトに起こして生成する
    private void CreateDangeon()
    {
        for (int i = 0; i < MapHeight; i++){
            for (int k = 0; k < MapWidth; k++)
            {
                if (Parent != null)
                {
                    if (Map[i, k] == MapData.Wall)
                    {
                        Instantiate(WallObject, new Vector3(k * CrackLength, 0, i * CrackLength), Quaternion.identity, Parent.transform);
                        Instantiate(WallObject, new Vector3(k * CrackLength, 1, i * CrackLength), Quaternion.identity, Parent.transform); //力技で高さをいじってる
                    }
                    Instantiate(GroundObject, new Vector3(k * CrackLength, -1, i * CrackLength), Quaternion.identity, Parent.transform);
                } else {
                    //たくさんクローンオブジェクトが生成されてヒエラルキーが見にくくなるので親オブジェクトを作っておく
                    Parent = new GameObject("Map");
                    Parent.transform.position = new Vector3(0, 0, 0);
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

    //Playerのポップ
    private void PlayerPop()
    {
        PlayerPopRoom = Random.Range(0, roomCount);
        int PopPosX = Random.Range(room[PlayerPopRoom].Pos.x, room[PlayerPopRoom].Pos.x + room[PlayerPopRoom].Size.x);
        int PopPosY = Random.Range(room[PlayerPopRoom].Pos.y, room[PlayerPopRoom].Pos.y + room[PlayerPopRoom].Size.y);
        Instantiate(Player, new Vector3(PopPosX * CrackLength, 1 , PopPosY * CrackLength), Quaternion.identity);
    }

    //Enemyのポップ
    //Enemyとゴール同じ部屋の出現させてもいいのか？
    private void EnemyPop()
    {
        //ポップする部屋番号と位置座標
        int EnemyPopRoom;
        int PopPosX;
        int PopPosY;

        do
        {
            EnemyPopRoom = Random.Range(0, roomCount);
            PopPosX = Random.Range(room[EnemyPopRoom].Pos.x, room[EnemyPopRoom].Pos.x + room[EnemyPopRoom].Size.x);
            PopPosY = Random.Range(room[EnemyPopRoom].Pos.y, room[EnemyPopRoom].Pos.y + room[EnemyPopRoom].Size.y);
        }
        while (EnemyPopRoom == PlayerPopRoom);//プレイヤーと同じ部屋のポップしないようにする
        
        Instantiate(Enemy, new Vector3(PopPosX * CrackLength, 1, PopPosY * CrackLength), Quaternion.identity);
    }
    
    //ゴールの生成
    void GorlPop()
    {
        int GoalPopRoom;
        int PopPosX;
        int PopPosY;

        do
        {
            GoalPopRoom = Random.Range(0, roomCount);
            PopPosX = Random.Range(room[GoalPopRoom].Pos.x, room[GoalPopRoom].Pos.x + room[GoalPopRoom].Size.x);
            PopPosY = Random.Range(room[GoalPopRoom].Pos.y, room[GoalPopRoom].Pos.y + room[GoalPopRoom].Size.y);
        }
        while (GoalPopRoom == PlayerPopRoom);

        Instantiate(Goal, new Vector3(PopPosX * CrackLength, 1, PopPosY * CrackLength),Quaternion.Euler(45,0,45)); //identityだと回転軸が０，０，０に戻されてしまう
    }

}