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
    public GameObject explosion;

    private Rigidbody rb;
    private float t;

    private float shootGrace = 0.01f;
    private float t2;
    
    // Start is called before the first frame update
    void Start()
    {
        t = time;
        t2 = shootGrace;
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;    
    }

    // Update is called once per frame
    void Update()
    {
        t -= Time.deltaTime;
        t2 -= Time.deltaTime;
        
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
            Health health = colliders[i].GetComponent<Health>();
            if (health != null)
            {
                health.ReduceHealth(damage);
            }
        }

        Instantiate(explosion, transform.position, new Quaternion());
        
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        // if (!other.CompareTag("Player"))
        if (t2 <= 0 && !other.gameObject.CompareTag("Bumper")) 
        {
            Debug.Log(other);
            Explode();
        }
    }
}
