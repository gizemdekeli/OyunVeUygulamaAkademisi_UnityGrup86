using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float rollspeed;
    [SerializeField] float xSpeed;
    [SerializeField] float maxRollSpeed;

    float xInput;
    float xPos;
    Rigidbody _rigidbody;
    Transform _transform;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = transform;
    }
    private void Update()
    {
        // Fruit move
        xInput = Input.GetAxis("Horizontal") * xSpeed;
        rollspeed = Mathf.Clamp(_rigidbody.velocity.z, 0, maxRollSpeed);
        _rigidbody.velocity = new Vector3(xInput, _rigidbody.velocity.y, rollspeed);

        // Move limit
        xPos = Mathf.Clamp(_transform.position.x, -4.5f, 4.5f);
        _transform.position = new Vector3(xPos, _transform.position.y, _transform.position.z);
    }
    private void FixedUpdate()
    {
        // Rolling
        _rigidbody.AddForce(rollspeed * Vector3.forward, ForceMode.Acceleration);
    }
}
