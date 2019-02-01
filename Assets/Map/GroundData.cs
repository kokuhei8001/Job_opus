using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//全てのGroundObjectに付けるスクリプト
//キャラクターがRayと飛ばしてこれにアクセスして現在の位置情報を取得する

public class GroundData : MonoBehaviour {
    //基本情報
    [SerializeField] public int PosX;
    [SerializeField] public int PosY;
    [SerializeField] public MapStatus Status = MapStatus.Ground;
    [SerializeField] public bool rout = false;
}
