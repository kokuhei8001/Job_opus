using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//全てのGroundObjectに付けるスクリプト
//キャラクターがRayと飛ばしてこれにアクセスして現在の位置情報を取得する

public class GroundData : MonoBehaviour {
    //基本情報
    int PosX;
    int PosY;
    MapData Status;
    
    //Roomの情報
    int roomNum;
    int Width;   
    int Height;
    int RoadCount = 0; //通路がつながっている数
    Vector2Int[] RoadPos = new Vector2Int[10]; //通路の開始位置 //メモリがもったいない

    //Raodの情報
    int StartRoomNum;
    int EndRoomNum;
    Vector2Int StartPos;
    Vector2Int EndPos;
}
