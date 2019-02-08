using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour {


    private void Update()
    {
        //分かりやすいように回転させる
        transform.Rotate(new Vector3(0, -1.0f, 0),Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SceneManager.LoadScene("ResultMenu");
            Debug.Log("GameClear!!");
        }
    }
}
