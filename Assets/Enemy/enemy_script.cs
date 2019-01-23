using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_script : MonoBehaviour {

    private bool IsSearching = false; //追いかけているか
    private GameObject _player;//プレイヤーの情報
    private Vector3 player_pos;       //プレイヤーの位置情報
    private Vector3 myself_pos; //自分の位置情報
    
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (IsSearching)
        {
            player_pos = _player.transform.position;
            if(SearchRay())
            {
                RunToPlayer();
            }
        }

        //デバック
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            transform.position = new Vector3(5, 1, 7);
        }
    }

    //範囲内に入った瞬間
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _player = other.gameObject;
            IsSearching = true;
        }
    }
    //範囲から出た瞬間
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            IsSearching = false;
        }
    }
    //エネミーからプレイヤーが見えているかどうか
    private bool SearchRay()
    {
        Vector3 direction = player_pos - transform.position;
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;

        Debug.DrawRay(transform.position, direction, Color.red);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.tag == "Player")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
    //プレイヤーを追跡している状態
    private void RunToPlayer()
    {
       // Debug.Log("RunToPlayer");
        myself_pos = transform.position;
        Vector3 target_pos = new Vector3(player_pos.x, myself_pos.y, player_pos.z);
        //Debug.Log("LookAtまえ！");
        transform.LookAt(target_pos);//プレイヤーの方を向く
        //Debug.Log("LookAtあと！");
        rb.MovePosition(myself_pos + transform.forward * Time.deltaTime);//前方に進む
    }
}
