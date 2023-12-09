using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public int scoreValue = 10; // Score value awarded upon enemy death
    public UnityEvent onDeath; // UnityEvent for handling enemy's death

    void Start()
    {
        currentHealth = maxHealth;
        if (onDeath == null)
        {
            onDeath = new UnityEvent();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Trigger the UnityEvent for handling enemy's death
        onDeath.Invoke();

        // Add score to the player
        AddScore();

        // Destroy the enemy GameObject
        Destroy(gameObject);
    }

    void AddScore()
    {
        WaveSpawner waveSpawner = FindObjectOfType<WaveSpawner>();

        if (waveSpawner != null)
        {
            // Increase player's score by the scoreValue when an enemy dies
            waveSpawner.IncreaseScore(scoreValue);
        }
        else
        {
            Debug.LogWarning("WaveSpawner script not found!");
        }
    }
}
