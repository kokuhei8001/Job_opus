using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private GameManager _gameManager;

    [SerializeField]
    private float speed = 20.0f;

    Rigidbody body;

    private void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

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

        //デバッグ用リスポーンキー
        if (Input.GetKey(KeyCode.Alpha1))
        {
            transform.position = new Vector3(0, 5, 10);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector2Int aaa = _gameManager.GetPosData(this.gameObject);
            Debug.Log(" PosX : " + aaa.x + " PosY: " + aaa.y);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            List<Vector2Int> ToGorl = new List<Vector2Int>();
            ToGorl = _gameManager.ASter(this.gameObject,_gameManager.GorlPos);
            for (int i = 0; i < ToGorl.Count; i++)
            {
                Debug.Log(i + "歩目" + ToGorl[ToGorl.Count - 1 - i]);
            }
        }

    }

    void Moov(string dirction)
    {
        //方向を決める
        Vector3 dir = new Vector3();
        //Transform myTransform = this.transform;
        //Vector3 worldPos = myTransform.position;
        //Vector3 localPos = myTransform.localPosition;

        switch (dirction)
        {
            case "forward":
                dir = transform.forward;
                //localPos.z += 0.5f;
                break;

            case "back":
                dir = -transform.forward;
                break;

            case "right":
                dir = transform.right;
                //localPos.x += 0.5f;
                break;

            case "left":
                dir = -transform.right;
                break;
        }
        if (MoovRay(dir))//Rayを飛ばして先が壁かどうか調べる
        {
            body.MovePosition(transform.position + (dir * Time.deltaTime) * speed);
            //myTransform.localPosition = localPos;
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

