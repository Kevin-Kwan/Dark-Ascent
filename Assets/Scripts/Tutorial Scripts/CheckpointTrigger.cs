/*
 * File: CheckpointManager.cs
 * Author: Akhilesh Sivaganesan
 * Created: 10/16/2023
 * Modified: 10/29/2023
 * Description: Checkpoint trigger used to test respawning in tutorial level. 
 * 
 * Contributions:
 *   Akhilesh Sivganesan:
 *    - Set the checkpoint using the checkpoint manager
 */
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    public CheckpointManager checkpointManager; // Assign the Checkpoint Manager in the Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            checkpointManager.SetCheckpoint(transform); // Notify the checkpoint manager to set the checkpoint
            Debug.Log("Checkpoint triggered!"); // Add a debug statement
        }
    }
}
