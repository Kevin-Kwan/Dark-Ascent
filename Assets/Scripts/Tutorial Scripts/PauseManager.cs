using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu; // Assign the pause menu panel in the Inspector

    private bool isPaused = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; // Make the cursor visible
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
    }
}
