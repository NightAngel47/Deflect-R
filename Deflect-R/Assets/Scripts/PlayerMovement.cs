﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // tweakable variables
    [SerializeField, Tooltip("The speed the player moves at"), Range(1, 50)] private float speed = 1;
    [SerializeField, Tooltip("The force that the player jumps with"), Range(1, 50)] private float jumpForce = 1;
    [SerializeField, Tooltip("The gravity scale the player has when jumping"), Range(-20, 20)] private float jumpGravity = 0.5f;
    [SerializeField, Tooltip("The gravity scale the player has when falling/grounded"), Range(-20, 20)] private float normalGravity = 1f;

    // component references
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    
    // movement variables
    private float _xInput;
    private bool _canJump = true;
    
    // animation variables
    private static readonly int Moving = Animator.StringToHash("Moving");
    private static readonly int InAir = Animator.StringToHash("InAir");

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // store x input for movement
        _xInput = Input.GetAxis("Horizontal");

        // handle sprite flip and animation
        if (Input.GetButton("Horizontal"))
        {
            _spriteRenderer.flipX = !(_xInput > 0);
        }
        _animator.SetBool(Moving, Input.GetButton("Horizontal"));

         // handles player press jump 
        if (_canJump && (Input.GetButtonDown("Jump") || (Input.GetButtonDown("Vertical") && Input.GetAxis("Vertical") > 0)))
        {
            _canJump = false;
            _rigidbody2D.gravityScale = jumpGravity;
            _rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            
            _animator.SetBool(InAir, true);
        }

        // handles player release jump
        if (!_canJump && _rigidbody2D.gravityScale != normalGravity && (Input.GetButtonUp("Jump") || (Input.GetButtonUp("Vertical") && Input.GetAxis("Vertical") > 0)))
        {
            _rigidbody2D.gravityScale = normalGravity;
        }
    }

    private void FixedUpdate()
    {
        // applies x input movement
        _rigidbody2D.velocity = new Vector2(_xInput * speed, _rigidbody2D.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // resets jump and animation
        if (!_canJump)
        {
            _canJump = true;
            _rigidbody2D.gravityScale = normalGravity;
            _animator.SetBool(InAir, false);
        }
    }
}
