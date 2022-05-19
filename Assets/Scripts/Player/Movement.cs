using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameControllerNameSpace;

public class Movement : MonoBehaviour
{
    [Header("Player Controls")]
    [SerializeField] float xSpeed;
    [SerializeField] float moveLimitX;

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
        Move();
    }

    void Move()
    {
        if (GameController.gameState == GameController.GameState.Started)
        {
            // Fruit move
            xInput = Input.GetAxis("Horizontal") * xSpeed;
            _rigidbody.velocity = new Vector3(xInput, _rigidbody.velocity.y, _rigidbody.velocity.z);

            // Player move limit
            xPos = Mathf.Clamp(_transform.position.x, -moveLimitX, moveLimitX);
            _transform.position = new Vector3(xPos, _transform.position.y, _transform.position.z);
        }
    }



}