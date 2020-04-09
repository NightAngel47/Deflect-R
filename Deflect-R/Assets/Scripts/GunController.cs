using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    GameObject player;
    float distanceFrom;

    // Start is called before the first frame update
    void Start()
    {
        //player = FindObjectOfType<PlayerMovement>().gameObject;
        if(GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        distanceFrom = Vector2.Distance(transform.position, player.transform.position);
        if(distanceFrom < 10)
        {
            //start firing
        }
        if(distanceFrom >= 10)
        {
            //stop firing
        }
    }

    void Fire()
    {
        //Every X seconds fire a projectile @ the player
    }
}
