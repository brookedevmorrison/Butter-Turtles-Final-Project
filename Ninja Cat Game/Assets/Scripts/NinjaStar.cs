using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaStar : MonoBehaviour
{
    public int damage = 1;
    private bool hasDamaged = false; // Checks if damage has been applied

    // Start is called on the frame when this script is enabled
    void Start()
    {
        // Destroy the ninja star after 3 seconds
        Destroy(gameObject, 3f);
    }

    /// <summary>
    /// Deals with Enemy Damage
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (!hasDamaged && (other.CompareTag("Enemy") || other.CompareTag("bossenemy")))
        {
            // Apply damage to the enemy
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                hasDamaged = true;
            }

            // Destroy the ninja star
            Destroy(gameObject);
        }
    }
}
