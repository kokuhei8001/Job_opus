using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    [SerializeField]
    GameObject Attack;
    [SerializeField]
    private float speed = 20.0f;

    Rigidbody body;

    void Start()
    {
        if (Attack != null)
        {
            Attack.SetActive(false);
        }
    }
    void FixedUpdate()
    {
        body = GetComponent<Rigidbody>();
        //float x = Input.GetAxis("Horizontal");
        //float y = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            body.MovePosition(transform.position + (transform.forward * Time.deltaTime) * speed);
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            body.MovePosition(transform.position + (transform.forward * -Time.deltaTime) * speed);
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            body.MovePosition(transform.position + (transform.right * Time.deltaTime) * speed);
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            body.MovePosition(transform.position + (transform.right * -Time.deltaTime) * speed);
        }

        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(new Vector3(0,5,0));
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(new Vector3(0, -5, 0));
        }


        if (Input.GetKey(KeyCode.LeftShift))
        {
            //しゃがみ処理
        }

        if (Attack != null)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //攻撃
                Attack.SetActive(true);
            }
            else { Attack.SetActive(false); }
        }
    }
}
