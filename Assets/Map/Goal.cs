using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour {

    private JsonPacker _Savedata;
    private GameManager manager;

    private void Start()
    {
        GameObject Manager = GameObject.Find("GameManager");
        manager = Manager.GetComponent<GameManager>();
        _Savedata = Manager.GetComponent<JsonPacker>();
    }

    private void Update()
    {
        //分かりやすいように回転させる
        transform.Rotate(new Vector3(0, -1.0f, 0),Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _Savedata.SaveToJson(_Savedata._data ,manager.Score);

            SceneManager.LoadScene("ResultMenu");
            Debug.Log("GameClear!!");
        }
    }
}
