using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        var wall = other.gameObject.tag;

        if (wall == "Wall")
        {
            var mesh = other.GetComponent<MeshRenderer>();
            mesh.enabled = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        var wall = other.gameObject.tag;

        if (wall == "Wall")
        {
            var mesh = other.GetComponent<MeshRenderer>();
            mesh.enabled = true;
        }
    }
}
