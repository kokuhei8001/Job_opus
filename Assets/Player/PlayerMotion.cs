using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotion : MonoBehaviour {

    private Animator anim;
    
    Motion status;

    private void Start()
    {
        status = 0;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {

        anim.SetInteger("motionNum", (int)status); //Int

        //歩いて進む
        if (Input.GetKey(KeyCode.W))
        {
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
                else if (Input.GetKey(KeyCode.A))
                {
                    status = Motion.WalkLeft;
                }
                else
                {
                    status = Motion.Walk;
                }
            }
        }
        else//なにも押されていなかったらアイドリング状態
        {
            status = Motion.Idle;
        }

        if (status == Motion.Idle)
        {
            if (Input.GetKey(KeyCode.D))
            {
                status = Motion.SpotRigh;
            }
            if (Input.GetKey(KeyCode.A))
            {
                status = Motion.SpotLeft;
            }

            if (Input.GetKey(KeyCode.S))
            {
                status = Motion.Back;
            }
        }
    }
}
