using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public float switchTargetRange;
    public float switchDist;

    public float AIDelay;
    
    
    public Transform turret;
    public Transform muzzle;
    public GameObject projectile;

    public string stringState;
    
    public float shootCooldown = 1f;
    private float t;
    private float AIt;

    private GameObject targetObj;
    private Vector3 target;

    private int obstacleMask;
    private enum State { forward, left, right, back, stop};

    private State state;
    private State nextState;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        t = 0f;
        AIt = 0f;
        obstacleMask = LayerMask.GetMask("Obstacle");
        state = State.forward;
        nextState = State.forward;
    }
    
    void FixedUpdate()
    {
        t -= Time.deltaTime;
        // Prevents flipping over
        Vector3 currentRotation = rb.rotation.eulerAngles;
        rb.rotation = Quaternion.Euler(0f, currentRotation.y, 0f);

        if (Vector3.Distance(transform.position, target) < switchTargetRange)
        {
            float randomx = UnityEngine.Random.Range(-switchDist, switchDist);
            float randomz = UnityEngine.Random.Range(-switchDist, switchDist);


            target += new Vector3(randomx, 0f, randomz);
        }
        
        
        // Search player
        if (targetObj != null)
        {
            if (Vector3.Distance(transform.position, targetObj.transform.position) < detectRange)
            {
                if (!Physics.Linecast(transform.position, targetObj.transform.position, obstacleMask))
                {
                    target = targetObj.transform.position;
                    
                    // AI SHOOTS
                    if (t < 0)
                    {
                        Instantiate(projectile, muzzle.position, muzzle.rotation);
                        t = shootCooldown;
                    }
                    
                    if (Vector3.Distance(target, transform.position) < stopRange)
                    {
                        nextState = State.stop;
                    }
                }
            }
        }
        else
        {
            targetObj = GameObject.FindGameObjectWithTag("Player");
        }
        
        Debug.DrawLine(target, target+ new Vector3(0f, 5f, 0f), Color.green);
        
        
        float angle = Vector3.SignedAngle(transform.forward, target - transform.position, Vector3.up);



        if (AIt < 0)
        {
            state = nextState;
            AIt = AIDelay;
        }
        else
        {
            AIt -= Time.deltaTime;
        }
        
        // AI State machine
        if (state == State.forward)
        {
            stringState = "forward";
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
        else if (state == State.left)
        {
            stringState = "left";   
            Turning(-1f);
            Move(1f);
        }
        else if (state == State.right)
        {
            stringState = "right";
            Turning(1f);
            Move(1f);
        }
        else if (state == State.back)
        {
            stringState = "back";
            Move(-1f);
            nextState = State.forward;
        }
        else if (state == State.stop)
        {
            stringState = "stop";
            Move(0f);
            nextState = State.forward;
        }
        
        Vector3 targetDir = target - turret.position;
        targetDir.y = 0f;
        Vector3 turningDir = Vector3.RotateTowards(turret.forward, targetDir, turretTurnSpeed * Time.deltaTime, 0f);
        turret.rotation = Quaternion.LookRotation(turningDir);
        
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

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Obstacle") && !other.gameObject.CompareTag("Wall"))
        {
            return;
        }

        RaycastHit leftHit;
        RaycastHit rightHit;

        float leftLenght = 0f;
        float rightLenght = 0f;

        if (Physics.Raycast(transform.position, transform.forward + transform.right * -1, out leftHit, Mathf.Infinity,
                obstacleMask))
        {
            leftLenght = leftHit.distance;
        }
        
        if (Physics.Raycast(transform.position, transform.forward + transform.right, out rightHit, Mathf.Infinity,
                obstacleMask))
        {
            rightLenght= rightHit.distance;
        }

        if (leftLenght > rightLenght)
        {
            state = State.left;
            target = leftHit.point;
        }
        else
        {
            state = State.right;
            target = rightHit.point;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        nextState = State.forward;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Obstacle") && !collision.gameObject.CompareTag("Wall"))
        {
            return;
        }

        state = State.back;
    }
}
