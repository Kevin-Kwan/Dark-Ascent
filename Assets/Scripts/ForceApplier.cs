using UnityEngine;

public class ForceApplier : MonoBehaviour
{
    public float forwardSpeed = 5.0f; // Adjust the speed as needed

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger is the player (assuming it has a Rigidbody).
        if (other.CompareTag("Player"))
        {
            Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();

            if (playerRigidbody != null)
            {
                // Set the player's velocity to move forward.
                Vector3 playerVelocity = playerRigidbody.velocity;
                playerVelocity.z = forwardSpeed; // Set the forward speed.
                playerRigidbody.velocity = playerVelocity;
            }
        }
    }
}
