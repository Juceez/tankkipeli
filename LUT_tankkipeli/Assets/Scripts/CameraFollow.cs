using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    private GameObject player;
        
    void Update()
    {
        if (player == null) player = GameObject.FindGameObjectWithTag("Player");
        else
        {
            target = player.transform;
            transform.position = target.position + offset;
        }
        
    }
}
