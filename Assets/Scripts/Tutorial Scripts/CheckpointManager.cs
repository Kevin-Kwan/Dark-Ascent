using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    // Store the player character's Transform
    public Transform playerTransform;

    // Store the last checkpoint position
    public Vector3 lastCheckpointPosition;

    // Set an initial checkpoint position (you can also set this in the Inspector)
    private void Start()
    {
        lastCheckpointPosition = playerTransform.position;
        Debug.Log("Initial checkpoint was set here " + playerTransform.position);

    }

    // Triggered when the player reaches a checkpoint
    public void SetCheckpoint(Transform checkpoint)
    {
        Debug.Log("checkpoint was set here");
        lastCheckpointPosition = checkpoint.position;
    }

    // Called to respawn the player at the last checkpoint
    public void RespawnPlayer()
    {
        // Add debug statements
        Debug.Log("RespawnPlayer method called.");
        Debug.Log("playerTransform is: " + playerTransform);
        Debug.Log("lastCheckpointPosition is: " + lastCheckpointPosition);
        Time.timeScale = 1;

        // Set the player's position to last checkpoint
        if (playerTransform != null)
        {
            playerTransform.position = lastCheckpointPosition;
            Debug.Log("Player position set to last checkpoint: " + lastCheckpointPosition);
        }

    }
}
