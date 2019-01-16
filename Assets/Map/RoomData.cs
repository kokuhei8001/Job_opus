using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData : MonoBehaviour {

    int PosX;
    int PosY;
    
    //Mapの情報
    int roomNum;
    int roomPosX;    //位置座標
    int roomPosY;
    int Width;   //部屋のサイズ
    int Height;

    int RoadCount; //通路がつながっている数
    int[] RoadPosX;//部屋の座標位置
    int[] RoadPosY;

}
