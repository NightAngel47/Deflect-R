using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImage : MonoBehaviour
{
    public GameObject player;

    private ParticleSystemRenderer psren;
    private ParticleSystem ps;

    private void Awake()
    {
        psren = GetComponent<ParticleSystemRenderer>();
        ps = GetComponent<ParticleSystem>();
        var main = ps.main;
        main.startLifetime = 1;
    }

    private void Update()
    {
        transform.position = player.transform.position;
        

        if (ps.isEmitting)
        {
            if (player.GetComponent<Rigidbody2D>().velocity.x < 0)
                psren.flip = new Vector3(180, 0, 0);
            else
                psren.flip = new Vector3(0, 0, 0);
        }
    }
}
