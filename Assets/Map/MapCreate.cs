﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapStatus
{
    Room,
    Wall,
    Road,
    Door,
    Ground
}

public class ROOM
{
    public int Room_num;    //番号(配列番号と一緒)
    public Vector2Int Pos;  //開始地点
    public Vector2Int Size; //部屋のサイズ
}


public class MapCreate : MonoBehaviour
{
    [SerializeField] private GameManager Manager = null;
    [SerializeField] private GameObject Player = null;      //プレイヤーキャラクター本体
    [SerializeField] private GameObject Enemy = null;       //エネミーキャラクター本体
    [SerializeField] private GameObject Goal = null;        //ゴールのオブジェクト
    [SerializeField] private GameObject WallObject = null;  //壁のオブジェクト
    [SerializeField] private GameObject DoorObject = null;  //ドアのオブジェクト
    [SerializeField] private GameObject GroundObject = null;//地面のオブジェクト
    [SerializeField] private float CrackLength = 1.0f;      //オブジェクト同士の隙間距離
    [SerializeField] private int EnemySize = 0;             //エネミーの数
    [SerializeField] private bool ceiling;                  //デバック用（天井が消える）

    //Mapのデータ
    public MapStatus[,] Map;
    public ROOM[] room;

    //Mapの大きさ
    public int MapWidth;
    public int MapHeight;

    public int roomCount;          //部屋の数 (10,15)
    private int RoomCountMin = 10;
    private int RoomCountMax = 15;

    private int roomMinHeight = 4; //縦のふり幅 (5,10)
    private int roomMaxHeight = 8;

    private int roomMinWidth = 4;  //横のふり幅 (5,10)
    private int roomMaxWidth = 8;

    //道の集合地点を増やしたいならこれを増やす
    private int meetPointCount = 1;
    private int[] meetPointsX;
    private int[] meetPointsY;

    //Playerがポップする部屋の番号
    private int PlayerPopRoom = 0;
    public Vector2Int PlayerPopPos = new Vector2Int(0, 0);
    public Vector2Int GorlPopPos = new Vector2Int(0, 0);

    private void Awake()
    {
        Manager = GameObject.Find("GameManager").GetComponent<GameManager>();

        MapWidth = Manager.MapWidth;
        MapHeight = Manager.MapHeight;

        //ダンジョンを生成
        ResetMapData();
        CreateSpaceData();
        CreateDangeon();

        //プレイヤー、敵、ゴールを配置する
        if (Player != null) { PlayerPop(); }
        for (int i = 0; i < EnemySize; i++){ EnemyPop(); }
        if (Goal != null){ GorlPop(); }
        
        //Managerにプレイヤーとゴールの位置を渡す
        Manager.PlayerPos = PlayerPopPos;
        Manager.GorlPos = GorlPopPos;
    }

    //Mapデータのリセット
    private void ResetMapData()
    {
        //GameManager------------------------------------------------------------------------------------------------------------------------
        Manager.Map = new MapData[MapHeight, MapWidth];
        for (int i = 0; i < MapHeight; i++)
        {
            for (int k = 0; k < MapWidth; k++)
            {
                Manager.Map[i, k] = new MapData();
                Manager.Map[i, k].Pos = new Vector2Int(i, k);
                Manager.Map[i, k].Status = MapStatus.Wall;
            }
        }
        //MapCreate------------------------------------------------------------------------------------------------------------------------
        Map = new MapStatus[MapHeight, MapWidth];
        for (int i = 0; i < MapHeight; i++)
        {
            for (int k = 0; k < MapWidth; k++)
            {
                Map[i, k] = MapStatus.Wall;
            }
        }
    }

    //部屋のスペースを作る
    private void CreateSpaceData()
    {
        roomCount = Random.Range(RoomCountMin, RoomCountMax); //部屋の数を決める

        room = new ROOM[roomCount];//Mapデータの配列
<<<<<<< HEAD
        Manager.RoomCount = roomCount;
=======
>>>>>>> 27dcafa932f573b7918e14a2d5ae7aacb501b779
        Manager.Room = new Room[roomCount]; //GameManager用の----------------------------------------------------------

        meetPointsX = new int[meetPointCount]; //通路の集合地点を決める
        meetPointsY = new int[meetPointCount];
        for (int i = 0; i < meetPointsX.Length; i++)
        {
            meetPointsX[i] = Random.Range(MapWidth / 4, MapWidth * 3 / 4);
            meetPointsY[i] = Random.Range(MapHeight / 4, MapHeight * 3 / 4);

            //Map[meetPointsX[i], meetPointsY[i]] = MapStatus.Road;
        }

        //部屋を作って行く
        for (int i = 0; i < roomCount; i++)
        {
            bool isRoad;
            int roomHeight = 0;  //高さ
            int roomWidth = 0;   //幅
            int roomPointX = 0;  //部屋のｘ位置
            int roomPointY = 0;  //部屋のｙ位置

            int roadStartPointX = 0;
            int roadStartPointY = 0;

            for (int k = 0; k < 5; k++)//5回やって全部重なっていたらもうあきらめる(そうでないと無限ループにい入る可能性がある)
            {
                roomHeight = Random.Range(roomMinHeight, roomMaxHeight);   
                roomWidth = Random.Range(roomMinWidth, roomMaxWidth);      
                roomPointX = Random.Range(2, MapWidth - roomMaxWidth - 2); 
                roomPointY = Random.Range(2, MapWidth - roomMaxWidth - 2); 

                if (!IsBooking(roomHeight, roomWidth, roomPointX, roomPointY))//他の部屋と重なっていなかったらループ抜ける
                { break; }
            }

            roadStartPointX = Random.Range(roomPointX, roomPointX + roomWidth);  //部屋に通路を繋ぐ位置
            roadStartPointY = Random.Range(roomPointY, roomPointY + roomHeight);

            isRoad = CreateRoomData(roomHeight, roomWidth, roomPointX, roomPointY); //部屋に通路を引くかどうか判断する

            //部屋のデータをいれてく
            //GameManager------------------------------------------------------------------------------------------------------------------------
<<<<<<< HEAD
            for (int a = roomPointY; a < roomPointY + roomHeight; a++)
            {
                for (int b = roomPointX; b < roomPointX + roomWidth; b++)
                {
                    Manager.Map[a, b].Pos = new Vector2Int(a, b);
                    Manager.Map[a, b].Status = MapStatus.Room;
                    //Manager.roomData[a, b].roomNum = i;
                    //Manager.roomData[a, b].Height = roomHeight;
                    //Manager.roomData[a, b].Width = roomWidth;
                }
            }
=======
            //for (int a = roomPointY; a < roomPointY + roomHeight; a++)
            //{
            //    for (int b = roomPointX; b < roomPointX + roomWidth; b++)
            //    {
            //        Manager.Map[a, b].Pos = new Vector2Int(a , b);
            //        Manager.Map[a, b].Status = MapStatus.Room;
            //        //Manager.roomData[a, b].roomNum = i;
            //        //Manager.roomData[a, b].Height = roomHeight;
            //        //Manager.roomData[a, b].Width = roomWidth;
            //    }
            //}
>>>>>>> 27dcafa932f573b7918e14a2d5ae7aacb501b779

            Manager.Room[i] = new Room();
            Manager.Room[i].Pos = new Vector2Int(roomPointX, roomPointY);
            Manager.Room[i].Num = i;
            Manager.Room[i].Size = new Vector2Int(roomWidth, roomHeight);

            //MapCreate------------------------------------------------------------------------------------------------------------------------
            //room[i] = new ROOM();
            //room[i].Room_num = i;
            //room[i].Pos = new Vector2Int(roomPointX, roomPointY);
            //room[i].Size = new Vector2Int(roomWidth, roomHeight);
            //ここまで

            if (isRoad == false)//他の部屋と重なっていなかったら通路を作る
            {
                CreateRoadData(roadStartPointX, roadStartPointY, meetPointsX[Random.Range(0, 0)], meetPointsY[Random.Range(0, 0)]);
            }
        }
    }

    //部屋が重なっているかどうか
    private bool IsBooking(int roomHeight, int roomWidth, int roomPointX, int roomPointY)
    {
        for (int i = 0 - 1; i < roomHeight + 1; i++){
            for (int k = 0 - 1; k < roomWidth + 1; k++){

                //GameManager----------------------------------------------------------------------
                if (Manager.Map[roomPointX + i, roomPointY + k].Status == MapStatus.Room)
<<<<<<< HEAD
                {
                    return true;
                }


                //MapCreate------------------------------------------------------------------------
                if (Map[roomPointX + i, roomPointY + k] == MapStatus.Room)
=======
>>>>>>> 27dcafa932f573b7918e14a2d5ae7aacb501b779
                {
                    return true;
                }


                //MapCreate------------------------------------------------------------------------
                //if (Map[roomPointX + i, roomPointY + k] == MapStatus.Room)
                //{
                //    return true;
                //}
            }
        }
        return false;
    }

    //Mapに部屋を作っていく
    private bool CreateRoomData(int roomHeight, int roomWidth, int roomPointX, int roomPointY)
    {
        bool isRoad = false;
        for (int i = 0; i < roomHeight; i++)
        {
            for (int k = 0; k < roomWidth; k++)
            {
                //GameManager-----------------------------------

                if (Manager.Map[roomPointX + k, roomPointY + i].Status == MapStatus.Room ||
                    Manager.Map[roomPointX + k, roomPointY + i].Status == MapStatus.Road)
<<<<<<< HEAD
                {
                    Manager.Map[roomPointX + k, roomPointY + i].Status = MapStatus.Room;//-------------------------------------ここいる？
                    isRoad = true;
                }
                else
                {
                    Manager.Map[roomPointX + k, roomPointY + i].Status = MapStatus.Room;
                }

                //MapCreate------------------------------------
                if (Map[roomPointX + k, roomPointY + i] == MapStatus.Room || //他の部屋に重なっている
                    Map[roomPointX + k, roomPointY + i] == MapStatus.Road) 　//または道にまたがっていたら道を新たには作らない
=======
>>>>>>> 27dcafa932f573b7918e14a2d5ae7aacb501b779
                {
                    Manager.Map[roomPointX + k, roomPointY + i].Status = MapStatus.Room;//-------------------------------------ここいる？
                    isRoad = true;
                }
                else
                {
                    Manager.Map[roomPointX + k, roomPointY + i].Status = MapStatus.Room;
                }
                
                //MapCreate------------------------------------
                //if (Map[roomPointX + k, roomPointY + i] == MapStatus.Room || //他の部屋に重なっている
                //    Map[roomPointX + k, roomPointY + i] == MapStatus.Road) 　//または道にまたがっていたら道を新たには作らない
                //{
                //    Map[roomPointX + k, roomPointY + i] = MapStatus.Room;
                //    isRoad = true;
                //}
                //else
                //{
                //    Map[roomPointX + k, roomPointY + i] = MapStatus.Room;   //部屋のステータスを代入する
                //}
            }
        }
        return isRoad;
    }

    //Mapに通路を作っていく
    private void CreateRoadData(int roadStartPointX, int roadStartPointY, int meetPointX, int meetPointY)
    {
        bool isRight; //左右を調べる
        if (roadStartPointX > meetPointX)
        {
            isRight = true;
        }
        else
        {
            isRight = false;
        }

        bool isUnder; //上下を調べる
        if (roadStartPointY > meetPointY)
        {
            isUnder = false;
        }
        else
        {
            isUnder = true;
        }

        //通路を引くプログラム
        bool InRoom = false;    //部屋に当たったらそれ以上通路を引きたくない。しかし、制作した瞬間の部屋も対象にしているため。自分の部屋を抜けた時にtrueにする

        //全部同じパターンだと単調になってしますから上から延ばすか横から延ばすかの2パターン用意する
        if (Random.Range(0, 2) == 0)
        {
            //左右に伸ばして上下に伸ばすパターン
            InRoom = false;
            while (roadStartPointX != meetPointX)
            {

                Manager.Map[roadStartPointX, roadStartPointY].Status = MapStatus.Road;//-------------------------------------
                Map[roadStartPointX, roadStartPointY] = MapStatus.Road;
                if (isRight == true)
                {
                    roadStartPointX--;
                }
                else
                {
                    roadStartPointX++;
                }

                //部屋に当たった時点で終了させる

                //manager
                if (Manager.Map[roadStartPointX, roadStartPointY].Status == MapStatus.Wall)
<<<<<<< HEAD
                {
                    InRoom = true;
                }
                if (Manager.Map[roadStartPointX, roadStartPointY].Status == MapStatus.Room ||
                    Manager.Map[roadStartPointX, roadStartPointY].Status == MapStatus.Road)
                {
                    if (InRoom) break;
                }
                //MapCreat---------------------------------------------------------------------------
                if (Map[roadStartPointX, roadStartPointY] == MapStatus.Wall)
                { InRoom = true; }
                if (Map[roadStartPointX, roadStartPointY] == MapStatus.Room ||
                    Map[roadStartPointX, roadStartPointY] == MapStatus.Road)
=======
>>>>>>> 27dcafa932f573b7918e14a2d5ae7aacb501b779
                {
                    InRoom = true;
                }
                if (Manager.Map[roadStartPointX, roadStartPointY].Status == MapStatus.Room ||
                    Manager.Map[roadStartPointX, roadStartPointY].Status == MapStatus.Road)
                {
                    if (InRoom) break;
                }
                //MapCreat---------------------------------------------------------------------------
                //if (Map[roadStartPointX, roadStartPointY] == MapStatus.Wall)
                //{ InRoom = true; }
                //if (Map[roadStartPointX, roadStartPointY] == MapStatus.Room ||
                //    Map[roadStartPointX, roadStartPointY] == MapStatus.Road)
                //{
                //    if (InRoom)
                //    {
                //        break;
                //    }
                //}
            }
            InRoom = false;
            while(roadStartPointY != meetPointY)
            {
                Manager.Map[roadStartPointX, roadStartPointY].Status = MapStatus.Road;//-------------------------------------
                Map[roadStartPointX, roadStartPointY] = MapStatus.Road;
                if (isUnder == true)
                {
                    roadStartPointY++;
                }
                else
                {
                    roadStartPointY--;
                }
                //部屋の当たった時点で終了させる
                //manager
                if (Manager.Map[roadStartPointX, roadStartPointY].Status == MapStatus.Wall)
                {
                    InRoom = true;
                }
                if (Manager.Map[roadStartPointX, roadStartPointY].Status == MapStatus.Room ||
                    Manager.Map[roadStartPointX, roadStartPointY].Status == MapStatus.Road)
<<<<<<< HEAD
                {
                    if (InRoom) break;
                }
                //MapCreat---------------------------------------------------------------------------
                if (Map[roadStartPointX, roadStartPointY] == MapStatus.Wall)
                { InRoom = true; }
                if (Map[roadStartPointX, roadStartPointY] == MapStatus.Room ||
                    Map[roadStartPointX, roadStartPointY] == MapStatus.Road)
=======
>>>>>>> 27dcafa932f573b7918e14a2d5ae7aacb501b779
                {
                    if (InRoom) break;
                }
                //MapCreat---------------------------------------------------------------------------
                //if (Map[roadStartPointX, roadStartPointY] == MapStatus.Wall)
                //{ InRoom = true; }                     
                //if (Map[roadStartPointX, roadStartPointY] == MapStatus.Room ||
                //    Map[roadStartPointX, roadStartPointY] == MapStatus.Road)
                //{
                //    if (InRoom)
                //    {
                //        break;
                //    }
                //}
            }
        }
        else
        {
            // 上下に伸ばして左右に伸ばすパターン
            InRoom = false;
            while (roadStartPointY != meetPointY)
            {
                Manager.Map[roadStartPointX, roadStartPointY].Status = MapStatus.Road;//-------------------------------------
                Map[roadStartPointX, roadStartPointY] = MapStatus.Road;
                if (isUnder == true)
                {
                    roadStartPointY++;
                }
                else
                {
                    roadStartPointY--;
                }
                //部屋の当たった時点で終了させる
                //manager
                if (Manager.Map[roadStartPointX, roadStartPointY].Status == MapStatus.Wall)
                {
                    InRoom = true;
                }
                if (Manager.Map[roadStartPointX, roadStartPointY].Status == MapStatus.Room ||
                    Manager.Map[roadStartPointX, roadStartPointY].Status == MapStatus.Road)
<<<<<<< HEAD
                {
                    if (InRoom) break;
                }
                //MapCreat---------------------------------------------------------------------------
                if (Map[roadStartPointX, roadStartPointY] == MapStatus.Wall)
                { InRoom = true; }
                if (Map[roadStartPointX, roadStartPointY] == MapStatus.Room ||
                    Map[roadStartPointX, roadStartPointY] == MapStatus.Road)
=======
>>>>>>> 27dcafa932f573b7918e14a2d5ae7aacb501b779
                {
                    if (InRoom) break;
                }
                //MapCreat---------------------------------------------------------------------------
                //if (Map[roadStartPointX, roadStartPointY] == MapStatus.Wall)
                //{ InRoom = true; }                     
                //if (Map[roadStartPointX, roadStartPointY] == MapStatus.Room ||
                //    Map[roadStartPointX, roadStartPointY] == MapStatus.Road)
                //{
                //    if (InRoom)
                //    {
                //        break;
                //    }
                //}
            }
            InRoom = false;
            while (roadStartPointX != meetPointX)
            {
                Manager.Map[roadStartPointX, roadStartPointY].Status = MapStatus.Road;//-------------------------------------
                Map[roadStartPointX, roadStartPointY] = MapStatus.Road;
                if (isRight == true)
                {
                    roadStartPointX--;
                }
                else
                {
                    roadStartPointX++;
                }
                //部屋の当たった時点で終了させる
                //manager
                if (Manager.Map[roadStartPointX, roadStartPointY].Status == MapStatus.Wall)
                {
                    InRoom = true;
                }
                if (Manager.Map[roadStartPointX, roadStartPointY].Status == MapStatus.Room ||
                    Manager.Map[roadStartPointX, roadStartPointY].Status == MapStatus.Road)
<<<<<<< HEAD
                {
                    if (InRoom) break;
                }
                //MapCreat---------------------------------------------------------------------------
                if (Map[roadStartPointX, roadStartPointY] == MapStatus.Wall)
                { InRoom = true; }
                if (Map[roadStartPointX, roadStartPointY] == MapStatus.Room ||
                    Map[roadStartPointX, roadStartPointY] == MapStatus.Road)
=======
>>>>>>> 27dcafa932f573b7918e14a2d5ae7aacb501b779
                {
                    if (InRoom) break;
                }
                //MapCreat---------------------------------------------------------------------------
                //if (Map[roadStartPointX, roadStartPointY] == MapStatus.Wall)
                //{ InRoom = true; }                     
                //if (Map[roadStartPointX, roadStartPointY] == MapStatus.Room ||
                //    Map[roadStartPointX, roadStartPointY] == MapStatus.Road)
                //{
                //    if (InRoom)
                //    {
                //        break;
                //    }
                //}
            }
        }//if()else
    }//void CreatRoadData

    //ダンジョンをオブジェクトに起こして生成する
    private void CreateDangeon()
    {
        //たくさんクローンオブジェクトが生成されてヒエラルキーが見にくくなるので親オブジェクトを作っておく
        GameObject Parent = new GameObject("Map");
        Parent.transform.position = new Vector3(0, 0, 0);
        for (int i = 0; i < MapHeight; i++)
        {
            for (int k = 0; k < MapWidth; k++)
            {
                //GameManager-----------------------------------------------------------------------
<<<<<<< HEAD
                //if (Manager.Map[k,i].Status == MapStatus.Wall)
                //{
                //    Instantiate(WallObject, new Vector3(k * CrackLength, 0, i * CrackLength), Quaternion.identity, Parent.transform);
                //    Instantiate(WallObject, new Vector3(k * CrackLength, 1, i * CrackLength), Quaternion.identity, Parent.transform);//2段置いて壁にしている
                //}
                //MapCreat--------------------------------------------------------------------------
                if (Map[k, i] == MapStatus.Wall)
=======
                if (Manager.Map[i, k].Status == MapStatus.Wall)
>>>>>>> 27dcafa932f573b7918e14a2d5ae7aacb501b779
                {
                    Instantiate(WallObject, new Vector3(k * CrackLength, 0, i * CrackLength), Quaternion.identity, Parent.transform);
                    Instantiate(WallObject, new Vector3(k * CrackLength, 1, i * CrackLength), Quaternion.identity, Parent.transform);//2段置いて壁にしている
                }
<<<<<<< HEAD
=======
                //MapCreat--------------------------------------------------------------------------
                //if (Map[k,i] == MapStatus.Wall)
                //{
                //    Instantiate(WallObject, new Vector3(k * CrackLength, 0, i * CrackLength), Quaternion.identity, Parent.transform);
                //    Instantiate(WallObject, new Vector3(k * CrackLength, 1, i * CrackLength), Quaternion.identity, Parent.transform);//2段置いて壁にしている
                //}
>>>>>>> 27dcafa932f573b7918e14a2d5ae7aacb501b779
                //-----------------------------------------------------------------------------------------------------------------------
                GameObject _Ground = Instantiate(GroundObject, new Vector3(k * CrackLength, -1, i * CrackLength), Quaternion.identity, Parent.transform);
                if (ceiling)
                {
                    Instantiate(GroundObject, new Vector3(k * CrackLength, 2, i * CrackLength), Quaternion.identity, Parent.transform);
                }
                _Ground.name = "Ground[" + i + "," + k + "]";
                GroundData G = _Ground.GetComponent<GroundData>();
                if (G != null)
                {
                    G.PosX = i;
                    G.PosY = k;
                    G.Status = MapStatus.Room;
                }
            }
        }
    }

    //Playerのポップ
    private void PlayerPop()
    {
        PlayerPopRoom = Random.Range(0, roomCount);
        //Gamemanager
        int PopPosX = Random.Range(Manager.Room[PlayerPopRoom].Pos.x, Manager.Room[PlayerPopRoom].Pos.x + Manager.Room[PlayerPopRoom].Size.x);
        int PopPosY = Random.Range(Manager.Room[PlayerPopRoom].Pos.y, Manager.Room[PlayerPopRoom].Pos.y + Manager.Room[PlayerPopRoom].Size.y);

        //Mapcreate
        //int _PopPosX = Random.Range(room[PlayerPopRoom].Pos.x, room[PlayerPopRoom].Pos.x + room[PlayerPopRoom].Size.x);
        //int _PopPosY = Random.Range(room[PlayerPopRoom].Pos.y, room[PlayerPopRoom].Pos.y + room[PlayerPopRoom].Size.y);
        Instantiate(Player, new Vector3(PopPosX * CrackLength, -0.5f, PopPosY * CrackLength), Quaternion.identity);

        PlayerPopPos = new Vector2Int(PopPosX, PopPosY);

    }

    //Enemyのポップ
    //Enemyとゴール同じ部屋の出現させてもいいのか？
    private void EnemyPop()
    {
        //ポップする部屋番号と位置座標
        int EnemyPopRoom;

        do
        {
            EnemyPopRoom = Random.Range(0, roomCount);
        }
        while (EnemyPopRoom == PlayerPopRoom);//プレイヤーと同じ部屋のポップしないようにする

        int PopPosX = Random.Range(Manager.Room[EnemyPopRoom].Pos.x, Manager.Room[EnemyPopRoom].Pos.x + Manager.Room[EnemyPopRoom].Size.x);
        int PopPosY = Random.Range(Manager.Room[EnemyPopRoom].Pos.y, Manager.Room[EnemyPopRoom].Pos.y + Manager.Room[EnemyPopRoom].Size.y);

        //int _PopPosX = Random.Range(room[EnemyPopRoom].Pos.x, room[EnemyPopRoom].Pos.x + room[EnemyPopRoom].Size.x);
        //int _PopPosY = Random.Range(room[EnemyPopRoom].Pos.y, room[EnemyPopRoom].Pos.y + room[EnemyPopRoom].Size.y);

        Instantiate(Enemy, new Vector3(PopPosX * CrackLength, -0.5f, PopPosY * CrackLength), Quaternion.identity);

    }

    //ゴールの生成
    void GorlPop()
    {
        int GoalPopRoom;

        do
        {
            GoalPopRoom = Random.Range(0, roomCount);
        }
        while (GoalPopRoom == PlayerPopRoom);

        int PopPosX = Random.Range(Manager.Room[GoalPopRoom].Pos.x, Manager.Room[GoalPopRoom].Pos.x + Manager.Room[GoalPopRoom].Size.x);
        int PopPosY = Random.Range(Manager.Room[GoalPopRoom].Pos.y, Manager.Room[GoalPopRoom].Pos.y + Manager.Room[GoalPopRoom].Size.y);

        //int PopPosX = (room[GoalPopRoom].Pos.x + room[GoalPopRoom].Pos.x + room[GoalPopRoom].Size.x) / 2;
        //int PopPosY = (room[GoalPopRoom].Pos.y + room[GoalPopRoom].Pos.y + room[GoalPopRoom].Size.y) / 2;

        Instantiate(Goal, new Vector3(PopPosX * CrackLength, 1, PopPosY * CrackLength), Quaternion.Euler(45, 0, 45)); //identityだと回転軸が０，０，０に戻されてしまう

        GorlPopPos = new Vector2Int(PopPosX, PopPosY);
    }
}