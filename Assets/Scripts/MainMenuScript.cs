/*
 * File: MainMenuScript.cs
 * Authors: Kevin Kwan
 * Created: 09/18/2022
 * Modified: 10/27/2023
 * Description: This script handles the main menu of the game and all button interactions.
 *
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuScript : MonoBehaviour
{   
    public GameObject mainMenuPanel;
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject creditsMenu;
    public GameObject quitConfirmationMenu;
    public string firstLevelName = "Level1";
    // start game button
    public GameObject startGameButton;
    public GameObject settingsButton;
    public GameObject creditsButton;
    public GameObject quitButton;
    // Start is called before the first frame update
    void Start()
    {
        // This dynamically changes the text of the start game button depending on whether or not the player has saved data
        TextMeshProUGUI startGameButtonText = startGameButton.GetComponentInChildren<TextMeshProUGUI>();
        if (!PlayerPrefs.HasKey("CurrentLevelIndex"))
        {
            startGameButtonText.text = "Start New Game";
        } else {
            startGameButtonText.text = "Continue Game";   
        }
    }

    // Update is called once per frame
    void Update()
    {

        // debugging dev tool
        if (Input.GetKeyDown(KeyCode.I))
        {
            // clear all playerprefs, debugging purposes only
            PlayerPrefs.DeleteAll();
            Debug.Log("PlayerPrefs cleared!");
        }
    }

    public void ContinueGame() {
        // load level from playerpreferences
        // if no level saved, load level 1
        if (!PlayerPrefs.HasKey("CurrentLevelIndex")) {
            Debug.Log("No level saved. Loading level 1.");
            PlayerPrefs.SetInt("CurrentLevelIndex", 1);
            PlayerPrefs.SetInt("CurrentCheckpointIndex", 0);
            SceneManager.LoadScene(firstLevelName);
        } else {
            Debug.Log("Loading level " + PlayerPrefs.GetInt("CurrentLevelIndex", 1));
            SceneManager.LoadScene("Level" + PlayerPrefs.GetInt("CurrentLevelIndex", 1));
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
