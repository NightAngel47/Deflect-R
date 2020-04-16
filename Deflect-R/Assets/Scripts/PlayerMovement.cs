﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // tweakable variables
    [SerializeField, Tooltip("The speed the player moves at"), Range(1, 500)] private float speed = 1;
    [SerializeField, Tooltip("The force that the player jumps with"), Range(1, 500)] private float jumpForce = 1;
    [SerializeField, Tooltip("The gravity scale the player has when jumping"), Range(-20, 20)] private float jumpGravity = 0.5f;
    [SerializeField, Tooltip("The gravity scale the player has when falling/grounded"), Range(-20, 20)] private float normalGravity = 1f;
    [SerializeField, Tooltip("The radius that the player may dash to objects in")] public GameObject detectionZone;
    [SerializeField, Tooltip("The max time the player may be frozen in time for before time unfreezes")] public float timeFreezeMaxDuration = 2f;
    [SerializeField, Tooltip("The time that a player must wait after dashing before dashing again.")] public float dashCooldown;

    // dash variables
    private DashRadius dashRadius;
    public LayerMask undashableLayer;
    private bool dashCooled;
    
    // deflect variables
    [SerializeField, Tooltip("The force that is applied to the player on deflect")] private Vector2 deflectForce = new Vector2(15f, 30f);

    // component references
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;
    
    // movement variables
    private float _xInput;
    private bool _canJump = true;
    [SerializeField, Tooltip("The drag on the player walking")] private float normalDrag = 5;
    [SerializeField, Tooltip("The drag on the player in the air")] private float airDrag = 0;
    
    // animation variables
    [SerializeField, Tooltip("The x offset for the collider, used for when the sprite flips")] private float xOffsetForCollider = 0.4f;
    private static readonly int Moving = Animator.StringToHash("Moving");
    private static readonly int InAir = Animator.StringToHash("InAir");

    // time freeze variables
    public GameObject deflectDirectionCircle;
    private bool timeFrozen;
    private IEnumerator coroutine;
    private bool freezeTimeCoroutineStopped;

    private Transform closestBullet;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();

        dashRadius = detectionZone.GetComponent<DashRadius>();

        deflectDirectionCircle.SetActive(false);
        SetTimeFrozen(false);

        coroutine = FreezeTimeDuration();
        freezeTimeCoroutineStopped = false;

        dashCooled = true;
    }

    private void Start()
    {
        _rigidbody2D.gravityScale = normalGravity;
        _rigidbody2D.drag = normalDrag;
    }

    // Update is called once per frame
    void Update()
    {
        if (dashCooled)
        {
            transform.rotation = Quaternion.identity;
            _spriteRenderer.flipY = false;
            _animator.SetBool("Slashing", false);
        }
        else
        {
            _animator.SetBool("Slashing", true);
        }

        // if time is frozen, rotate the deflect direction circle with mouse movement
        // deflection circle points towards the mouse position from the center of the screen
        if (GetTimeFrozen())
        {
            // determines the angle between the center of the screen and the mouse position
            Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(deflectDirectionCircle.transform.position);
            Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
            float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);

            // rotate the deflection circle towards the mouse position
            deflectDirectionCircle.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle + 90));

            if (Input.GetButtonDown("Fire1"))
            {
                if (!freezeTimeCoroutineStopped)
                {
                    freezeTimeCoroutineStopped = true;
                    StopCoroutine(coroutine);
                    UnfreezeTime();
                    DeflectPlayer();
                }
            }
        }
        else
        {
            // store x input for movement
            _xInput = Input.GetAxis("Horizontal");

            // handle sprite flip and animation
            if (_rigidbody2D.velocity.x != 0 && dashCooled)
            _spriteRenderer.flipX = !(_rigidbody2D.velocity.x > 0);

            var colliderOffset = _collider.offset;
            colliderOffset.x = _xInput > 0 ? xOffsetForCollider : -xOffsetForCollider;
            _collider.offset = colliderOffset;
            
            _animator.SetBool(Moving, Input.GetButton("Horizontal"));

            // handles player press jump 
            if (_canJump && (Input.GetButtonDown("Jump") || (Input.GetButtonDown("Vertical") && Input.GetAxis("Vertical") > 0)))
            {
                AudioManager.instance.PlaySound("Jump");
                _canJump = false;
                _rigidbody2D.gravityScale = jumpGravity;
                _rigidbody2D.drag = airDrag;
                _rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

                _animator.SetBool(InAir, true);
            }

            // handles player release jump
            if (!_canJump && _rigidbody2D.gravityScale != normalGravity && (Input.GetButtonUp("Jump") || (Input.GetButtonUp("Vertical") && Input.GetAxis("Vertical") > 0)))
            {
                _rigidbody2D.gravityScale = normalGravity;
                _rigidbody2D.drag = normalDrag;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (dashRadius.FindClosestObject() != null)
                {
                    closestBullet = dashRadius.FindClosestObject();

                    // dashes player to the closest projectile
                    DashToBullet(closestBullet);
                }
            }
        }
        
        print(_rigidbody2D.velocity);
    }

    private void FixedUpdate()
    {
        // applies x input movement
        if (dashCooled)
        _rigidbody2D.velocity = new Vector3 (_xInput * speed, _rigidbody2D.velocity.y, 0);

        if (_canJump && _rigidbody2D.velocity.x != 0)
            AudioManager.instance.PlaySound("Footstep");
        else
            AudioManager.instance.StopSound("Footstep");
    }

    /// <summary>
    /// Finds the angle between two points in degrees
    /// </summary>
    /// <param name="objectPosOnScreen"></param>
    /// <param name="mousePos"></param>
    /// <returns></returns>
    private float AngleBetweenTwoPoints(Vector3 objectPosOnScreen, Vector3 mousePos)
    {
        return Mathf.Atan2(objectPosOnScreen.y - mousePos.y, objectPosOnScreen.x - mousePos.x) * Mathf.Rad2Deg;
    }

    /// <summary>
    /// If the player can dash to a projectile, moves the player to that projectile
    /// </summary>
    /// <param name="projectileTransform"></param>
    private void DashToBullet(Transform projectileTransform)
    {
        Vector3 direction = transform.position - projectileTransform.position;
        float dashMagnitude = direction.sqrMagnitude;

        //Debug.DrawRay(transform.position, -direction, Color.green, dashMagnitude, false);

        if (CanDash(direction, dashMagnitude))
        {
            transform.position = projectileTransform.position;
            FreezeTime();
        }
    }

    /// <summary>
    /// Determines if there is an object inbetween the player and the target projectile
    /// that the player is unable to dash through. 
    /// </summary>
    /// <param name="dashDirection"></param>
    /// <param name="dashDistance"></param>
    /// <returns></returns>
    private bool CanDash(Vector3 dashDirection, float dashDistance)
    {
        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, -dashDirection, dashDistance, undashableLayer);

        if (rayHit.collider == null && dashCooled)
        {
            dashCooled = false;
            Invoke("CoolDash", dashCooldown);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void CoolDash()
    {
        dashCooled = true;
    }

    /// <summary>
    /// Initiates freezing time
    /// </summary>
    private void FreezeTime()
    {
        //animates the player drawing the sword
        _animator.SetBool("Drawing", true);
        AudioManager.instance.PlaySound("Draw");
        AudioManager.instance.PlaySound("Heartbeat");

        _canJump = false;
        deflectDirectionCircle.SetActive(true);
        SetTimeFrozen(true);
        coroutine = FreezeTimeDuration();
        _rigidbody2D.velocity = new Vector2(0,0);
        StartCoroutine(coroutine);
    }

    /// <summary>
    /// Initiates unfreezing time
    /// </summary>
    private void UnfreezeTime()
    {
        _animator.SetBool("Drawing", false);

        Time.timeScale = 1;
        freezeTimeCoroutineStopped = false;
        deflectDirectionCircle.SetActive(false);
        SetTimeFrozen(false);
        DeflectPlayer();
    }

    /// <summary>
    /// Unfreezes time after the time freeze's max duration is reached
    /// </summary>
    /// <returns></returns>
    private IEnumerator FreezeTimeDuration()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(timeFreezeMaxDuration);
        UnfreezeTime();
    }

    /// <summary>
    /// Setter function for time freeze
    /// </summary>
    /// <param name="freezeTime"></param>
    private void SetTimeFrozen(bool freezeTime)
    {
        timeFrozen = freezeTime;
    }

    /// <summary>
    /// Getter function for time freeze
    /// </summary>
    /// <returns></returns>
    public bool GetTimeFrozen()
    {
        return timeFrozen;
    }

    /// <summary>
    /// Determines the direction to deflect the player in and deflects the player
    /// </summary>
    private void DeflectPlayer()
    {
        AudioManager.instance.StopSound("Heartbeat");
        AudioManager.instance.StopSound("Draw");

        //Deflect player in the direction of the mouse
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 difference = mousePos - gameObject.transform.position;
        difference.z = 0;
        difference.Normalize();

        //Sets gravity to jump gravity
        _rigidbody2D.gravityScale = jumpGravity;
        _rigidbody2D.drag = airDrag;

        //Adds the deflection force to the player
        _rigidbody2D.AddForce(new Vector2(difference.x, difference.y) * deflectForce, ForceMode2D.Impulse);

        _spriteRenderer.flipY = _rigidbody2D.velocity.x < 0;
        _spriteRenderer.flipX = false;
        //_spriteRenderer.flipX = _rigidbody2D.velocity.x < 0;
;
        transform.rotation = Quaternion.AngleAxis((Mathf.Atan2(_rigidbody2D.velocity.y, _rigidbody2D.velocity.x) * Mathf.Rad2Deg), Vector3.forward);

        AudioManager.instance.PlaySound("Slash");

        //Deflect bullet in opposite direction from the player's deflection
        closestBullet.gameObject.GetComponent<ProjectileBehavior>().DefleftProjectile(-(new Vector2(difference.x, difference.y) * deflectForce));
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // resets jump and animation
        if (!_canJump)
        {
            AudioManager.instance.PlaySound("Land");
            _canJump = true;
            _rigidbody2D.drag = normalDrag;
            _rigidbody2D.gravityScale = normalGravity;
            _animator.SetBool(InAir, false);
        }
    }
}
