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

    private EnemyAutoSurch _enemyAutoSurch;
    private enemy_script _enemyScript;
    
    public EnemyStatus NowStatus;

    private void Start()
    {
        _enemyAutoSurch = GetComponent<EnemyAutoSurch>();
        _enemyScript = GetComponent<enemy_script>();
    }



    private void Update()
    {
        if (NowStatus == EnemyStatus.AutoSurch)
        {
            _enemyAutoSurch.IsAutoSurch = true;
        }
        else { _enemyAutoSurch.IsAutoSurch = false; }
    }



}
