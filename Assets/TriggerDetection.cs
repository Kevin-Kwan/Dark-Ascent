using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other) {
        // if trigger entered has tag "Elevator"
        Debug.Log("Triggered elevator");
        if (other.gameObject.CompareTag("Elevator")) {
            // set player's parent to the elevator
            transform.parent.SetParent(other.transform.parent);
        }
    }
    void OnTriggerExit(Collider other) {
        // if trigger exited has tag "Elevator"
        if (other.gameObject.CompareTag("Elevator")) {
            // set player's parent to null
            transform.parent.SetParent(null);
        }
    }
}
