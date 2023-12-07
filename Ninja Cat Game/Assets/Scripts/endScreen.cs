using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Morrison,Brooke 
/// Melendrez, Servando
/// 10/31/23
/// This script controls the main game scene and game over screen loading when the appropriate buttons are pressed
/// </summary>
public class EndScreen : MonoBehaviour
{
    /// <summary>
    /// Quits the game.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
    /// <summary>
    /// Changes the currrent scene to the scene with a matching index 
    /// </summary>
    /// <param name="sceneIndex">The Index of the scene to switch to</param>
    public void SwitchScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
