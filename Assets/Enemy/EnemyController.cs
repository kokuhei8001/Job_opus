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
    Idle,
    Walk,
    Run,
    TurnLeft
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
        switch (motion)
        {
            case Motion.Idle:
                anim.SetBool("IsRun", false);
                anim.SetBool("IsWalk", false);
                anim.SetBool("IsTurnLeft", false);
                break;
            case Motion.Run:
                anim.SetBool("IsRun", true);
                break;
            case Motion.Walk:
                anim.SetBool("IsWalk", true);
                break;
            case Motion.TurnLeft:
                anim.SetBool("IsTurnLeft", true);
                break;
        }

        if (NowStatus == EnemyStatus.AutoSurch)
        {
            _enemyAutoSurch.IsAutoSurch = true;
        }
        else { _enemyAutoSurch.IsAutoSurch = false; }
    }



}
