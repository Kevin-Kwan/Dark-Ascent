/*
 * File: MainMenuScript.cs
 * Authors: Kevin Kwan
 * Created: 10/27/2023
 * Modified: 10/27/2023
 * Description: This script handles the main menu of the game and all button interactions.
 * This script also handles the loading of the first level of the game if the player hasn't played before.
 * This script also handles the loading of the last level that the player has reached if the player has played before.
 * The main menu includes the loading and unloading of screens such as the settings menu, credits menu, and quit confirmation menu.
 * 
 *  * Contributions:
 *   Kevin Kwan:
 *    - Main Menu Functionality
 *   Connor Sugasawara:
 *    - Reset Progress Button
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
    public GameObject resetConfirmationMenu;
    public string firstLevelName = "Level1";
    // start game button
    public GameObject startGameButton;
    public GameObject settingsButton;
    public GameObject creditsButton;
    public GameObject quitButton;
    public GameObject resetButton;
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

    public void showMainMenu() {
        mainMenuPanel.SetActive(true);
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        quitConfirmationMenu.SetActive(false);
        resetConfirmationMenu.SetActive(false);
    }
    public void showSettingsMenu() {
        mainMenuPanel.SetActive(true);
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
        creditsMenu.SetActive(false);
        quitConfirmationMenu.SetActive(false);
        resetConfirmationMenu.SetActive(false);
    }
    public void showCreditsMenu() {
        mainMenuPanel.SetActive(false);
        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        creditsMenu.SetActive(true);
        quitConfirmationMenu.SetActive(false);
        resetConfirmationMenu.SetActive(false);
    }
    public void showQuitConfirmationMenu() {
        mainMenuPanel.SetActive(true);
        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        quitConfirmationMenu.SetActive(true);
        resetConfirmationMenu.SetActive(false);
    }

    public void showResetConfirmationMenu()
    {
        mainMenuPanel.SetActive(true);
        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        resetConfirmationMenu.SetActive(true);
    }

    public void ResetProgress()
    {
        PlayerPrefs.SetInt("CurrentLevelIndex", 1);
        PlayerPrefs.SetInt("CurrentCheckpointIndex", 0);
        TextMeshProUGUI startGameButtonText = startGameButton.GetComponentInChildren<TextMeshProUGUI>();
        startGameButtonText.text = "Start New Game";
        showSettingsMenu();
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
