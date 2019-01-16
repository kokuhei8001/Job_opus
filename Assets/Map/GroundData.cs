using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//全てのGroundObjectに付けるスクリプト
//キャラクターがRayと飛ばしてこれにアクセスして現在の位置情報を取得する

public class GroundData : MonoBehaviour {

    [SerializeField] MapCreate _script;
    [SerializeField] MapData Status;
    private MapData[,] _map;

    //Mapの情報
    int roomNum;
    int PosX;    //位置座標
    int PosY;
    int Width;   //部屋のサイズ
    int Height;

    int RoadCount; //通路がつながっている数
    int[] RoadPosX;//部屋の座標位置
    int[] RoadPosY;

    private void Start()
    {
        //_map = _script.Map;
    }
}
