using UnityEngine;
using UnityEngine.UI;

public class DeathCollider : MonoBehaviour
{
    public GameObject deathScreenPanel; // Assign the Death Screen UI panel in the Inspector
    private bool isPlayerDead = false;
    public Transform playerTransform;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; // Make the cursor visible

        // Make sure the death menu is initially inactive
        deathScreenPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPlayerDead)
        {
            // Check if the collision was with the player character and the player is not already dead
            //Time.timeScale = 0;
            Debug.Log("Entered lava bed");
            ActivateDeathScreen();
        }
    }

    private void ActivateDeathScreen()
    {
        if (deathScreenPanel != null)
        {
            deathScreenPanel.SetActive(true);

            // Disable other UI elements in the same Canvas
            Canvas canvas = deathScreenPanel.GetComponentInParent<Canvas>();
            foreach (Transform child in canvas.transform)
            {
                if (child.gameObject != deathScreenPanel)
                {
                    child.gameObject.SetActive(false);
                }
            }

            // You can also add additional logic here, such as pausing the game or displaying a game over message.

            isPlayerDead = true; // Set the flag to prevent multiple activations of the death screen
        }
        RespawnPlayer();
    }

    public void RespawnPlayer()
    {
        // Deactivate the death screen
        if (deathScreenPanel != null)
        {
            deathScreenPanel.SetActive(false);
        }
        //Time.timeScale = 1;
        playerTransform.position = new Vector3(0f,1f,-4.11f);
        Debug.Log("Finished respawning");


    }
}
