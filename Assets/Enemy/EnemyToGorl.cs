using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyToGorl : MonoBehaviour {

    private List<int> RoutNum = new List<int>();

    private Animator _anim;
    private Motion _motion;

    private Vector3 _targetPos;
    private int NowNum = 0;
    
    private void Start()
    {
        _motion = Motion.Idle;
        _anim = GetComponent<Animator>();
        for (int i = 0; i < 9; i++)
        {
            RoutNum.Add(i);
        }

        _targetPos = FindNextTarget(RoutNum[0]);
        transform.LookAt(_targetPos);
        _motion = Motion.Walk;
    }


    private void Update()
    {
        switch (_motion)
        {
            case Motion.Idle:
                _anim.SetBool("IsRun", false);
                _anim.SetBool("IsWalk", false);
                _anim.SetBool("IsTurnLeft", false);
                break;
            case Motion.Run:
                _anim.SetBool("IsRun", true);
                break;
            case Motion.Walk:
                _anim.SetBool("IsWalk", true);
                break;
            case Motion.TurnLeft:
                _anim.SetBool("IsTurnLeft", true);
                break;
        }


        if (_targetPos.x - 0.2f < transform.position.x && transform.position.x < _targetPos.x + 0.2f)
        {
            if (_targetPos.z - 0.2f < transform.position.z && transform.position.z < _targetPos.z + 0.2f)
            {
                if (RoutNum.Count != 0)
                {
                    RoutNum.RemoveAt(0);
                    _targetPos = FindNextTarget(RoutNum[0]);
                    transform.LookAt(_targetPos);
                    _motion = Motion.Walk;
                }
                else
                {
                    _motion = Motion.Idle;
                }
            }
        }
    }

    private Vector3 FindNextTarget(int i)
    {
        GameObject tempObj = GameObject.Find("Ground[" + i + "]");
        Vector3 answer = new Vector3(tempObj.transform.position.x , this.gameObject.transform.position.y , tempObj.transform.position.z);
        return answer;
    }
}
