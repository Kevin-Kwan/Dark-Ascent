/*
 * File: CheckpointManager.cs
 * Author: Akhilesh Sivaganesan
 * Created: 10/16/2023
 * Modified: 10/29/2023
 * Description: Old checkpoint manager used to test respawning in tutorial level. 
 * 
 * Contributions:
 *   Akhilesh Sivganesan:
 *    - Add Pause manager integration
 *    - Add resume button on click function
 */
using UnityEngine;
using UnityEngine.UI;

public class ResumeButtonScript : MonoBehaviour
{
    // Reference to the pause manager or your game manager
    public PauseManager pauseManager; // You may need to customize this based on your project structure

    // Add an OnClick event to the button in the Inspector and link it to this function
    public void OnResumeButtonClick()
    {
        if (pauseManager != null)
        {
            // Call the ResumeGame function in your PauseManager or GameManager script
            pauseManager.ResumeGame();
        }
    }
}
