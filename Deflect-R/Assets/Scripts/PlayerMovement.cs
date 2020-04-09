using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    [SerializeField, Tooltip("The speed the player moves at"), Range(1, 50)] private float speed = 1;
    [SerializeField, Tooltip("The force that the player jumps with"), Range(1, 50)] private float jumpForce = 1;
    [SerializeField, Tooltip("The gravity scale the player has when jumping"), Range(-20, 20)] private float jumpGravity = 0.5f;
    [SerializeField, Tooltip("The gravity scale the player has when falling/grounded"), Range(-20, 20)] private float normalGravity = 1f;

    private float _xInput;
    private bool _canJump = true;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _xInput = Input.GetAxis("Horizontal");

        if (_canJump && (Input.GetButtonDown("Jump") || (Input.GetButtonDown("Vertical") && Input.GetAxis("Vertical") > 0)))
        {
            _canJump = false;
            _rigidbody2D.gravityScale = jumpGravity;
            _rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (!_canJump && _rigidbody2D.gravityScale != normalGravity && (Input.GetButtonUp("Jump") || (Input.GetButtonUp("Vertical") && Input.GetAxis("Vertical") > 0)))
        {
            _rigidbody2D.gravityScale = normalGravity;
        }
    }

    private void FixedUpdate()
    {
        _rigidbody2D.velocity = new Vector2(_xInput * speed, _rigidbody2D.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!_canJump)
        {
            _canJump = true;
            _rigidbody2D.gravityScale = normalGravity;
        }
    }
}
