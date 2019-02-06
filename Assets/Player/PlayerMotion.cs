using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotion : MonoBehaviour {

    private Animator anim;
    private bool Ispush;

    private void Start()
    {
        anim = GetComponent<Animator>();
        Ispush = false;
    }

    private void Update()
    {
        //Wを押したら歩く
        if (Input.GetKeyDown(KeyCode.W))
        {
            Ispush = true;
            anim.SetBool("IsWalk", true);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            Ispush = false;
            anim.SetBool("IsWalk", false);
        }
        //Eを押したら走る
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ispush = true;
            anim.SetBool("IsRun", true);
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            Ispush = false;
            anim.SetBool("IsRun", false);
        }

        if (!Ispush)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                anim.SetTrigger("IsTurnLeftTrigger");
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                anim.SetTrigger("IsTurnRightTrigger");
            }
        }
    
    }
}
