using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

enum ButoonType
{
    NewGame,
    StartGame,
    Ranking,
    EndGame,
    BackToMenu
}
public class StartMenuButoon : MonoBehaviour {

    [SerializeField] private ButoonType Type;

    [SerializeField] private GameObject StartMenu;
    [SerializeField] private GameObject OpeMenu;

    private void Update()
    {

    }

    public void _OnClick()
    {
        switch (Type)
        {
            case ButoonType.NewGame:
                StartMenu.SetActive(false);
                OpeMenu.SetActive(true);
                break;
            case ButoonType.StartGame:
                SceneManager.LoadScene("MainGame");
                break;
            case ButoonType.EndGame:

                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #elif UNITY_WEBPLAYER
		        Application.OpenURL("http://www.yahoo.co.jp/");
                #else
		        Application.Quit();
                #endif
                break;
            case ButoonType.Ranking:
                SceneManager.LoadScene("ResultMenu");
                break;
            case ButoonType.BackToMenu:
                StartMenu.SetActive(true);
                OpeMenu.SetActive(false);
                break;
        }
    }


}
