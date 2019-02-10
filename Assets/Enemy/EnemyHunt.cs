using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHunt : MonoBehaviour {

    private EnemyController manager;

    private bool IsSearching = false; //追いかけているか
    private GameObject _player;//プレイヤーの情報
    private Vector3 player_pos; //プレイヤーの位置情報
    private Vector3 myself_pos; //自分の位置情報

    private int GessHuntTrigger = 0;//GessHuntに移行するための変数0が通常探索、１が追跡、２がGessHuntへの移行。その後0に戻る

    private void Start()
    {
        manager = GetComponent<EnemyController>();
    }

    private void Update()
    {
        if (IsSearching)
        {
            player_pos = new Vector3(
                _player.transform.position.x,
                _player.transform.position.y + 1,
                _player.transform.position.z
                );

            if (SearchRay())
            {
                GessHuntTrigger = 1;
                LookToPlayer();
                Debug.Log("LookPlayer");
                manager.NowStatus = EnemyStatus.Hunt;
                manager.debug = true;
            }
            else
            {
                if (GessHuntTrigger == 1)
                {
                    Debug.Log("GessHuntはつ");
                    manager.NowStatus = EnemyStatus.Idling;
                    GessHuntTrigger = 0;
                }
            }
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
    private void LookToPlayer()
    {
        myself_pos = transform.position;
        Vector3 target_pos = new Vector3(player_pos.x, myself_pos.y, player_pos.z);

        transform.LookAt(target_pos);//プレイヤーの方を向く
    }
}
