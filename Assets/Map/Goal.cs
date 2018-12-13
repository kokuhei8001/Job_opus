using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {


    private void Update()
    {
        //分かりやすいように回転させる
        transform.Rotate(new Vector3(0, -1.0f, 0),Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        var _player = other.GetComponent<PlayerMove>();

        if (_player != null)
        {
            Debug.Log("GameClear!!");
        }
    }
}
