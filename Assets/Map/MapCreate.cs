using System.Collections;
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
    [SerializeField] private float CrackLength = 0.0f;      //オブジェクト同士の隙間距離
    [SerializeField] private int EnemySize = 0;             //エネミーの数

    private GameObject Parent;//大量のオブジェが生産されるのでこの階層下で生成させる

    //Mapのデータ
    private MapStatus[,] Map;
    private ROOM[] room;

    //Mapの大きさ
    private int MapWidth = 50;
    private int MapHeight = 50;

    private int roomCount;          //部屋の数 (10,15)
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
        //MakeGorl();
    }
    //Mapデータのリセット
    private void ResetMapData()
    {
        //GameManager------------------------------------------------------------------------------------------------------------------------
        Manager.Map = new MapData[MapHeight, MapWidth];
        Manager.roomData = new RoomData[MapHeight, MapWidth];
        Manager.roadData = new RoadData[MapHeight, MapWidth];
        for (int i = 0; i < MapHeight; i++)
        {
            for (int k = 0; k < MapWidth; k++)
            {
                Manager.Map[i, k] = new MapData();
                Manager.Map[i, k].Status = MapStatus.Wall;

                Manager.roomData[i, k] = new RoomData();
                Manager.roadData[i, k] = new RoadData();
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

        meetPointsX = new int[meetPointCount];
        meetPointsY = new int[meetPointCount];
        for (int i = 0; i < meetPointsX.Length; i++)
        {
            meetPointsX[i] = Random.Range(MapWidth / 4, MapWidth * 3 / 4);
            meetPointsY[i] = Random.Range(MapHeight / 4, MapHeight * 3 / 4);

            Map[meetPointsY[i], meetPointsX[i]] = MapStatus.Road;
        }

        for (int i = 0; i < roomCount; i++)
        {
            bool isRoad;
            int roomHeight = 0;
            int roomWidth = 0;
            int roomPointX = 0;
            int roomPointY = 0;

            int roadStartPointX = 0;
            int roadStartPointY = 0;

            for (int k = 0; k < 5; k++)//5回やって全部重なっていたらもうあきらめる(そうでないと無限ループにい入る可能性がある)
            {
                roomHeight = Random.Range(roomMinHeight, roomMaxHeight);    //高さ
                roomWidth = Random.Range(roomMinWidth, roomMaxWidth);      //横幅
                roomPointX = Random.Range(2, MapWidth - roomMaxWidth - 2);  //部屋のｘ位置
                roomPointY = Random.Range(2, MapWidth - roomMaxWidth - 2);  //部屋のｙ位置

                if (!IsBooking(roomHeight, roomWidth, roomPointX, roomPointY))
                { break; }
            }

            roadStartPointX = Random.Range(roomPointX, roomPointX + roomWidth);  //部屋に通路を繋ぐ位置
            roadStartPointY = Random.Range(roomPointY, roomPointY + roomHeight);

            isRoad = CreateRoomData(roomHeight, roomWidth, roomPointX, roomPointY); //部屋に通路を引くかどうか判断する

            //部屋のデータ
            //GameManager------------------------------------------------------------------------------------------------------------------------
            for (int a = roomPointY; a < roomPointY + roomHeight; a++)
            {
                for (int b = roomPointX; b < roomPointX + roomWidth; b++)
                {
                    Manager.Map[a, b].Pos = new Vector2Int(b, a);
                    Manager.Map[a, b].Status = MapStatus.Room;
                    //Manager.roomData[a, b].roomNum = i;
                    //Manager.roomData[a, b].Height = roomHeight;
                    //Manager.roomData[a, b].Width = roomWidth;
                }
            }
            //MapCreate------------------------------------------------------------------------------------------------------------------------
            room[i] = new ROOM();
            room[i].Room_num = i;
            room[i].Pos = new Vector2Int(roomPointX, roomPointY);
            room[i].Size = new Vector2Int(roomWidth, roomHeight);
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
        for (int i = 0 - 1; i < roomHeight + 1; i++)
        {
            for (int k = 0 - 1; k < roomWidth + 1; k++)
            {
                if (roomPointY + i < 0 || roomPointX + k < 0)
                {
                    continue;
                }
                if (Map[roomPointY + i, roomPointX + k] == MapStatus.Room)
                {
                    return true;
                }
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
                if (Map[roomPointY + i, roomPointX + k] == MapStatus.Room || //他の部屋に重なっている
                    Map[roomPointY + i, roomPointX + k] == MapStatus.Road) 　//または道にまたがっていたら道を新たには作らない
                {
                    Map[roomPointY + i, roomPointX + k] = MapStatus.Room;
                    isRoad = true;
                }
                else
                {
                    Map[roomPointY + i, roomPointX + k] = MapStatus.Room;   //部屋のステータスを代入する
                }
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
        //全部同じパターンだと単調になってしますから上から延ばすか横から延ばすかの2パターン用意する

        bool IsRoom = false;
        bool IsContinue = true;
        if (Random.Range(0, 2) == 0)
        {

            IsRoom = false;
            IsContinue = true;
            //while (roadStartPointX != meetPointX)
            while (IsContinue)
            {
                Map[roadStartPointY, roadStartPointX] = MapStatus.Road;
                if (isRight == true)
                {
                    roadStartPointX--;
                }
                else
                {
                    roadStartPointX++;
                }

                //部屋に当たった時点で終了させる
                IsContinue = CheckRoom(roadStartPointY, roadStartPointX, meetPointX, "X", IsRoom);
                //CheckRoom(roadStartPointY, roadStartPointX, meetPointX, "X", IsRoom,IsContinue);

                //if (Map[roadStartPointY, roadStartPointX] == MapStatus.Wall)
                //{ IsRoom = true; }
                //if (Map[roadStartPointY, roadStartPointX] == MapStatus.Room)
                //{
                //    if (IsRoom)
                //    {
                //        break;
                //    }
                //}
            }
            IsRoom = false;
            IsContinue = true;
            while (IsContinue)
            {
                Map[roadStartPointY, roadStartPointX] = MapStatus.Road;
                if (isUnder == true)
                {
                    roadStartPointY++;
                }
                else
                {
                    roadStartPointY--;
                }
                //部屋の当たった時点で終了させる
                IsContinue = CheckRoom(roadStartPointY, roadStartPointX, meetPointY, "Y", IsRoom);
                //if (Map[roadStartPointY, roadStartPointX] == MapStatus.Wall)
                //{ IsRoom = true; }
                //if (Map[roadStartPointY, roadStartPointX] == MapStatus.Room)
                //{
                //    if (IsRoom)
                //    {
                //        break;
                //    }
                //}
            }
        }
        else
        {
            IsRoom = false;
            IsContinue = true;
            while (IsContinue)
            {
                Map[roadStartPointY, roadStartPointX] = MapStatus.Road;
                if (isUnder == true)
                {
                    roadStartPointY++;
                }
                else
                {
                    roadStartPointY--;
                }
                //部屋の当たった時点で終了させる               
                IsContinue = CheckRoom(roadStartPointY, roadStartPointX, meetPointY, "Y", IsRoom);

                //if (Map[roadStartPointY, roadStartPointX] == MapStatus.Wall)
                //{ IsRoom = true; }
                //if (Map[roadStartPointY, roadStartPointX] == MapStatus.Room)
                //{
                //    if (IsRoom)
                //    {
                //        break;
                //    }
                //}
            }
            IsRoom = false;
            IsContinue = true;
            while (IsContinue)
            {

                Map[roadStartPointY, roadStartPointX] = MapStatus.Road;
                if (isRight == true)
                {
                    roadStartPointX--;
                }
                else
                {
                    roadStartPointX++;
                }
                //部屋の当たった時点で終了させる
                IsContinue = CheckRoom(roadStartPointY, roadStartPointX, meetPointX, "X", IsRoom);
                //if (Map[roadStartPointY, roadStartPointX] == MapStatus.Wall)
                //{ IsRoom = true; }
                //if (Map[roadStartPointY, roadStartPointX] == MapStatus.Room)
                //{
                //    if (IsRoom)
                //    {
                //        break;
                //    }
                //}
            }
        }//if()else
    }//void CreatRoadData

    bool CheckRoom(int PointY, int PointX, int meetPoint, string dis, bool IsRoom)
    {
        if (dis == "X") { if (PointX == meetPoint) return false; }
        if (dis == "Y") { if (PointY == meetPoint) return false; }
        
        //Debug.Log("X:" + PointX + " Y:" + PointY + " END:" + meetPoint + " room: " + IsRoom);
        if (Map[PointY, PointX] == MapStatus.Wall)
        {
            IsRoom = true;
            return true;
        }
        else if (Map[PointY, PointX] == MapStatus.Room)
        {
            if (IsRoom)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else { return true; }
    }

    //部屋に当たったらwhileを抜ける関数
    //void CheckRoom(int PointY, int PointX,int meetPoint,string dis, bool IsRoom, bool IsContinue)
    //{
    //    if (dis == "X") { if (PointX == meetPoint) IsContinue = false; }
    //    if (dis == "Y") { if (PointY == meetPoint) IsContinue = false; }
    //
    //    Debug.Log("X:" + PointX+" Y:"+ PointY +" END:" + meetPoint + " room: "+ IsRoom +" *while: " + IsContinue);
    //    if (Map[PointY, PointX] == MapStatus.Wall)
    //    { IsRoom = true; }
    //    if (Map[PointY, PointX] == MapStatus.Room)
    //    {
    //        if (IsRoom)
    //        {
    //            IsContinue = false;
    //        }
    //    }
    //}

    //ダンジョンをオブジェクトに起こして生成する
    private void CreateDangeon()
    {
        for (int i = 0; i < MapHeight; i++)
        {
            for (int k = 0; k < MapWidth; k++)
            {
                if (Parent != null) //親があったら直下に作る
                {
                    if (Map[i, k] == MapStatus.Wall)
                    {
                        Instantiate(WallObject, new Vector3(k * CrackLength, 0, i * CrackLength), Quaternion.identity, Parent.transform);
                        Instantiate(WallObject, new Vector3(k * CrackLength, 1, i * CrackLength), Quaternion.identity, Parent.transform); //力技で高さをいじってる
                    }
                    GameObject _wallTmp = Instantiate(GroundObject, new Vector3(k * CrackLength, -1, i * CrackLength), Quaternion.identity, Parent.transform);
                    GroundData G = _wallTmp.GetComponent<GroundData>();
                    G.PosX = k;
                    G.PosY = i;
                    //G.Status = MapStatus.Room;
                }
                else
                {
                    //たくさんクローンオブジェクトが生成されてヒエラルキーが見にくくなるので親オブジェクトを作っておく
                    Parent = new GameObject("Map");
                    Parent.transform.position = new Vector3(0, 0, 0);
                    if (Map[i, k] == MapStatus.Wall)
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
        Instantiate(Player, new Vector3(PopPosX * CrackLength, 1, PopPosY * CrackLength), Quaternion.identity);
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

        int PopPosX = Random.Range(room[EnemyPopRoom].Pos.x, room[EnemyPopRoom].Pos.x + room[EnemyPopRoom].Size.x);
        int PopPosY = Random.Range(room[EnemyPopRoom].Pos.y, room[EnemyPopRoom].Pos.y + room[EnemyPopRoom].Size.y);

        Instantiate(Enemy, new Vector3(PopPosX * CrackLength, 1, PopPosY * CrackLength), Quaternion.identity);
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

        int PopPosX = (room[GoalPopRoom].Pos.x + room[GoalPopRoom].Pos.x + room[GoalPopRoom].Size.x) / 2;
        int PopPosY = (room[GoalPopRoom].Pos.y + room[GoalPopRoom].Pos.y + room[GoalPopRoom].Size.y) / 2;

        Instantiate(Goal, new Vector3(PopPosX * CrackLength, 1, PopPosY * CrackLength), Quaternion.Euler(45, 0, 45)); //identityだと回転軸が０，０，０に戻されてしまう
    }

    //ゴールを新たに作る
    void MakeGorl()
    {
        ////部屋の大きさは固定vecter2(3,3)
        //int posX;
        //int posY;

        //do
        //{
        //    posX = Random.Range(2, MapWidth - 3 - 2);
        //    posY = Random.Range(2, MapWidth - 3 - 2);
        //}
        //while (IsBooking(3, 3, posX, posY));

        //int roadStartPointX = Random.Range(posX, posX + 1);  //部屋に通路を繋ぐ位置
        //int roadStartPointY = Random.Range(posY, posY + 1);

        //bool Makeroom = CreateRoomData(3, 3, posX, posY);
        //if (Makeroom == false)
        //{
        //    CreateRoadData(roadStartPointX, roadStartPointY, meetPointsX[Random.Range(0, 0)], meetPointsY[Random.Range(0, 0)]);
        //}
        //Instantiate(Goal, new Vector3(posX * CrackLength, 1, posY * CrackLength), Quaternion.Euler(45, 0, 45)); //identityだと回転軸が０，０，０に戻されてしまう


        bool isRoad;
        int roomHeight = 0;
        int roomWidth = 0;
        int roomPointX = 0;
        int roomPointY = 0;

        int roadStartPointX = 0;
        int roadStartPointY = 0;

        while (true)
        {
            roomHeight = 3;     //高さ
            roomWidth = 3;      //横幅
            roomPointX = Random.Range(2, MapWidth - 3 - 2);  //部屋のｘ位置
            roomPointY = Random.Range(2, MapWidth - 3 - 2);  //部屋のｙ位置

            if (!IsBooking(roomHeight, roomWidth, roomPointX, roomPointY))
            { break; }
        };

        roadStartPointX = Random.Range(roomPointX, roomPointX + roomWidth);  //部屋に通路を繋ぐ位置
        roadStartPointY = Random.Range(roomPointY, roomPointY + roomHeight);

        isRoad = CreateRoomData(roomHeight, roomWidth, roomPointX, roomPointY); //部屋に通路を引くかどうか判断する

        if (isRoad == false)//他の部屋と重なっていなかったら通路を作る
        {
            CreateRoadData(roadStartPointX, roadStartPointY, meetPointsX[Random.Range(0, 0)], meetPointsY[Random.Range(0, 0)]);
        }
        Instantiate(Goal, new Vector3(roomPointX * CrackLength, 1, roomPointY * CrackLength), Quaternion.Euler(45, 0, 45)); //identityだと回転軸が０，０，０に戻されてしまう

    }

}