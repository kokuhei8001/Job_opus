using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyAutoSurch : MonoBehaviour {

    //他のスクリプトへのリンク
    private GameObject _Manager;
    private GameManager _gameManager;
    private EnemyController _enemyManager;
    private MapCreate _mapcreat;
    
    //目的地までのルート
    private List<Vector2Int> Rout = new List<Vector2Int>();
    private Vector3 TargetPos; //次のブロックのPosition


    private void Start()
    {
        //必要なスクリプトを入れていく
        _Manager = GameObject.Find("GameManager");
        _gameManager = _Manager.GetComponent<GameManager>();
        _mapcreat = _Manager.GetComponent<MapCreate>();
        _enemyManager = GetComponent<EnemyController>();

        //初回のルートを決めて実行する
        RoutReset();
        if (Rout.Count != 0) { TargetPos = FindNextTarget(Rout[Rout.Count - 1].x, Rout[Rout.Count - 1].y); }
        transform.LookAt(TargetPos);
    }

    private void Update()
    {
        if (_enemyManager.NowStatus == EnemyStatus.AutoSurch)
        {
            //マスに付いたら次のマスへ進む
            if (TargetPos.x - 0.2f < transform.position.x && transform.position.x < TargetPos.x + 0.2f){
                if (TargetPos.z - 0.2f < transform.position.z && transform.position.z < TargetPos.z + 0.2f){
                    if (Rout.Count != 0)
                    {
                        Rout.RemoveAt(Rout.Count - 1);
                        if (Rout.Count != 0)
                        {
                            TargetPos = FindNextTarget(Rout[Rout.Count - 1].x, Rout[Rout.Count - 1].y);
                            transform.LookAt(TargetPos);
                        }
                        else
                        {
                            //ルートの終点に到達したらアイドリング状態にする
                            //ここでずっと更新されてるから追跡されない
                            _enemyManager.NowStatus = EnemyStatus.Idling;
                        }
                    }
                }
            }
        }

    }

    //ランダムな場所を取得してそこまでのルートを記録する。
    private void RoutReset()
    {

        Rout = new List<Vector2Int>();

        int TargetRoom = Random.Range(0, _mapcreat.roomCount);
        int TargetX = Random.Range(_mapcreat.room[TargetRoom].Pos.x, _mapcreat.room[TargetRoom].Pos.x + _mapcreat.room[TargetRoom].Size.x);
        int TargetY = Random.Range(_mapcreat.room[TargetRoom].Pos.y, _mapcreat.room[TargetRoom].Pos.y + _mapcreat.room[TargetRoom].Size.y);

        Vector2Int End = new Vector2Int(TargetX,TargetY);//目的地の座標
        Rout = _gameManager.ASter(this.gameObject, End);        
    }

    private Vector3 FindNextTarget(int i,int k)
    {
        GameObject tempObj = GameObject.Find("Ground[" + k + "," + i +"]");
        if (tempObj != null)
        {
            Vector3 answer = new Vector3(tempObj.transform.position.x, this.gameObject.transform.position.y, tempObj.transform.position.z);
            return answer;
        }
        else
        {
            Debug.Log("エネミールートにエラー");
            return new Vector3(0, 100, 0);
        }
    }
}
