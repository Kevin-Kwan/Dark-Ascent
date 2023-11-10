using UnityEngine;

public class DespawnObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object has a specific tag or component if needed
        if (other.CompareTag("Hazard"))
        {
            Destroy(other.gameObject);
            Debug.Log("Object Destroyed!");
        }
    }
}
