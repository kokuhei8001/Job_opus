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
}
