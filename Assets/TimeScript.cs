using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TimeScript : MonoBehaviour {

    private GameManager manager;
    private Text _text;

    //時間を記録
    string _s, _m;

    private void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _text = GetComponent<Text>();
    }

    private void Update()
    {

        if (manager.Score.y < 10)
        {
            _s = "0" + manager.Score.y;
        }
        else { _s = "" + manager.Score.y; }
        if (manager.Score.x < 10)
        {
            _m = "0" + manager.Score.x;
        }
        else { _m = "" + manager.Score.x; }

        //時間を表示
        _text.text = "Time  " + _m + ":" + _s;
    }

}
