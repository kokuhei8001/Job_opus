using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private float speed = 20.0f;

    Rigidbody body;
    
    void Update()
    {
        body = GetComponent<Rigidbody>();

        //移動について
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            Moov("forward");
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            Moov("back");
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            Moov("right");
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            Moov("left");
        }

        //回転
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(new Vector3(0, 5, 0));
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(new Vector3(0, -5, 0));
        }


        if (Input.GetKey(KeyCode.LeftShift))
        {
            //しゃがみ処理
        }

        //リスポーンキー
        if (Input.GetKey(KeyCode.Alpha1))
        {
            transform.position = new Vector3(0, 5, 10);
        }
    }

    void Moov(string dirction)
    {
        //方向を決める
        Vector3 dir = new Vector3();
        switch (dirction)
        {
            case "forward":
                dir = transform.forward;
                break;

            case "back":
                dir = -transform.forward;
                break;

            case "right":
                dir = transform.right;
                break;

            case "left":
                dir = -transform.right;
                break;
        }
        if (MoovRay(dir))//Rayを飛ばして先が壁かどうか調べる
        {
            body.MovePosition(transform.position + (dir * Time.deltaTime) * speed);
        }
    }
    
    bool MoovRay(Vector3 dirction)
    {
        Ray ray = new Ray(transform.position, dirction);
        RaycastHit hit;
        float distance = 0.75f; //0.75fが完璧

        //Debug用ライン
        Debug.DrawRay(transform.position, dirction * distance, Color.red);

        //衝突したら
        if (Physics.Raycast(ray, out hit, distance))
        {
            if (hit.collider.tag == "Wall")//壁だったら
            {
                return false;
            }
            else//それ以外だったら
            {
                return true;
            }
        }
        else//衝突しなければ
        {
            return true;
        }
    }
}
