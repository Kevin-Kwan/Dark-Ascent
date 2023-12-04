using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingDonut : MonoBehaviour
{
    public float hoverDistance = 0.5f; // Adjust this to change the hovering distance
    public float hoverSpeed = 1.5f; // Adjust this to change the speed of hovering

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        // Calculate the new Y position using a sine wave to create the hovering effect
        float newY = initialPosition.y + Mathf.Sin(Time.time * hoverSpeed) * hoverDistance;

        // Update the object's position
        transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);
    }
}

