using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // The prefab of the enemy to spawn
    public Transform playerWaypoint; // Player's waypoint

    public int enemiesPerWave = 5; // Number of enemies per wave
    public float waveDelay = 5.0f; // Delay between waves
    public int scoreToStopSpawning = 20; // Score to stop spawning enemies

    private int currentScore = 0;
    private bool canSpawn = true;

    IEnumerator Start()
    {
        while (canSpawn)
        {
            yield return StartCoroutine(SpawnEnemyWave());
            yield return new WaitForSeconds(waveDelay);

            // Check if the score has reached the limit to stop spawning enemies
            if (currentScore >= scoreToStopSpawning)
            {
                canSpawn = false;
                Debug.Log("Reached score limit. Stopped spawning enemies.");
            }
        }
    }

    IEnumerator SpawnEnemyWave()
    {
        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(1.0f); // Adjust delay between individual enemies in the wave if needed
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefab != null && playerWaypoint != null)
        {
            GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            enemy.GetComponent<EnemyMovement>().playerWaypoint = playerWaypoint;
            // You might want to set other properties or behaviors for the enemy here
        }
    }

    // Call this method when the player's score increases
    public void IncreaseScore(int amount)
    {
        currentScore += amount;
        Debug.Log("Current score: " + currentScore);
    }
}
