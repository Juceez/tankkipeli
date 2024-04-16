using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{

    public float maxHealth = 3f;
    private float currentHealth;
    private float damageFlashTime = 0.5f;
    public Color damageColor = Color.red;
    public GameObject explosion;
    private Color originalColor;
    private Color originalEmissionCol;
    private float t;

    private MeshRenderer[] meshRenderers;
    
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        originalColor = meshRenderers[0].material.color;
        originalEmissionCol = meshRenderers[0].material.GetColor("_EmissionColor");
    }

    public void ReduceHealth(float damage)
    {
        StartCoroutine(DamageFlash());
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Instantiate(explosion, transform.position, new Quaternion());
            Destroy(gameObject);
        }
    }

    private IEnumerator DamageFlash()
    {
        t = damageFlashTime;
        while (t > 0)
        {
            t -= Time.deltaTime;

            Color newColor = Color.Lerp(originalColor, damageColor, t / damageFlashTime);
            Color newEmColor = Color.Lerp(originalEmissionCol, damageColor, t / damageFlashTime);
            
            foreach (MeshRenderer meshr in meshRenderers)
            {
                meshr.material.color = newColor;
                meshr.material.SetColor("_EmissionColor", newEmColor);
            }
            yield return null;
        }

        foreach (MeshRenderer meshr in meshRenderers)
        {
            meshr.material.color = originalColor;
            meshr.material.SetColor("_EmissionColor", originalEmissionCol);
        }
    }
}
