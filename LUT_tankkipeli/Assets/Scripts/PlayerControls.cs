using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class LayerControls : MonoBehaviour
{
    private Rigidbody rb;
    public float movementSpeed;
    public float turningSpeed;
    public float turretTurnSpeed;
    private float maxRayDist = 100f;
    private int floorMask;
    public GameObject projectile;
    public Transform muzzle;
    public float shootCooldown = 1f;
    private float t;

    public Transform turret;
    
    private Camera mainCamera;
    public AudioSource AudioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        t = 0f;
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        floorMask = LayerMask.GetMask("Floor");
    }

    void Update()
    {
        if (t <= 0)
        {
            if (Input.GetButton("Fire1"))
            {
                AudioSource.Play();
                Instantiate(projectile, muzzle.position, muzzle.rotation);
                t = shootCooldown;
            }
        }
        else
        {
            t -= Time.deltaTime;
        }
      
    }
    
    void FixedUpdate()
    {
        Vector3 currentRotation = rb.rotation.eulerAngles;
        rb.rotation = Quaternion.Euler(0f, currentRotation.y, 0f);
        
        float inputHor = Input.GetAxis("Horizontal");
        float inputVer = Input.GetAxis("Vertical");

        if (inputHor != 0)
        {
            Vector3 turning = Vector3.up * inputHor * turningSpeed;
            rb.angularVelocity = turning;
        }

        if (inputVer != 0)
        {
            Vector3 movement = transform.forward * inputVer * movementSpeed;
            rb.velocity = movement;
        }


        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxRayDist, floorMask))
        {
            Vector3 targetDir = hit.point - turret.position;
            targetDir.y = 0f;
            Vector3 turningDir = Vector3.RotateTowards(turret.forward, targetDir, turretTurnSpeed * Time.deltaTime, 0f);
            turret.rotation = Quaternion.LookRotation(turningDir);
        }
    }
}
