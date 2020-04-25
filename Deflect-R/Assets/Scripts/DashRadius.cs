using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashRadius : MonoBehaviour
{
    private GameObject player;
    private List<Transform> bulletList = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerBehaviour>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(FindClosestObject() != null)
        {
            //Debug.Log("Closest Object " + FindClosestObject().gameObject.name);
        }
    }

    /// <summary>
    /// Finds the closest object to the player inside the Time Slash radius. 
    /// </summary>
    /// <returns></returns>
    public Transform FindClosestObject()
    {
        Transform closestObject = null;
        float closestDistance = Mathf.Infinity;

        //Compares the vector magnitude between the player and each bullet within bulletList
        foreach(Transform bulletTransform in bulletList)
        {
            Vector3 targetDist = player.transform.position - bulletTransform.position;
            float targetMagnitude = targetDist.sqrMagnitude;

            //If the vector's magnitude is less than the current shortest vector magnitude, update closestDistance and closestObject
            if (targetMagnitude < closestDistance)
            {
                //Disables all closest marker objects in list
                for(int i = 0; i < bulletList.Count; i++)
                {
                    bulletList[i].Find("ClosestMarker").gameObject.SetActive(false);
                }

                //Set new closest distance to compare projectiles to and new closest object
                closestDistance = targetMagnitude;
                closestObject = bulletTransform;

                //Activate the closest marker on the closest object
                closestObject.Find("ClosestMarker").gameObject.SetActive(true);
            }
        }

        return closestObject;
    }

    /// <summary>
    /// Adds a projectile to the bullet list if it has not yet been added
    /// </summary>
    /// <param name="bulletTransform"></param>
    public void AddBullet(Transform bulletTransform)
    {
        if (!bulletList.Contains(bulletTransform))
        {
            bulletList.Add(bulletTransform);
        }
    }

    /// <summary>
    /// Removes a projectile from the bullet list if the projectile is in the bullet list
    /// </summary>
    /// <param name="bulletTransform"></param>
    public void RemoveBullet(Transform bulletTransform)
    {
        if(bulletList.Contains(bulletTransform))
        {
            bulletTransform.Find("ClosestMarker").gameObject.SetActive(false);
            bulletList.Remove(bulletTransform);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Adds projectiles to list when they enter the designated radius
        if (collision.CompareTag("Projectile"))
        {
            AddBullet(collision.gameObject.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Removes projectiles from list when they leave the designated radius
        if (collision.CompareTag("Projectile"))
        {
            RemoveBullet(collision.gameObject.transform);
        }
    }
}
