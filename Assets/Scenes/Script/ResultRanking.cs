using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultRanking : MonoBehaviour {

    [SerializeField] private int NUM;

    private JsonPacker json;
    private SaveData _data;
    private Text _text;

    private void Start()
    {
        json = GameObject.Find("GameManager").GetComponent<JsonPacker>();
        _data = json.LoadFromJson();
        for (int i = 0; i < _data.TimeRanking.Count; i++)
        {
            if (NUM == i + 1)
            {
                _text = GetComponent<Text>();
                _text.text = _data.TimeRanking[i].x + " : " + _data.TimeRanking[i].y;
            }
        }

    }


}
