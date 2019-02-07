using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotion : MonoBehaviour {

    private Animator anim;
    private bool IsMoov;
    
    Motion status;

    private void Start()
    {
        status = 0;
        anim = GetComponent<Animator>();
        IsMoov = false;
    }

    private void Update()
    {

        anim.SetInteger("motionNum", (int)status); //Int

        //歩いて進む
        if (Input.GetKey(KeyCode.W))
        {
            IsMoov = true;
            status = Motion.Walk;

            if (Input.GetKey(KeyCode.LeftShift))//shift押したら走る
            {
                status = Motion.Run;

                //走ってる状態で左右に曲がる
                if (Input.GetKey(KeyCode.D))   
                {
                    status = Motion.RunRight;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    status = Motion.RunLeft;
                }
            }
            else
            {
                //歩いて左右に曲がる
                if (Input.GetKey(KeyCode.D))
                {
                    status = Motion.WalkRight;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    status = Motion.WalkLeft;
                }
            }
        }
        else//なにも押されていなかったらアイドリング状態
        {
            status = Motion.Idle;
        }

        //Wを離したら
        if (Input.GetKeyUp(KeyCode.W))
        {
            IsMoov = false;
        }

        if (status == Motion.Idle)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                anim.SetTrigger("IsTurnRightTrigger");
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                anim.SetTrigger("IsTurnLeftTrigger");
            }
        }
    }
}
