/*
 * File: PauseManager.cs
 * Authors: Akhilesh Sivaganesan, Kevin Kwan
 * Created: 10/28/2023
 * Modified: 10/29/2023
 * Description: This script is used to pause the game and display the pause menu.
 * It is attached to the PauseManager game object in the Canvas.
 */
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu; // Assign the pause menu panel in the Inspector

    public GameObject player;

    private bool isPaused = false;

    private void Start()
    {
        player = GameObject.Find("3rdPPlayer");
        // Kevin: This interferes with the cursor lock state in the PlayerController script
        // On start, the cursor should be locked and invisible.
        // The cursor only becomes visible when the game is paused or the player has died.
        
        // Cursor.lockState = CursorLockMode.None;
        // Cursor.visible = true; // Make the cursor visible
        // Make sure the pause menu is initially inactive
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        // Activate the pause menu
        pauseMenu.SetActive(true);

        // Disable other screens in the canvas
        foreach (Transform child in transform)
        {
            if (child.gameObject != pauseMenu)
            {
                child.gameObject.SetActive(false);
            }
        }

        // Pause the game by setting the time scale to 0
        Time.timeScale = 0;

        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        // Prevent the player from attacking while in the Pause Menu
        player.GetComponent<ThirdPController>().allowPlayerClick = false;

    }

    public void ResumeGame()
    {
        // Deactivate the pause menu
        pauseMenu.SetActive(false);

        // Enable other screens in the canvas
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }

        // Resume the game by setting the time scale back to 1
        Time.timeScale = 1;

        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        // Prevent the player from attacking while in the Pause Menu
        player.GetComponent<ThirdPController>().allowPlayerClick = true;
    }
}
