using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DefleftProjectile(Vector2 deflectForce)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = deflectForce;
        Debug.Log("wowj");
    }
}
