using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    GameObject player;
    public GameObject bullet;

    public float aggroRange; //How close the player has to be before the enemy starts attacking
    public float timeUntilFire; //Time it takes for the enemy to fire once the player enters their range
    public float timeBetweenFire; //Time between successive attacks if the player remains in the enemy's range
    public float bulletSpeed; //How fast the bullets move
    public float bulletDestroyTimer; //After how many seconds should the bullet destroy itself

    Vector2 bulletDirection; //The direction the bullet will move in
    float distanceFrom; //The enemy's disance from the player
    bool firing; //Whether or not the enemy is firing 

    // Start is called before the first frame update
    void Start()
    {
        //player = FindObjectOfType<PlayerMovement>().gameObject;
        if(GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            print("Found player!");
        }
    }

    void FixedUpdate()
    {
        distanceFrom = Vector2.Distance(transform.position, player.transform.position);
        if(distanceFrom < aggroRange && !firing) //If the player is close enough and the enemy isn't yet firing
        {
            firing = true;
            Invoke("Fire", timeUntilFire);

            //start firing
            print("Start Firing!");
        }
        if(distanceFrom >= aggroRange && firing) //If the player is too far and the enemy is firing
        {
            firing = false;

            //stop firing
            print("Stop Firing!");
        }
    }

    //Every X seconds fire a projectile @ the player
    void Fire()
    {
        if(firing)
        {
            Vector2 gunPos = gameObject.transform.position;
            Vector2 playerPos = player.transform.position;

            Vector2 bulletDirection = -(gunPos - playerPos).normalized; //Get the Vector2 towards the player
            GameObject newBullet = Instantiate(bullet, gunPos + bulletDirection, Quaternion.identity, gameObject.transform); //Make a bullet
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
}
