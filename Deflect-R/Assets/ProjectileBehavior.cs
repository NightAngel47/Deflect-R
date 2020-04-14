using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float deflectMultiplier = 1f;

    private Rigidbody2D rb;

    /// <summary>
    /// Moves the projectile in the opposite direction from which the player teleports
    /// </summary>
    /// <param name="deflectForce"></param>
    public void DefleftProjectile(Vector2 deflectForce)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = deflectForce * deflectMultiplier;
        Debug.Log("wow");
    }
}
