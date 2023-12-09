using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/// <summary>
/// Morrison, Brooke
/// Melendrez, Servando
/// 12/7/23
/// This script displays the Health on the top right hand corner of the game view
/// </summary>
public class UImanager : MonoBehaviour
{
    public playerController playerController;
    public WaveSpawner wavespawner;
    public TMP_Text healthDisplay;
    public TMP_Text nextWaveDisplay;

    // Update is called once per frame
    void Update()
    {
        if (playerController != null)
        {
            healthDisplay.text = "Player Health: " + playerController.totalHealth;
        }
        else
        {
            // Handle the case where playerController is null
            healthDisplay.text = "Player Health: N/A";
        }

        if (wavespawner != null)
        {
            int scoreLeft = wavespawner.scoreToStopSpawning - wavespawner.currentScore;
            nextWaveDisplay.text = "Next Wave: " + scoreLeft.ToString();
        }
        else
        {
            // Handle the case where wavespawner is null
            nextWaveDisplay.text = "Next Wave: N/A";
        }
    }
}
