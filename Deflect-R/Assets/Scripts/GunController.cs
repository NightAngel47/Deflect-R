using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    Transform player;
    public GameObject bullet;
    private Animator anim;

    public float aggroRange; //How close the player has to be before the enemy starts attacking
    //public float timeUntilFire; //Time it takes for the enemy to fire once the player enters their range
    public float timeBetweenFire = 1f; //Time between successive attacks if the player remains in the enemy's range
    public float timeBeforeFire = 1f;
    //public float bulletSpeed; //How fast the bullets move
    //public float bulletDestroyTimer; //After how many seconds should the bullet destroy itself

    //Vector2 bulletDirection; //The direction the bullet will move in
    //float distanceFrom; //The enemy's disance from the player
    bool firing; //Whether or not the enemy is firing 

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        player = FindObjectOfType<PlayerBehaviour>().transform;
        //print("Found player!");
    }

    void Update()
    {
        if (gameObject.tag == "Tracker")
        {
            Vector3 targ = player.transform.position;
            targ.z = 0f;

            Vector3 objectPos = transform.position;
            targ.x = targ.x - objectPos.x;
            targ.y = targ.y - objectPos.y;

            float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;
            anim.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        }
        else if (gameObject.tag == "Forward")
        {
            anim.transform.rotation = Quaternion.Euler(new Vector3(0, 0, + 90));
        }
        else if (gameObject.tag == "Backward")
        {
            anim.transform.rotation = Quaternion.Euler(new Vector3(0, 0, - 90));
        }
        else if (gameObject.tag == "Down Below")
        {
            anim.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else if (gameObject.tag == "From Above")
        {
            anim.transform.rotation = Quaternion.Euler(new Vector3(0, 0, - 180));
        }


        float distanceFrom = Vector2.Distance(transform.position, player.transform.position);
        if(distanceFrom < aggroRange && !firing) //If the player is close enough and the enemy isn't yet firing
        {
            firing = true;
            //Invoke("Fire", timeUntilFire);
            StartCoroutine(FireBullets());
            

            //start firing
            //print("Start Firing!");
        }
        if(distanceFrom >= aggroRange && firing) //If the player is too far and the enemy is firing
        {
            firing = false;

            //stop firing
            //print("Stop Firing!");
        }
    }

    private IEnumerator FireBullets()
    {
        anim.SetBool("Drawing", true);
        yield return new WaitForSeconds(timeBeforeFire);
        SpawnBullet();
        anim.SetBool("Drawing", false);
        yield return new WaitForSeconds(timeBetweenFire);

        if (firing)
        {
            StartCoroutine(FireBullets());
        }
    }

    private void SpawnBullet()
    {
        Vector3 bulletDirection;
        var gunPos = transform.position;
        var playerPos = player.position;
        
        if (gameObject.tag == "Tracker")
        {
            bulletDirection = (gunPos - playerPos).normalized; //Get the Vector2 towards the player
            ProjectileBehavior newBullet = Instantiate(bullet, gunPos - bulletDirection, Quaternion.identity).GetComponent<ProjectileBehavior>();
            newBullet.transform.Rotate(Vector3.forward, Vector3.SignedAngle(gunPos, bulletDirection, Vector3.forward) + 180);
            newBullet.FireProjectile(-bulletDirection);
        }
        else if (gameObject.tag == "Forward")
        {
            bulletDirection = (gunPos - Vector3.back).normalized;
            ProjectileBehavior newBullet = Instantiate(bullet, gunPos - bulletDirection, Quaternion.identity).GetComponent<ProjectileBehavior>();
            newBullet.transform.Rotate(Vector3.forward, Vector3.SignedAngle(gunPos, bulletDirection, Vector3.forward) + 180);
            newBullet.FireProjectile(-bulletDirection);
        }
        else if (gameObject.tag == "Backward")
        {
            bulletDirection = (gunPos - Vector3.forward).normalized;
            ProjectileBehavior newBullet = Instantiate(bullet, gunPos - bulletDirection, Quaternion.identity).GetComponent<ProjectileBehavior>();
            newBullet.transform.Rotate(Vector3.forward, Vector3.SignedAngle(gunPos, bulletDirection, Vector3.forward));
            newBullet.FireProjectile(bulletDirection);
        }
        else if (gameObject.tag == "From Above")
        {
            bulletDirection = (gunPos - Vector3.down).normalized;
            ProjectileBehavior newBullet = Instantiate(bullet, gunPos - bulletDirection, Quaternion.identity).GetComponent<ProjectileBehavior>();
            newBullet.transform.Rotate(Vector3.forward, Vector3.SignedAngle(gunPos, bulletDirection, Vector3.forward) - 90);
            newBullet.FireProjectile(Vector2.down);
        }
        else if (gameObject.tag == "Down Below")
        {
            bulletDirection = (gunPos - Vector3.up).normalized;
            ProjectileBehavior newBullet = Instantiate(bullet, gunPos - bulletDirection, Quaternion.identity).GetComponent<ProjectileBehavior>();
            newBullet.transform.Rotate(Vector3.forward, Vector3.SignedAngle(gunPos, bulletDirection, Vector3.forward) + 90);
            newBullet.FireProjectile(Vector2.up);
        }
        
        
        //print("Bullet spawned!");
    }

    //Every X seconds fire a projectile @ the player
    /*
    void Fire()
    {
        if(firing)
        {
            Vector2 gunPos = gameObject.transform.position;
            Vector2 playerPos = player.transform.position;

            Vector2 bulletDirection = -(gunPos - playerPos).normalized; //Get the Vector2 towards the player
            GameObject newBullet = Instantiate(bullet, gunPos + bulletDirection, Quaternion.identity); //Make a bullet
            bulletDirection *= bulletSpeed * Time.deltaTime;
            newBullet.GetComponent<Rigidbody2D>().velocity = bulletDirection; //Send it towards the player
            Destroy(newBullet, bulletDestroyTimer); //Destroy bullets after X time to prevent infinity bullets
            print("Bullet spawned!");
            Invoke("Fire", timeBetweenFire);
        }
        else
        {
            //target out of range
            print("Target out of range!");
        }
    }
    */
}
