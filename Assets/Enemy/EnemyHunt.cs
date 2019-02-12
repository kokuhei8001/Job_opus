using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHunt : MonoBehaviour {

    //必要なコンポーネント
    private GameManager gamemanager;
    private EnemyController manager;

    private bool IsSearching = false; //追いかけているか
    private GameObject _player;//プレイヤーの情報
    private Vector3 player_pos; //プレイヤーの位置情報
    private Vector3 myself_pos; //自分の位置情報

    private List<Vector2Int> gessRout = new List<Vector2Int>();
    private Vector3 TargetPos; //次のブロックのPosition

    private int GessHuntTrigger = 0;//GessHuntに移行するための変数0が通常探索、１が追跡、２がGessHuntへの移行。その後0に戻る

    private void Start()
    {
        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
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
                //プレイヤーが見えているなら
                GessHuntTrigger = 1;
                LookToPlayer();
                manager.NowStatus = EnemyStatus.Hunt;
            }
            else
            {
                if (GessHuntTrigger == 1)
                {
                    //もし直前まで追いかけていたなら最後に見失った場所まで走る
                    manager.NowStatus = EnemyStatus.GessHunt;
                    Vector2Int PlayerPos = gamemanager.GetPosData(_player);
                    gessRout = gamemanager.ASter(this.gameObject, PlayerPos);
                    if (gessRout.Count != 0) { TargetPos = FindNextTarget(gessRout[gessRout.Count - 1].x, gessRout[gessRout.Count - 1].y); }
                    transform.LookAt(TargetPos);

                    GessHuntTrigger = 0;//トリガーを元に戻す
                }
            }
        }

        if (manager.NowStatus == EnemyStatus.GessHunt)
        {
            //マスに付いたら次のマスへ進む
            if (TargetPos.x - 0.2f < transform.position.x && transform.position.x < TargetPos.x + 0.2f)
            {
                if (TargetPos.z - 0.2f < transform.position.z && transform.position.z < TargetPos.z + 0.2f)
                {
                    if (gessRout.Count != 0)
                    {
                        gessRout.RemoveAt(gessRout.Count - 1);
                        if (gessRout.Count != 0)
                        {
                            TargetPos = manager.FindNextTarget(gessRout[gessRout.Count - 1].x, gessRout[gessRout.Count - 1].y);
                            transform.LookAt(TargetPos);
                        }
                        else
                        {
                            manager.NowStatus = EnemyStatus.Idling;
                        }
                    }
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

    private Vector3 FindNextTarget(int i, int k)
    {
        GameObject tempObj = GameObject.Find("Ground[" + k + "," + i + "]");
        if (tempObj != null)
        {
            Vector3 answer = new Vector3(tempObj.transform.position.x, this.gameObject.transform.position.y, tempObj.transform.position.z);
            return answer;
        }
        else
        {
            Debug.Log("エネミールートにエラー");
            return new Vector3(0, 100, 0);
        }
    }
}
