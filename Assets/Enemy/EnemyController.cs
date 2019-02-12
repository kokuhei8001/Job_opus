using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStatus
{
    Idling,    //待機状態
    AutoSurch, //自動探索状態
    Hunt,      //追いかけている
    GessHunt,  //見失ったが最後の場所まで追いかける
    Down    　 //気絶状態
}

public class EnemyController : MonoBehaviour {

    private MapCreate _mapCreat;
    private EnemyAutoSurch _enemyAutoSurch;
    
    //Enemyの状態
    public EnemyStatus NowStatus;　//現在のステータス
    private Animator anim; 
    private Motion motion; //モーションの状態

    //待機状態だったら3秒で自動探索に切り替える
    private float AutoSurchTime = 0;


    private void Awake()
    {
        _mapCreat = GameObject.Find("GameManager").GetComponent<MapCreate>();
        _enemyAutoSurch = GetComponent<EnemyAutoSurch>();
        anim = GetComponent<Animator>();

        NowStatus = EnemyStatus.AutoSurch;
    }
    
    private void Update()
    {
        //エネミーの状態制御
        switch (NowStatus)
        {
            case EnemyStatus.Idling:
                _enemyAutoSurch.IsReset = true; //自動探索のルートをリセットする
                motion = Motion.Idle;

                //3秒たったら自動探索に切り替える
                AutoSurchTime += Time.deltaTime;
                if (AutoSurchTime > 3)
                {
                    AutoSurchTime = 0;
                    NowStatus = EnemyStatus.AutoSurch;
                }
                break;

            case EnemyStatus.Hunt:
                motion = Motion.Run;
                _enemyAutoSurch.IsReset = true;
                break;

            case EnemyStatus.AutoSurch:
                motion = Motion.Walk;
                break;

            case EnemyStatus.GessHunt:
                _enemyAutoSurch.IsReset = true;
                motion = Motion.Run;
                break;
        }

        //マイフレームモーションを更新する
        anim.SetInteger("motionNum", (int)motion);

        //バグによるマップ外へ行った時の復帰処理
        if (transform.position.x > 50 || transform.position.z > 50 || transform.position.x < 0 || transform.position.z < 0)
        {
            int ResetRoom = Random.Range(0, _mapCreat.roomCount);
            int PosX = Random.Range(_mapCreat.room[ResetRoom].Pos.x, _mapCreat.room[ResetRoom].Pos.x + _mapCreat.room[ResetRoom].Size.x);
            int PosY = Random.Range(_mapCreat.room[ResetRoom].Pos.y, _mapCreat.room[ResetRoom].Pos.y + _mapCreat.room[ResetRoom].Size.y);

            transform.position = new Vector3(PosX , -0.5f, PosY);
            NowStatus = EnemyStatus.Idling;
        }
    }

    //自動歩行などで次のマスの位置情報をゲットする
    public Vector3 FindNextTarget(int i, int k)
    {
        GameObject tempObj = GameObject.Find("Ground[" + k + "," + i + "]");//次のマスを探す
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
