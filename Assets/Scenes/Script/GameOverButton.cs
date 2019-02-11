using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverButton : MonoBehaviour {

    [SerializeField] int NUM;

    public void Onclick()
    {
        if (NUM == 0)
        {
            SceneManager.LoadScene("StartMenu");
        }
        if (NUM == 1)
        {
            SceneManager.LoadScene("MainGame");
        }
    }

}
