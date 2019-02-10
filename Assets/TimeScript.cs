using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TimeScript : MonoBehaviour {

    private Text _text;

    //時間を記録
    private float second;
    private int minutes;
    private string _s, _m;

    private void Start()
    {
        _text = GetComponent<Text>();
    }

    private void Update()
    {
        second += Time.deltaTime;

        if (second > 60.0f)
        {
            minutes++;
            second = 0;
        }

        if (second < 10)
        {
            _s = "0" + (int)second;
        }
        else { _s = "" + (int)second; }
        if (minutes < 10)
        {
            _m = "0" + (int)minutes;
        }
        else { _m = "" + (int)minutes; }

        //時間を表示
        _text.text = "Time  " + _m + ":" + _s;
    }

}
