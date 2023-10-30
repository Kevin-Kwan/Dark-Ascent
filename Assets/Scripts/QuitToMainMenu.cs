/*
 * File: CheckpointManager.cs
 * Author: Akhilesh Sivaganesan
 * Created: 10/27/2023
 * Modified: 10/29/2023
 * Description: Simple function for quitting to main menu from pause menu
 * 
 * Contributions:
 *   Akhilesh Sivganesan:
 *    - Add on click function
 *    - Cursor should be not locked when going to main menu
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class QuitToMainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void goToMain() {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("MainMenu");
    }
}
