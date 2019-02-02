using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAutoSurch : MonoBehaviour {

    //自動探索が行われるかどうかを決定する変数
    public bool IsAutoSurch;
    
    //他のスクリプトへのリンク
    private GameManager _gameManager;
    private EnemyController _enemyManager;
    
    //目的地までのルート
    private List<Vector2Int> Rout = new List<Vector2Int>();
    private int Count = 0;


    private void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _enemyManager = GetComponent<EnemyController>();
    }

    private void Update()
    {
        if (IsAutoSurch)
        {
            if (Count == 0) { RoutReset(); }
        }

        //デバッグモード
        if (Input.GetKeyDown(KeyCode.Alpha3))//3を推したらルートがリセットされる
        {
            RoutReset();
        }
    }

    //ランダムな場所を取得してそこまでのルートを記録する。
    private void RoutReset()
    {
        Rout = new List<Vector2Int>();

        Vector2Int End;//目的地の座標
        while (true)
        {
            End = new Vector2Int(Random.Range(3, 47), Random.Range(3, 47));

            if (_gameManager.Map[End.x, End.y].Status != MapStatus.Wall)
            {
                break;
            }
        }
        Debug.Log(_gameManager.GetPosData(this.gameObject));
        Debug.Log(End);

        Rout = _gameManager.ASter(this.gameObject, End);

        for (int i = 0; i < Rout.Count; i++)
        {
            Debug.Log(i + "歩目" + Rout[Rout.Count - 1 - i]);
        }
    }
}
