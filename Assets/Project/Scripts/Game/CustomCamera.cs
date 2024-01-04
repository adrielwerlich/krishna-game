using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCamera : MonoBehaviour
{
    //public GameObject target;
    public UnityPlayer player;
    public Vector3 offset;
    public float focusSpeed = 3.2f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            transform.position = Vector3.Lerp(transform.position,
                player.transform.position + offset,
                Time.deltaTime * focusSpeed
            );

            //if (player.JustTeleported)
            //{
            //    transform.position = player.transform.position + offset;
            //}
        }
        
    }
}
