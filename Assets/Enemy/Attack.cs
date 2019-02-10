using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Attack : MonoBehaviour {

    [SerializeField] private GameObject Enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("GameOver");
        }

        if (other.gameObject.tag == "Enemy")
        {
            var on = Enemy.GetComponent<SphereCollider>();
            on.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            var on = Enemy.GetComponent<SphereCollider>();
            on.enabled = true;
        }
    }

}
