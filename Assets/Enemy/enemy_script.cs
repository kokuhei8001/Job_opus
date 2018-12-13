using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_script : MonoBehaviour {

    private bool hunt = false; //追いかけているか
    private PlayerMove _player;//プレイヤーの情報
    private Vector3 player_pos;       //プレイヤーの位置情報
    private Vector3 myself_pos; //自分の位置情報



    private Rigidbody rb;
    public LayerMask mask;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(hunt == true)
        {
           player_pos = _player.transform.position;
           myself_pos = transform.position;
           player_pos = new Vector3(player_pos.x, myself_pos.y, player_pos.z);
           
           transform.LookAt(player_pos);
           rb.MovePosition(transform.position + transform.forward * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<PlayerMove>();
        if (player != null)
        {
            _player = player;
            hunt = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        var player = other.gameObject.GetComponent<PlayerMove>();
        if (player != null)
        {
            hunt = false;
        }
    }

    //void Ray()
    //{
    //    //Rayの作成
    //    Ray ray = new Ray(transform.position, transform.forward);
    //    Ray mouseray = Camera.main.ScreenPointToRay(Input.mousePosition);

    //    //2.Rayの衝突したコライダーの情報を得る
    //    RaycastHit hit;

    //    //Rayが衝突したかどうか
    //    if (Physics.Raycast(ray, out hit, 10.0f, mask))
    //    {
    //        //衝突したオブジェクトの色を赤に変える
    //        hit.collider.GetComponent<MeshRenderer>().material.color = Color.red;

    //        //衝突したオブジェの距離を測る
    //        Debug.Log(hit.distance);

    //    }

    //    Debug.DrawRay(ray.origin, ray.direction, Color.red, 3.0f);

    //    // マウスがEventSystem上にあるか
    //    //UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    //}
}
