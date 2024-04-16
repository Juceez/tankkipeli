using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AI_Controls : MonoBehaviour
{
    private Rigidbody rb;
    public float movementSpeed;
    public float turningSpeed;
    public float turretTurnSpeed;
    public float detectRange;
    public float stopRange;
    public float switchTargetDist;
    
    public Transform turret;
    public Transform muzzle;
    public GameObject projectile;
    
    public float shootCooldown = 1f;
    private float t;

    private GameObject targetObj;
    private Vector3 target;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    void FixedUpdate()
    {
        // Prevents flipping over
        Vector3 currentRotation = rb.rotation.eulerAngles;
        rb.rotation = Quaternion.Euler(0f, currentRotation.y, 0f);
        
        // Search player
        if (targetObj != null)
        {
            target = targetObj.transform.position;
        }
        else
        {
            targetObj = GameObject.FindGameObjectWithTag("Player");
        }

        float angle = Vector3.SignedAngle(transform.forward, target - transform.position, Vector3.up);


        if (angle < 0)
        {
            Turning(-1f);
        }
        else if (angle > 0)
        {
            Turning(1f);
        }

        if (Mathf.Abs(angle) < 90)
        {
            Move(1f);
        }
    }

    private void Move(float input)
    {
        Vector3 movement = transform.forward * input * movementSpeed;
        rb.velocity = movement;
    }

    private void Turning(float input)
    {
            Vector3 turning = Vector3.up * input * turningSpeed;
            rb.angularVelocity = turning;
    }
}
