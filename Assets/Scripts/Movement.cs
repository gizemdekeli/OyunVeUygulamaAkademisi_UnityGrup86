using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed;
    public float xSpeed;
    private float xInput;
    Rigidbody _rigidbody;
    private void Start() 
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        xInput = Input.GetAxis("Horizontal");
        if ( xInput != 0)
        {
            if(transform.position.x > 4.5f)
            {
                transform.position = new Vector3(4.5f, transform.position.y, transform.position.z);
            }
            else if (transform.position.x < -4.5f)
            {
                transform.position = new Vector3(-4.5f, transform.position.y, transform.position.z);
            }
            else
            {
                transform.Translate(new Vector3(xInput*xSpeed*Time.deltaTime, 0, 0));
            }
        }
    }
    private void FixedUpdate()
    {
        _rigidbody.AddForce(new Vector3(0, 0, Time.deltaTime*moveSpeed*100));
    }
}
