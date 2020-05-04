using System;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float projectileLaunchForce = 1f;
    public float deflectMultiplier = 1f;
    public float lifeTime = 5f;

    private Rigidbody2D rb;
    private TrailRenderer _trailRenderer;
    private DashRadius _dashRadius;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _trailRenderer = GetComponent<TrailRenderer>();
        _trailRenderer.enabled = false;
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out DashRadius dashRadius))
        {
            _dashRadius = dashRadius;
        }
        else if(other.CompareTag("Ground") || other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out DashRadius dashRadius))
        {
            _dashRadius = null;
        }
    }

    public void FireProjectile(Vector2 launchDir)
    {
        rb.AddForce(launchDir * projectileLaunchForce, ForceMode2D.Impulse);
    }

    /// <summary>
    /// Rotates the projectile in the opposite direction from which the player deflects,
    /// and then adds a velocity to the projectile in that direction.
    /// </summary>
    /// <param name="deflectForce"></param>
    public void DefleftProjectile(Vector2 deflectForce)
    {
        float newAngle = Mathf.Atan2(deflectForce.y, deflectForce.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(newAngle, Vector3.forward);
        rb.velocity = deflectForce * deflectMultiplier;
        _trailRenderer.enabled = true;
    }

    private void OnDestroy()
    {
        if (_dashRadius)
        {
            _dashRadius.RemoveBullet(transform);
        }
    }
}
