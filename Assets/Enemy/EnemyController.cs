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

public enum Motion
{
    Idle = 0,
    Walk = 1,
    Run = 2,
    WalkLeft = 11,
    WalkRight = 12,
    RunLeft = 21,
    RunRight = 22,

    SpotLeft = -1,
    SpotRigh = -2,

    Back = -9
}

public class EnemyController : MonoBehaviour {

    private EnemyAutoSurch _enemyAutoSurch;
    private enemy_script _enemyScript;
    
    public EnemyStatus NowStatus;
    private Animator anim;
    public Motion motion;

    private void Awake()
    {
        _enemyAutoSurch = GetComponent<EnemyAutoSurch>();
        _enemyScript = GetComponent<enemy_script>();
        motion = Motion.Idle;
        anim = GetComponent<Animator>();

        NowStatus = EnemyStatus.AutoSurch;
    }



    private void Update()
    {
        anim.SetInteger("motionNum", (int)motion); //Int

        if (NowStatus == EnemyStatus.AutoSurch)
        {
            _enemyAutoSurch.IsAutoSurch = true;
        }
        else { _enemyAutoSurch.IsAutoSurch = false; }
    }



}
