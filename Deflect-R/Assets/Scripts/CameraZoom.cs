using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    public static CameraZoom instance;

    public Vector2 sizeRange;

    public CinemachineVirtualCamera cam;

    public GameObject player;
    private Rigidbody2D playerRigid;

    [Range(0,1)]
    public float expandSpeed;

    private bool expanding;

    private void Awake()
    {
        instance = this;
        expanding = false;
        playerRigid = player.GetComponent<Rigidbody2D>();
    }

    public void Expand()
    {
        expanding = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

        if (expanding)
        {
            if (cam.m_Lens.OrthographicSize < sizeRange.y)
            {
                cam.m_Lens.OrthographicSize = Mathf.Lerp(cam.m_Lens.OrthographicSize, sizeRange.y, expandSpeed);
            }
        } else
        {
            if (cam.m_Lens.OrthographicSize > sizeRange.x)
            {
                cam.m_Lens.OrthographicSize = Mathf.Lerp(cam.m_Lens.OrthographicSize, sizeRange.x, expandSpeed);
            }
        }
        

        if (Time.timeScale != 0)
        {
            if (playerRigid.velocity != Vector2.zero)
            {
                expanding = true;
            } else
            {
                expanding = false;
            }
        } else
        {
            expanding = true;
        }
    }
}
