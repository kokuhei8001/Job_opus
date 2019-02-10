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
    private EnemyHunt _enemyScript;
    
    public EnemyStatus NowStatus;
    private Animator anim;
    private Motion motion;

    public bool debug = false;

    private void Awake()
    {
        _enemyAutoSurch = GetComponent<EnemyAutoSurch>();
        _enemyScript = GetComponent<EnemyHunt>();
        anim = GetComponent<Animator>();

        NowStatus = EnemyStatus.AutoSurch;
    }
    
    private void Update()
    {
        switch (NowStatus)
        {
            case EnemyStatus.Idling:
                motion = Motion.Idle;
                break;
            case EnemyStatus.Hunt:
                motion = Motion.Run;
                break;
            case EnemyStatus.AutoSurch:
                motion = Motion.Walk;
                break;
            case EnemyStatus.GessHunt:
                motion = Motion.Run;
                break;
        }

        if (debug)
        {
            Debug.Log(NowStatus);
        }

        anim.SetInteger("motionNum", (int)motion); //Int

    }
}
