using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Attack : MonoBehaviour {


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("GameOver");
        }

        if (other.gameObject.tag == "Enemy")
        {
            var on = other.gameObject.GetComponent<SphereCollider>();
            on.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            var on = other.gameObject.GetComponent<SphereCollider>();
            on.enabled = true;
        }
    }

}
