/*
 * File: CheckpointHandler.cs
 * Author: Kevin Kwan
 * Created: 10/16/2023
 * Modified: 10/16/2023
 * Description: This script handles the loading and storage of player's checkpoints in the game.
 * The last checkpoint that the player has reached is stored in PlayerPrefs to be loaded whenever the player continues the game.
 * The last level that the player has reached is also stored in PlayerPrefs to be loaded whenever the player continues the game.
 * Whenever the player dies, the player is respawned at the last checkpoint that they have reached.
 * If the player skips a checkpoint but reaches a later checkpoint, the player will respawn at the later checkpoint.
 * When the player reaches the last checkpoint, the player will be respawned at the first checkpoint of the next level.
 * Use the ExampleCheckpointPrefab as a template for creating checkpoints.
 * Start and end checkpoints are required for the script to work. These are treated as checkpoints as well.
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointHandler : MonoBehaviour
{
    public GameObject startCheckpoint; // starts at index 0
    public GameObject endCheckpoint;
    public GameObject[] checkpoints;
    public GameObject player;
    public bool mustTouchAllCheckpoints;
    public string nextSceneName;
    public bool loadFromStoredData;
    public int currentLevelIndex = 1; // starts at index 1

    private int currentCheckpointIndex = 0;

    private void Start()
    {
        Debug.Log(PlayerPrefs.GetInt("CurrentLevelIndex", 1));
        Debug.Log(PlayerPrefs.GetInt("CurrentCheckpointIndex", 0));
        // player = GameObject.FindGameObjectWithTag("Player");

        // Add the start and end checkpoints to the checkpoints array
        GameObject[] tempArray = new GameObject[checkpoints.Length + 2];
        tempArray[0] = startCheckpoint;
        tempArray[tempArray.Length - 1] = endCheckpoint;
        checkpoints.CopyTo(tempArray, 1);
        checkpoints = tempArray;

        // Check if preferences exist for level
        // if they don't, store the first level and checkpoint
        if (!PlayerPrefs.HasKey("CurrentLevelIndex"))
        {
            PlayerPrefs.SetInt("CurrentLevelIndex", currentLevelIndex);
            PlayerPrefs.SetInt("CurrentCheckpointIndex", 0);
        }

        if (loadFromStoredData)
        {
            // Load the current checkpoint index and scene index from stored data
            currentCheckpointIndex = PlayerPrefs.GetInt("CurrentCheckpointIndex", 0);
            SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentLevelIndex", 1));
        }
        RespawnPlayer();
    }

    private void Update()
    {
        for (int i = currentCheckpointIndex; i < checkpoints.Length; i++)
        {
            GameObject checkpoint = checkpoints[i];
            BoxCollider boxCollider = checkpoint.transform.Find("TriggerArea").GetComponent<BoxCollider>();
            Vector3 boxSize = boxCollider.size;

            if (Physics.CheckBox(checkpoint.transform.position, boxSize, checkpoint.transform.rotation, 1 << LayerMask.NameToLayer("Player")))
            {
                currentCheckpointIndex = i;
                PlayerPrefs.SetInt("CurrentCheckpointIndex", currentCheckpointIndex);
                Debug.Log("Checkpoint " + currentCheckpointIndex + " reached!");
                break;
            }
        }

        if (currentCheckpointIndex == checkpoints.Length - 1)
        {
            // The player has reached the end checkpoint
            PlayerPrefs.SetInt("CurrentLevelIndex", currentLevelIndex + 1);
            // Reset the current checkpoint index
            PlayerPrefs.SetInt("CurrentCheckpointIndex", 0);
            Debug.Log("You have reached the end of the level!");
            Cursor.lockState = CursorLockMode.None;
            player.GetComponent<ThirdPController>().enabled = false;
            // bring up a menu here before loading the next scene?
            SceneManager.LoadScene(nextSceneName);
        }

        // If the 'R' key is pressed, respawn the player
        if (Input.GetKeyDown(KeyCode.R))
        {
            RespawnPlayer();
        }

        // If the 'P' key is pressed, reset the playerprefs to current level and start checkpoint
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerPrefs.SetInt("CurrentLevelIndex", currentLevelIndex);
            PlayerPrefs.SetInt("CurrentCheckpointIndex", 0);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void RespawnPlayer()
    {
        // Respawn the player at the current checkpoint's TriggerArea
        GameObject currentCheckpoint = checkpoints[currentCheckpointIndex];
        GameObject triggerArea = currentCheckpoint.transform.Find("TriggerArea").gameObject;

        // Disable the player's CharacterController component to allow for position updates
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = triggerArea.transform.position;
        player.GetComponent<CharacterController>().enabled = true;

    }
}