using UnityEngine;

public class ProximityTrigger : MonoBehaviour
{
    public GameObject popUpPanel; // Reference to the associated pop-up panel

    private bool playerInTrigger = false;

    void Start()
    {
        // Deactivate the pop-up panel when the game starts
        popUpPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            popUpPanel.SetActive(true); // Activate the pop-up panel when the player enters
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            popUpPanel.SetActive(false); // Deactivate the pop-up panel when the player exits
        }
    }
}
