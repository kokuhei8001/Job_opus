using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    private GameObject Player;
    private Quaternion Direction;

	void Start () {
        Player = GameObject.Find("Player");
        Direction = transform.rotation;
	}
	
	void Update () {

        Vector3 Pos = new Vector3(
            Player.transform.position.x,
            Player.transform.position.y + 1,
            Player.transform.position.z - 1
            );

        transform.position = Pos;
        transform.rotation = Direction;
	}
}
