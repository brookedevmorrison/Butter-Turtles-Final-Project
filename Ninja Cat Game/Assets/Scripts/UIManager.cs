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
    public EnemyHealth EnemyHealth;
    public TMP_Text healthDisplay;
    public TMP_Text enemyHealthDisplay;

    // Update is called once per frame
    void Update()
    {
        healthDisplay.text = "Player Health: " + playerController.totalHealth;
        enemyHealthDisplay.text = "Enemy Health: " + playerController.maxHealth;
    }
}
