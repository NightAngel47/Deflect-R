using System;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float projectileLaunchForce = 1f;
    public float deflectMultiplier = 1f;
    public float lifeTime = 5f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime);
    }

    public void FireProjectile(Vector2 launchDir)
    {
        rb.AddForce(launchDir * projectileLaunchForce, ForceMode2D.Impulse);
    }

    /// <summary>
    /// Moves the projectile in the opposite direction from which the player teleports
    /// </summary>
    /// <param name="deflectForce"></param>
    public void DefleftProjectile(Vector2 deflectForce)
    {
        rb.velocity = deflectForce * deflectMultiplier;
        Debug.Log("wow");
    }
}
