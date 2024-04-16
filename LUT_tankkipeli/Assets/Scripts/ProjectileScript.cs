using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float speed;
    public float time;
    public float radius;
    public float damage;

    private Rigidbody rb;
    private float t;
    
    // Start is called before the first frame update
    void Start()
    {
        t = time;
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;    
    }

    // Update is called once per frame
    void Update()
    {
        t -= Time.deltaTime;
        if (t <= 0)
        {
            Explode();
        }
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        for (int i = 0; i < colliders.Length; i++)
        {
            Destroy(colliders[i].gameObject);
        }
        
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            Explode();
        }
    }
}
