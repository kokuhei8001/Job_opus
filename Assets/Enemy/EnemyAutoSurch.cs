using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyAutoSurch : MonoBehaviour {

    //他のスクリプトへのリンク
    private GameObject _Manager;
    private GameManager _GameManager;
    private EnemyController _enemyManager;
    private MapCreate _mapcreat;
    
    //目的地までのルート
    private List<Vector2Int> Rout = new List<Vector2Int>();
    private Vector3 TargetPos; //次のブロックのPosition
    
    //自動探索のルートをリセットするかどうか
    public bool IsReset = true;

    private void Start()
    {
        //必要なスクリプトを取得する
        _Manager = GameObject.Find("GameManager");
        _GameManager = _Manager.GetComponent<GameManager>();
        _mapcreat = _Manager.GetComponent<MapCreate>();
        _enemyManager = GetComponent<EnemyController>();

    }

    private void Update()
    {
        //もし自動探索状態なら
        if (_enemyManager.NowStatus == EnemyStatus.AutoSurch)
        {
            //リセットするなら
            if (IsReset)
            {
                RoutReset();
                if (Rout.Count != 0) { TargetPos = _enemyManager.FindNextTarget(Rout[Rout.Count - 1].x, Rout[Rout.Count - 1].y); }
                transform.LookAt(TargetPos);
                IsReset = false;
            }

            //マスに付いたら次のマスへ進む
            if (TargetPos.x - 0.2f < transform.position.x && transform.position.x < TargetPos.x + 0.2f){
                if (TargetPos.z - 0.2f < transform.position.z && transform.position.z < TargetPos.z + 0.2f){
                    if (Rout.Count != 0)
                    {
                        Rout.RemoveAt(Rout.Count - 1);
                        if (Rout.Count != 0)
                        {
                            //次のマスを探す
                            TargetPos = _enemyManager.FindNextTarget(Rout[Rout.Count - 1].x, Rout[Rout.Count - 1].y);
                        }
                        else
                        {
                            //もしゴールに付いたならアイドリング状態にする
                            _enemyManager.NowStatus = EnemyStatus.Idling;
                        }
                    }
                }
            }

            //マイフレームLookAtしないと何かにぶつかって方向がずれた時永遠と進んでしまう。
            transform.LookAt(TargetPos);
        }

    }

    //ランダムな場所を取得してそこまでのルートを記録する。
    public void RoutReset()
    {
        Rout = new List<Vector2Int>();
        int TargetRoom = Random.Range(0, _mapcreat.roomCount);
        int TargetX = Random.Range(_GameManager.Room[TargetRoom].Pos.x, _GameManager.Room[TargetRoom].Pos.x + _GameManager.Room[TargetRoom].Size.x);
        int TargetY = Random.Range(_GameManager.Room[TargetRoom].Pos.y, _GameManager.Room[TargetRoom].Pos.y + _GameManager.Room[TargetRoom].Size.y);

        //int TargetRoom = Random.Range(0, _mapcreat.roomCount);
        //int TargetX = Random.Range(_mapcreat.room[TargetRoom].Pos.x, _mapcreat.room[TargetRoom].Pos.x + _mapcreat.room[TargetRoom].Size.x);
        //int TargetY = Random.Range(_mapcreat.room[TargetRoom].Pos.y, _mapcreat.room[TargetRoom].Pos.y + _mapcreat.room[TargetRoom].Size.y);

        Vector2Int End = new Vector2Int(TargetX,TargetY);//目的地の座標
        Rout = _GameManager.ASter(this.gameObject, End);        
    }
}
