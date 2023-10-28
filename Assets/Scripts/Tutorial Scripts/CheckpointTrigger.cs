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
