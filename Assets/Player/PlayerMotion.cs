using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotion : MonoBehaviour {

    private Animator anim;
    private bool IsMoov;
    private bool IsRun;

    private void Start()
    {
        anim = GetComponent<Animator>();
        IsMoov = false;
        IsRun = false;
    }

    private void Update()
    {
        //Wを押したら歩く
        if (Input.GetKeyDown(KeyCode.W))
        {
            IsMoov = true;
            anim.SetBool("IsWalk", true);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            IsMoov = false;
            anim.SetBool("IsWalk", false);
            anim.SetBool("IsRun", false);
        }
        //Eを押したら走る
        if (IsMoov)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                IsRun = true;
                anim.SetBool("IsRun", true);
                anim.SetBool("IsWalk", false);
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                IsRun = false;
                anim.SetBool("IsWalk", true);
                anim.SetBool("IsRun", false);
            }
        }






        //歩いていたり走っていなかったらその場で回る
        if (!IsMoov)
        {
            if (Input.GetKey(KeyCode.D))
            {
                anim.SetTrigger("IsTurnRightTrigger");
            }
            if (Input.GetKey(KeyCode.A))
            {
                anim.SetTrigger("IsTurnLeftTrigger");
            }
        }
    }
}
