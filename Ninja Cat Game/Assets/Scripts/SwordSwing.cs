using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSwing : MonoBehaviour
{
    public Animator swordAnimator;
    public int damageAmount = 1;

    bool isSwinging = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isSwinging)
        {
            SwingSword();
        }
    }

    void SwingSword()
    {
        isSwinging = true;
        swordAnimator.SetTrigger("Swing");

        // Play audio for sword swing sound effect (if applicable)
        // GetComponent<AudioSource>().Play(); // Add audio source to the sword GameObject
    }

    // OnTriggerEnter to detect collision with enemies during swing
    void OnTriggerEnter(Collider other)
    {
        if (isSwinging && other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount);
            }
        }
    }

    // This method is called at the end of the sword swing animation in the Animator
    public void SwordSwingEnd()
    {
        isSwinging = false;
    }
}
