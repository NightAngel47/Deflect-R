using System;
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
    [SerializeField, Tooltip("The radius that the player may dash to objects in")] public GameObject detectionZone;

    // dash variables
    private DashRadius dashRadius;
    public LayerMask undashableLayer;

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

    public GameObject deflectDirectionCircle;
    private bool timeFrozen;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        dashRadius = detectionZone.GetComponent<DashRadius>();

        deflectDirectionCircle.SetActive(false);
        SetTimeFrozen(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(GetTimeFrozen())
        {
            Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(deflectDirectionCircle.transform.position);
            Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
            float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);
            deflectDirectionCircle.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle + 90));
        }
        else
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

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (dashRadius.FindClosestObject() != null)
                {
                    DashToBullet(dashRadius.FindClosestObject());
                }
            }
        }
    }

    private void FixedUpdate()
    {
        // applies x input movement
        _rigidbody2D.velocity = new Vector2(_xInput * speed, _rigidbody2D.velocity.y);
    }

    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
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

        if (CanDash(direction, Mathf.Infinity))
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

        if (rayHit.collider == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void FreezeTime()
    {
        SetTimeFrozen(true);
        Time.timeScale = 0;
    }

    private void SetTimeFrozen(bool freezeTime)
    {
        timeFrozen = freezeTime;

        if(timeFrozen)
        {
            deflectDirectionCircle.SetActive(true);
        }
        else if(!timeFrozen)
        {
            deflectDirectionCircle.SetActive(false);
        }
    }

    public bool GetTimeFrozen()
    {
        return timeFrozen;
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
