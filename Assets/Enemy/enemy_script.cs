using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_script : MonoBehaviour {

    private Rigidbody rb;

    public LayerMask mask;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerStay(Collider other)
    {
        var player = other.gameObject.GetComponent<PlayerMove>();

        if (player != null)
        {
            Vector3 pos = player.transform.position;
            Vector3 myselfpos = this.transform.position;
            pos = new Vector3(pos.x, myselfpos.y , pos.z);

            transform.LookAt(pos);
            rb.MovePosition(transform.position + transform.forward * Time.deltaTime);
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
