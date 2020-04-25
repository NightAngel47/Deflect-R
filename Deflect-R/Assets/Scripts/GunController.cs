﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    Transform player;
    public GameObject bullet;

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
        player = FindObjectOfType<PlayerBehaviour>().transform;
        //print("Found player!");
    }
    
    void Update()
    {
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
        yield return new WaitForSecondsRealtime(timeBeforeFire);
        SpawnBullet();
        yield return new WaitForSecondsRealtime(timeBetweenFire);

        if (firing)
        {
            StartCoroutine(FireBullets());
        }
    }

    private void SpawnBullet()
    {
        var gunPos = transform.position;
        var playerPos = player.position;
        
        Vector3 bulletDirection = (gunPos - playerPos).normalized; //Get the Vector2 towards the player
        ProjectileBehavior newBullet = Instantiate(bullet, gunPos - bulletDirection, Quaternion.identity).GetComponent<ProjectileBehavior>();
        newBullet.transform.Rotate(Vector3.forward, Vector3.SignedAngle(gunPos, bulletDirection, Vector3.forward) + 180);
        newBullet.FireProjectile(-bulletDirection);
        
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
