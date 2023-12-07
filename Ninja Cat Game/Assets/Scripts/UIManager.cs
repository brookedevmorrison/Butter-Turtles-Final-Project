using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/// <summary>
/// Morrison, Brooke
/// Melendrez, Servando
/// 10/23/23
/// This script displays the Health on the top right hand corner of the game view
/// </summary>
public class UImanager : MonoBehaviour
{
    public playerController playerController;
    public TMP_Text healthDisplay;

    // Update is called once per frame
    void Update()
    {
        healthDisplay.text = "Health: " + playerController.totalHealth;
    }
}
