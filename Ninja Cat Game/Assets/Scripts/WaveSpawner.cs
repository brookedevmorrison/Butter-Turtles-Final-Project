using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveSpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // The prefab of the regular enemy to spawn
    public GameObject bossEnemyPrefab; // The prefab of the boss enemy to spawn
    public Transform playerWaypoint; // Player's waypoint

    public int enemiesPerWave = 5; // Number of enemies per wave
    public int numberOfWaves = 3; // Number of regular waves before boss
    public float waveDelay = 5.0f; // Delay between waves
    public int scoreToStopSpawning = 20; // Score to stop spawning regular enemies
    public int bossEnemyScore = 21; // Score threshold to spawn the boss

    public int currentScore = 0;
    private int currentWave = 0;
    private bool canSpawn = true;

    IEnumerator Start()
    {
        while (canSpawn)
        {
            if (currentWave < numberOfWaves)
            {
                yield return StartCoroutine(SpawnEnemyWave());
                yield return new WaitForSeconds(waveDelay);
            }
            else if (currentScore >= bossEnemyScore)
            {
                SpawnBossEnemy();
                canSpawn = false; // Stop regular enemy spawning after boss spawns
            }
            else
            {
                yield return null; // Wait without spawning until boss or score requirement is met
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
        currentWave++;
    }

    void SpawnEnemy()
    {
        if (enemyPrefab != null && playerWaypoint != null)
        {
            GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            enemy.GetComponent<EnemyMovement>().playerWaypoint = playerWaypoint;

            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.onDeath.AddListener(OnEnemyDefeated);
            }
        }
    }

    void SpawnBossEnemy()
    {
        if (bossEnemyPrefab != null && playerWaypoint != null)
        {
            GameObject bossEnemy = Instantiate(bossEnemyPrefab, transform.position, Quaternion.identity);
            bossEnemy.GetComponent<EnemyMovement>().playerWaypoint = playerWaypoint;

            EnemyHealth bossHealth = bossEnemy.GetComponent<EnemyHealth>();
            if (bossHealth != null)
            {
                bossHealth.onDeath.AddListener(OnBossDefeated);
            }
        }
    }

    void OnEnemyDefeated()
    {
        // Handle any logic upon regular enemy defeat (if needed)
    }

    void OnBossDefeated()
    {
        SceneManager.LoadScene(3); // Load Scene 3 when the boss is defeated
    }

    // Call this method when the player's score increases
    public void IncreaseScore(int amount)
    {
        currentScore += amount;
        Debug.Log("Current score: " + currentScore);
    }
}
