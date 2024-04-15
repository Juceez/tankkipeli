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

    public Transform turret;
    
    private Camera mainCamera;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        floorMask = LayerMask.GetMask("Floor");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
