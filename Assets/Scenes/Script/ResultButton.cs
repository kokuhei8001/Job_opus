using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultButton : MonoBehaviour {

    [SerializeField] int type; // 0がタイトル、1がRETRY

    public void _OnClick()
    {
        switch (type)
        {
            case 0:
                SceneManager.LoadScene("StartMenu");
                break;
            case 1:
                SceneManager.LoadScene("MainGame");
                break;
        }
    }
}
